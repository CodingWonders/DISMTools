<#
.SYNOPSIS
    Creates the self-signed HTTPS certificate for the SDS server.

.DESCRIPTION
    Password sign-in from another machine needs HTTPS on the SDS API. This script creates a
    self-signed certificate the server can use for its HTTPS endpoint, so a lab or test server
    works without a certificate authority. In production, prefer a certificate from your CA.

    The certificate covers the computer name (and its FQDN when the computer is in a domain),
    localhost and the machine's IP addresses, because browsers check the name in the address
    bar against the certificate. Its subject is always "CN=SDS Server HTTPS", so the server
    settings stay the same on every machine.

    Steps:
      1. Work out the names and addresses the certificate must cover.
      2. Remove certificates this script created before in the same store (see -KeepExisting).
      3. Create the certificate in <StoreLocation>\My (LocalMachine\My unless told otherwise),
         valid for 2 years from the moment it is created, with a private key that cannot be
         exported. Run the script again to renew it before it expires.
      4. Export the public certificate (.cer) for the browsers that must trust it.
      5. Restart the SDS service when asked (-Restart), or say what to do next.

    Nothing has to be configured: at startup the server looks for this certificate and, when it
    finds it, adds an HTTPS endpoint on port 8352 (setting Sds:HttpsPort) limited to TLS 1.2 and
    1.3, and adds the port to its firewall rule. The Web Console on the same machine then calls
    the API over HTTPS by itself. Explicit Kestrel:Endpoints settings turn this off.

    Ships next to SdsServer.exe. Works with Windows PowerShell 5.1 and PowerShell 7.

.PARAMETER StoreLocation
    LocalMachine (default): the machine store, which the SDS service and an elevated server
    read; needs an elevated prompt. CurrentUser only when the server runs unelevated under
    your own account.

.PARAMETER DnsName
    Host names to cover. Defaults to the computer name, its FQDN when it is in a domain, and
    localhost.

.PARAMETER IPAddress
    IP addresses to cover. Defaults to every IPv4 address in use on this machine (link-local
    169.254.x.x left out), plus 127.0.0.1 and ::1. A browser that opens the server by an
    address the certificate does not list shows a warning, so run the script again after an
    address change.

.PARAMETER ExportPath
    Where to write the public certificate. Defaults to SdsServer.cer next to this script.

.PARAMETER KeepExisting
    Keep certificates this script created before. By default they are removed (with their
    private keys) so the server does not pick an old one. A server that is running keeps using
    the certificate it loaded at startup, so restart it after running this script (-Restart
    does that for the SDS service); until then its HTTPS connections fail.

.PARAMETER Restart
    Restart the SDS Windows service afterwards, when it is running, so HTTPS is on right away.
    Deployments in progress are interrupted.

.EXAMPLE
    .\New-SdsCertificate.ps1 -Restart
    Creates the certificate in LocalMachine\My (run as administrator) and restarts the SDS
    service, which then serves HTTPS on port 8352.

.EXAMPLE
    .\New-SdsCertificate.ps1 -StoreLocation CurrentUser -DnsName sds01,sds01.contoso.com,localhost
    Creates the certificate for a server that runs under your account, for the given names.
