# OneSTools.PS.TechLog

![PowerShell Gallery Version](https://img.shields.io/powershellgallery/v/OneSTools.PS.TechLog?style=plastic)

PowerShell commandlet for parsing of 1C technological log.  

Parsed event (TjEvent class) already contains commonly used properties that can be used without additional parsing actions from user side 

## Installation

```powershell
Install-Module -Name OneSTools.PS.TechLog
```

## Usage example

```powershell
Get-TechLog "C:\Users\akpaev.e\Desktop\*.log" | ForEach-Object { $aggr[$_.FirstContextLine] += $_.Duration }

$aggr.GetEnumerator() | sort Value -Descending | where Name -ne "" | select -First 10 | Format-Table -AutoSize
```
