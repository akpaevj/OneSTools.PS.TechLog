﻿using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OneSTools.PS.TechLog
{
    public class LocksInfoRegion
    {
        public string Region { get; }
        public string BlockingMode { get; }
        public List<(string Field, string Values)> FieldValues { get; } = new List<(string, string)>();
        public string Hash { get; }

        public LocksInfoRegion(string locks)
        {
            Hash = TjEvent.GetMd5Hash(locks);

            Region = Regex.Match(locks, @"^\w+\.\w+", RegexOptions.ExplicitCapture).Value;
            BlockingMode = Regex.Match(locks, @"(?<=^\w+\.\w+ ).*?(?= )", RegexOptions.ExplicitCapture).Value;

            var fields = Regex.Matches(locks, @"\w+=.*?(?=( \w+=|$))", RegexOptions.ExplicitCapture);

            foreach (Match fieldMatch in fields)
            {
                var fieldData = fieldMatch.Value;

                var splitIndex = fieldData.IndexOf('=');
                var field = fieldData.Substring(0, splitIndex);
                var value = fieldData.Substring(splitIndex + 1);

                FieldValues.Add((field, value.Trim('"')));
            }
        }
    }
}