#>
[CmdletBinding(SupportsShouldProcess)]
param(
    [ValidateSet('LocalMachine', 'CurrentUser')]
    [string] $StoreLocation = 'LocalMachine',

    [string[]] $DnsName,

    [string[]] $IPAddress,

    # Default worked out below: Windows PowerShell 5.1 leaves $PSScriptRoot empty in param()
    # defaults when the script is started with powershell.exe -File.
    [string] $ExportPath,

    [switch] $KeepExisting,

    [switch] $Restart
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

# Load the modules used below up front, outside -WhatIf: modules loaded on first use inherit it,
# which skips creating their aliases and, in Windows PowerShell 5.1, the Cert: drive itself.
$whatIf = $WhatIfPreference
$WhatIfPreference = $false
# Microsoft.PowerShell.Security (the Cert: drive) is loaded the usual way, by using one of its
# commands: importing it explicitly breaks in Windows PowerShell 5.1 (duplicate type data).
$null = Get-ExecutionPolicy
if (-not (Get-PSDrive -Name Cert -ErrorAction SilentlyContinue)) {
    throw 'The Cert: drive is not available (module Microsoft.PowerShell.Security did not load).'
}
Import-Module CimCmdlets, NetTCPIP, PKI
$WhatIfPreference = $whatIf

$subject = 'CN=SDS Server HTTPS'
$friendlyName = 'SDS Server HTTPS'

# Fixed validity: 2 years from the moment of creation.
$validYears = 2

if (-not $ExportPath) {
    $scriptFolder = if ($PSScriptRoot) { $PSScriptRoot } else { Split-Path -Parent $MyInvocation.MyCommand.Path }
    $ExportPath = Join-Path $scriptFolder 'SdsServer.cer'
}
$storePath = "Cert:\$StoreLocation\My"

if ($StoreLocation -eq 'LocalMachine') {
    $principal = [Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()
    if (-not $principal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
        throw 'Creating a certificate in LocalMachine\My needs an elevated prompt. Run PowerShell as administrator, or use -StoreLocation CurrentUser when the server runs under your account.'
    }
}

# ---------------------------------------------------------------------
# Step 1: names and addresses
# ---------------------------------------------------------------------
if (-not $DnsName) {
    $DnsName = @($env:COMPUTERNAME)
    $computer = Get-CimInstance Win32_ComputerSystem
    if ($computer.PartOfDomain) {
        $DnsName += "$($env:COMPUTERNAME).$($computer.Domain)".ToLowerInvariant()
    }
    $DnsName += 'localhost'
}

if (-not $IPAddress) {
    $IPAddress = @(Get-NetIPAddress -AddressFamily IPv4 |
        Where-Object { $_.IPAddress -notlike '169.254.*' -and $_.IPAddress -ne '127.0.0.1' } |
        Select-Object -ExpandProperty IPAddress)
    $IPAddress += '127.0.0.1', '::1'
}

foreach ($address in $IPAddress) {
    $parsed = $null
    if (-not [Net.IPAddress]::TryParse($address, [ref] $parsed)) {
        throw "'$address' is not an IP address."
    }
}

$DnsName = @($DnsName | Select-Object -Unique)
$IPAddress = @($IPAddress | Select-Object -Unique)
$san = @($DnsName | ForEach-Object { "DNS=$_" }) + @($IPAddress | ForEach-Object { "IPAddress=$_" })

# ---------------------------------------------------------------------
# Step 2: previous certificates from this script
# ---------------------------------------------------------------------
$removedPrevious = $false
if (-not $KeepExisting) {
    $previous = @(Get-ChildItem $storePath | Where-Object { $_.FriendlyName -eq $friendlyName -and $_.Subject -eq $subject })
    foreach ($old in $previous) {
        if ($PSCmdlet.ShouldProcess("$storePath\$($old.Thumbprint) (expires $($old.NotAfter))", 'Remove previous SDS certificate and its private key')) {
            Remove-Item -Path "$storePath\$($old.Thumbprint)" -DeleteKey
            $removedPrevious = $true
        }
    }
}

# ---------------------------------------------------------------------
# Step 3: the certificate
# ---------------------------------------------------------------------
if (-not $PSCmdlet.ShouldProcess($storePath, "Create '$subject' for $($san -join ', ')")) {
    return
}

$createdAt = Get-Date
$certificate = New-SelfSignedCertificate `
    -Subject $subject `
    -FriendlyName $friendlyName `
    -TextExtension @("2.5.29.17={text}$($san -join '&')") `
    -CertStoreLocation $storePath `
    -Type SSLServerAuthentication `
    -Provider 'Microsoft Software Key Storage Provider' `
    -KeyAlgorithm RSA `
    -KeyLength 2048 `
    -HashAlgorithm SHA256 `
    -KeyUsage DigitalSignature, KeyEncipherment `
    -KeyExportPolicy NonExportable `
    -NotBefore $createdAt `
    -NotAfter $createdAt.AddYears($validYears)

# ---------------------------------------------------------------------
# Step 4: public certificate for the browsers
# ---------------------------------------------------------------------
$exportFolder = Split-Path -Parent $ExportPath
if ($exportFolder -and -not (Test-Path $exportFolder)) {
    New-Item -ItemType Directory -Path $exportFolder | Out-Null
}
Export-Certificate -Cert $certificate -FilePath $ExportPath -Type CERT | Out-Null

# ---------------------------------------------------------------------
# Step 5: what to do next
# ---------------------------------------------------------------------
Write-Host ''
Write-Host "Created $subject in $storePath" -ForegroundColor Green
Write-Host "  Thumbprint : $($certificate.Thumbprint)"
Write-Host "  Valid      : $($certificate.NotBefore) to $($certificate.NotAfter) ($validYears years)"
Write-Host "  Covers     : $($san -join ', ')"
Write-Host "  Public cert: $ExportPath"
Write-Host ''
$service = Get-Service -Name 'SDS' -ErrorAction SilentlyContinue
$restarted = $false
if ($Restart -and $service -and $service.Status -eq 'Running') {
    if ($PSCmdlet.ShouldProcess('Windows service SDS', 'Restart')) {
        Restart-Service -Name 'SDS'
        $restarted = $true
        Write-Host 'Restarted the SDS service. HTTPS is now on (port 8352 unless Sds:HttpsPort says otherwise).' -ForegroundColor Green
    }
}
else {
    Write-Host 'Restart the SDS server: it finds this certificate and turns on HTTPS by itself (port 8352'
    Write-Host 'unless Sds:HttpsPort says otherwise), opens the port in its firewall rule, and the Web'
    Write-Host 'Console on this machine (restart it too) answers https:// on its port and redirects http://'
    Write-Host 'there. Nothing has to be edited.'
    if ($service) {
        Write-Host 'The SDS service can be restarted with: Restart-Service SDS (or run this script with -Restart).'
    }
}

# A running server loads the certificate once, at startup. If it is still using the one just
# removed, its key is gone and every HTTPS connection fails until the server restarts.
if (-not $restarted -and $removedPrevious -and (Get-Process -Name 'SdsServer' -ErrorAction SilentlyContinue)) {
    Write-Host ''
    Write-Warning 'SdsServer is running and still uses the previous certificate, which was just removed together with its private key. HTTPS connections to it fail until it is restarted: restart it now.'
}
Write-Host ''
Write-Host 'Browsers do not trust a self-signed certificate. The Web Console handles it: on the sign-in'
Write-Host 'page, a browser that has not accepted the certificate is taken to the warning once and back.'
Write-Host "To avoid the warning altogether, import $(Split-Path -Leaf $ExportPath) on the client machines."
