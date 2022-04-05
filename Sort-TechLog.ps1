function Sort-TechLog {
    [CmdletBinding()]
    [OutputType([psobject[]])]
    Param (
        [Parameter(Mandatory=$true,
                   ValueFromPipeline=$true)]
        $InputObject,

        [Parameter(Mandatory=$true,
                   Position = 0,
                   ValueFromPipelineByPropertyName=$true,
                   HelpMessage = "Property name that will be used for the comparison")]
        $Property,
        
        [Parameter(Mandatory=$true,
                   HelpMessage = "Max quantity of elements should be taken")]
        [int]$Top,

        [switch]$Descending
    )

    begin {
        $Script:currentCount = 0
        $Script:propertyIsScriptBlock = $property -is [scriptblock]
        $Script:inputObjects = [psobject[]]::new($Top)
    }

    process {
        if ($currentCount -lt $inputObjects.Count) {
            $inputObjects[$currentCount] = $InputObject

            $currentCount++

            if ($currentCount -eq $inputObjects.Count) {
                if ($Descending) {
                    $inputObjects = $inputObjects | Sort-Object $Property -Descending
                }
                else {
                    $inputObjects = $inputObjects | Sort-Object $Property
                }
            }
        }
        else {
            for ($i = 0; $i -lt $inputObjects.Count; $i++) {
                $item = $inputObjects[$i]

                $inputValue = $InputObject."$Property"
                $itemValue = $item."$Property"

                if ($Descending) {
                    $needShift = $inputValue -ge $itemValue
                } else {
                    $needShift = $inputValue -le $itemValue
                }

                if ($needShift) {
                    # shift other items to the right
                    for ($y = $inputObjects.Count - 2; $y -ge $i; $y--) {
                        $inputObjects[$y + 1] = $inputObjects[$y]
                    }

                    $inputObjects[$i] = $InputObject

                    break
                }
            }
        }
    }

    end {
        if ($Descending) {
            $inputObjects | Sort-Object $Property -Descending
        }
        else {
            $inputObjects | Sort-Object $Property
        }
    }
}