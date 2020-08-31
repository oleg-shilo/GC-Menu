$packageName = 'gc-menu'
$url = 'https://github.com/oleg-shilo/GC-Menu/releases/download/v1.0.0/gc-menu.7z'

try {
  [System.Environment]::SetEnvironmentVariable('UNDER_CHOCO', 'yes')
  Stop-Process -Name "gc-menu"

  $installDir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"

  $cheksum = '0251EDA03DC329D73AF01BCE50FBF7AAEC0FDF511F5ADD86967C8E5911979E5F'
  $checksumType = "sha256"

  # Download and unpack a zip file
  Install-ChocolateyZipPackage "$packageName" "$url" "$installDir" -checksum $checksum -checksumType $checksumType

} catch {
  throw $_.Exception
}
