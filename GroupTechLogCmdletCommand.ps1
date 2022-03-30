function Group-TechLog {
    [CmdletBinding()]
    [OutputType([PSCustomObject])]
    Param (
        [Parameter(Mandatory=$true,
                   Position = 0,
                   ValueFromPipeline=$true)]
        [System.Object]$InputObject,

        [Parameter(Mandatory=$true,
                   ValueFromPipeline=$true)]
        [string]$GroupProperty,
        
        [Parameter(Mandatory=$true,
                   ValueFromPipeline=$true)]
        [string]$AggregationProperty
    )

    begin {
        $inputObjects = New-Object 'Collections.Generic.Dictionary[string,object]'
    }

    process {
        $group = [string]$InputObject."$GroupProperty";

        if ([string]::IsNullOrEmpty($group)) { 
            return 
        };

        $aggr = [double]$InputObject."$AggregationProperty";

        if ($InputObjects.ContainsKey($group)) {
            $a = $inputObjects[$group];
            $a.Sum += $aggr;
            $a.Count += 1;

            if ($aggr -lt $a.Min) {
                $a.Min = $aggr
            }

            if ($aggr -gt $a.Max) {
                $a.Max = $aggr
            }
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