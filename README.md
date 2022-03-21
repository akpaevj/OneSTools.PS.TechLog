# OneSTools.PS.TechLog

PowerShell commandlet for parsing of 1C technological log.  

Parsed event (TjEvent class) already contains commonly used properties without additional parsing actions from user side 

## Usage example

```powershell
Get-TechLog "C:\Users\akpaev.e\Desktop\*.log" | ForEach-Object { $aggr[$_.FirstContextLine] += $_.Duration }

$aggr.GetEnumerator() | sort Value -Descending | where Name -ne "" | select -First 10 | Format-Table -AutoSize
```
