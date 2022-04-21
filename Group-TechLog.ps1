function Group-TechLog {
    [CmdletBinding()]
    [OutputType([PSCustomObject])]
    Param (
        [Parameter(Mandatory=$true,
                   Position = 0,
                   ValueFromPipeline=$true)]
        [OneSTools.PS.TechLog.TjEvent]$InputObject,

        [Parameter(Mandatory=$true,
                   ValueFromPipeline=$true)]
        [Alias("GP")]
        $GroupProperty,
        
        [Parameter(Mandatory=$true,
                   ValueFromPipeline=$true)]
        [Alias("AP")]
        $AggregationProperty
    )

    begin {
        $property = [OneSTools.PS.TechLog.TjEvent].GetProperty("$GroupProperty")
        if ($null -eq $property) {
            throw "TjEvent type doesn't contain property that's named $GroupProperty"
        } else {
            $propertyType = $property.PropertyType.FullName
            Write-Verbose("Grouping property type is $propertyType")

            $inputObjects = New-Object "Collections.Generic.Dictionary[$propertyType,object]"
        }
    }

    process {
        $group = $InputObject."$GroupProperty";

        if ([string]::IsNullOrEmpty($group)) { 
            return 
        };

        $aggr = [double]$InputObject."$AggregationProperty";

        if ($InputObjects.ContainsKey($group)) {
            $a = $inputObjects[$group];
            $a.Sum += $aggr;
            $a.Count += 1;
            $a.Min = [math]::Min($a.Min, $aggr);
            $a.Max = [math]::Max($a.Max, $aggr);
        }
        else {
            $inputObjects.Add($group, @{Sum = $aggr; Count = 1; Min = $aggr; Max = $aggr});
        }
    }

    end {
        foreach($kv in $inputObjects.GetEnumerator()) {
            $v = $kv.Value; 
            [PSCustomObject]@{
                Name = $kv.Key
                Sum = $v.Sum
                Avg = $v.Sum / $v.Count
                Min = $v.Min
                Max = $v.Max
                Count = $v.Count
            }
        }
    }
}