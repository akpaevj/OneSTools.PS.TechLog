using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Management.Automation;
using System.Linq;
using System.Text.RegularExpressions;

namespace OneSTools.PS.TechLog
{
    [Cmdlet(VerbsCommon.Get, "TechLog")]
    [OutputType(typeof(TjEvent))]
    public class GetTechLogCmdlet : PSCmdlet
    {
        private readonly Dictionary<string, StreamReader> readers = new Dictionary<string, StreamReader>();

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public string[] Path { get; set; }

        protected override void BeginProcessing()
        {
            var allFiles = new List<string>();

            foreach(var path in Path) {
                var files = SessionState.Path.GetResolvedPSPathFromPSPath(path).Select(c => c.Path);
                allFiles.AddRange(files);
            }

            foreach (var logFile in allFiles)
            {
                WriteVerbose($"Creating reader for the file by path {logFile}");

                var stream = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var reader = new StreamReader(stream);
                readers.Add(logFile, reader);
            }
        }
        
        public void BeginProcessingInternal()
        {
            BeginProcessing();
        }
        
        protected override void ProcessRecord()
        {
            foreach (var kv in readers)
            {
                WriteVerbose($"Reading of {kv.Key} is started");
                
                StartReading(kv.Key, kv.Value);
            }
        }
        
        public void ProcessRecordInternal()
        {
            ProcessRecord();
        }
        
        private void StartReading(string path, StreamReader reader)
        {
            var fileDateTime = GetFileDateTime(path) + ":";
            reader.BaseStream.Seek(3, SeekOrigin.Current);

            var eventBuffer = new StringBuilder();

            while (!reader.EndOfStream) {
                var line = reader.ReadLine();

                if (line?.Length > 15 && line[2] == ':' && line[5] == '.' && line[12] == '-') {
                    if (eventBuffer.Length > 0)
                    {
                        var tjEvent = ParseTjEventData(eventBuffer.ToString());
                        WriteObject(tjEvent);
                        eventBuffer.Clear();
                    }

                    eventBuffer.Append(fileDateTime);      
                }
                else
                    eventBuffer.Append('\n');

                eventBuffer.Append(line);
            }

            if (eventBuffer.Length > 0) 
            {
                var tjEvent = ParseTjEventData(eventBuffer.ToString());
                WriteObject(tjEvent);
            }
        }
        
        string GetFileDateTime(string filePath) {
            var info = new FileInfo(filePath);

            var year = info.Name.Substring(0, 2);
            var month = info.Name.Substring(2, 2);
            var day = info.Name.Substring(4, 2);
            var hour = info.Name.Substring(6, 2);

            return $"20{year}-{month}-{day} {hour}";
        }

        private TjEvent ParseTjEventData(string eventData) 
        {
            var eventDateTime = DateTime.Parse(eventData.Substring(0, 26));

            var durationStart = 27;
            var durationEnd = eventData.IndexOf(',');
            var duration = long.Parse(eventData.Substring(durationStart, durationEnd - durationStart));

            var eventNameStart = durationEnd + 1;
            var eventNameEnd = eventData.IndexOf(',', eventNameStart + 1);
            var eventName = eventData.Substring(eventNameStart, eventNameEnd - eventNameStart);

            var levelStart = eventNameEnd + 1;
            var levelEnd = eventData.IndexOf(',', levelStart + 1);
            var level = int.Parse(eventData.Substring(levelStart, levelEnd - levelStart));

            var properties = ParseEventProperties(eventData, levelEnd + 1);

            var ev = new TjEvent() 
            {
                DateTime = eventDateTime,
                Duration = duration,
                EventName = eventName.ToString(),
                Level = level,
                Properties = properties
            };

            return ev;
        }
        
        private Dictionary<string, string> ParseEventProperties(string eventData, int startIndex) 
        {
            var properties = new Dictionary<string, string>();

            while(true) {
                if (startIndex >= eventData.Length)
                    break;

                var propertyNameEnd = eventData.IndexOf('=', startIndex);
                var propertyName = eventData.Substring(startIndex, propertyNameEnd - startIndex);

                var propertyValueStart = propertyNameEnd + 1;
                var propertyValueEnd = eventData.Length;

                var startIndexOffset = 0;

                // Sometimes there is no property value and this property is the last one in the event
                if (propertyValueStart != eventData.Length)
                {
                    switch(eventData[propertyValueStart])
                    {
                        case ',':
                            // there is no value for this property
                            startIndex = propertyValueStart + 1;
                            continue;
                        case '\'':
                            // this is a text value
                            propertyValueStart += 1;
                            propertyValueEnd = eventData.Replace("\'\'", "••")
                                .IndexOf('\'', propertyValueStart);
                            startIndexOffset = 1;
                            break;
                        case '"':
                            // this is a text value
                            propertyValueStart += 1;
                            propertyValueEnd = eventData.Replace("\"\"", "••")
                                .IndexOf('"', propertyValueStart);
                            startIndexOffset = 1;
                            break;
                        default:
                            propertyValueEnd = eventData.IndexOf(',', propertyValueStart);
                            break;
                    }
                }

                if (propertyValueEnd < propertyValueStart)
                    propertyValueEnd = eventData.Length;

                var propertyValue = eventData.Substring(propertyValueStart, propertyValueEnd - propertyValueStart);

                var propertyNameStr = propertyName.ToString();
                var propertyValueStr = propertyValue.ToString();

                AddPropertyValue(properties, propertyNameStr, propertyValueStr);

                startIndex = propertyValueEnd + startIndexOffset + 1;
            }

            return properties;
        }

        private void AddPropertyValue(Dictionary<string, string> properties, string property, string value, int index = 0)
        {
            var propertyName = index == 0 ? property : $"{property}_{index}";

            if (properties.ContainsKey(propertyName))
                AddPropertyValue(properties, property, value, index + 1);
            else
            {
                properties.Add(propertyName, value);
            }
        }
        
        protected override void EndProcessing()
        {
            foreach(var reader in readers.Values)
                reader?.Dispose();

            readers.Clear();
        }
        
        public void EndProcessingInternal()
        {
            EndProcessing();
        }
    }
}
