# OneSTools.PS.TechLog

![PowerShell Gallery Version](https://img.shields.io/powershellgallery/v/OneSTools.PS.TechLog?style=plastic)

PowerShell commandlet for parsing of 1C technological log.  

Parsed event (TjEvent class) already contains commonly used properties that can be used without additional parsing actions from user side. All available properties can be found here: [TjEvent](https://github.com/akpaevj/OneSTools.PS.TechLog/blob/master/TjEvent.cs) 

## Installation

```powershell
Install-Module -Name OneSTools.PS.TechLog
```

## Properties

**Path** - array of paths (it can contains wildcard chars). Works like a *Path* property in Get-Item cmdlet

## Usage example

Calculating sum of duration and grouping by the first line of "Context" property:
```powershell
Get-TechLog "C:\Users\akpaev.e\Desktop\*.log" | ForEach-Object { $aggr[$_.FirstContextLine] += $_.Duration }

$aggr.GetEnumerator() | sort Value -Descending | where Name -ne "" | select -First 10 | Format-Table -AutoSize
```
