Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing

Add-Type -TypeDefinition @"
using System;
using System.Windows.Forms;

public class NoActivateForm : Form
{
    protected override bool ShowWithoutActivation
    {
        get { return true; }
    }

    protected override CreateParams CreateParams
    {
        get
        {
            const int WS_EX_NOACTIVATE = 0x08000000;
            CreateParams cp = base.CreateParams;
            cp.ExStyle |= WS_EX_NOACTIVATE;
            return cp;
        }
    }
}
"@ -ReferencedAssemblies "System.Windows.Forms.dll" -WarningAction SilentlyContinue

$dtpe_SysType = $env:FIRMWARE_TYPE
if (Test-Path -Path "HKLM:\SYSTEM\CurrentControlSet\Control\PXE") {
    $dtpe_SysType += "/PXE"
}
$dtpe_MinVersion = Get-ItemPropertyValue -Path "HKLM:\Software\DISMTools\Preinstallation Environment" -Name "MinBuild"
$dtpe_FullVersion = Get-ItemPropertyValue -Path "HKLM:\Software\DISMTools\Preinstallation Environment" -Name "FullBuild"
$winpeBuild = Get-ItemPropertyValue -Path "HKLM:\Software\Microsoft\Windows NT\CurrentVersion" -Name "BuildLabEx"

$message = "DISMTools $dtpe_MinVersion Preinstallation Environment ($dtpe_SysType)`nComponents Build $($dtpe_FullVersion)`nPreinstallation Environment Build $($winpeBuild)`n$env:SYSTEMROOT"

$form = New-Object NoActivateForm
$form.FormBorderStyle = 'None'
$form.StartPosition = 'Manual'
$form.Location = New-Object System.Drawing.Point(0, 0)
$form.Size = New-Object System.Drawing.Size(640, 128)
$form.BackColor = [System.Drawing.Color]::Black
$form.ForeColor = [System.Drawing.Color]::White
$form.TransparencyKey = [System.Drawing.Color]::Black
$form.Font = New-Object System.Drawing.Font("Segoe UI", 8.75)

$label = New-Object System.Windows.Forms.Label
$label.Text = "$message"
$label.AutoSize = $true
$form.Controls.Add($label)

[System.Windows.Forms.Application]::Run($form)