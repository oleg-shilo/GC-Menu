$packageName = 'GC-Menu'
$url = 'https://github.com/oleg-shilo/GC-Menu/releases/download/v1.1.0.0/gc-menu.7z'

Stop-Process -Name "gc-menu"

$installDir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"

$checksum = '6AF5F20F61970F5D5A5C19868A0CF17BBC01781735527940FC40C89FE1460227'
$checksumType = "sha256"

# Download and unpack a zip file
Install-ChocolateyZipPackage "$packageName" "$url" "$installDir" -checksum $checksum -checksumType $checksumType
