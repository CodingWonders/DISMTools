function Get-PolicyValue {
    param (
        [Parameter(Mandatory, Position = 0)] [string] $PolicyName,
        [Parameter(Mandatory, Position = 1)] [object] $DefaultPolicyValue,
        [Parameter(Position = 2)] [object[]] $ValidOptions = @()
    )

    $policyRootKey = "HKLM:\SOFTWARE\DISMTools\Preinstallation Environment\Policies"

    if (Test-Path -Path "$policyRootKey") {
        try {
            $value = Get-ItemPropertyValue -Path "$policyRootKey" -Name "$PolicyName" -ErrorAction Stop
            if ($ValidOptions.Count -eq 0) {
                return $value
            } else {
                if ($ValidOptions.Contains($value)) {
                    return $value
                } else {
                    return $DefaultPolicyValue
                }
            }
        } catch {
            return $DefaultPolicyValue
        }
    } else {
        return $DefaultPolicyValue
    }
}