$packageName = 'GC-Menu'
$url = 'https://github.com/oleg-shilo/GC-Menu/releases/download/v1.0.1/gc-menu.7z'

Stop-Process -Name "gc-menu"

$installDir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"

$cheksum = 'C0EA061AA2C749B1C7897CE447BBE0BFB8BB70F180F85146C17B780705696AD3'
$checksumType = "sha256"

# Download and unpack a zip file
Install-ChocolateyZipPackage "$packageName" "$url" "$installDir" -checksum $checksum -checksumType $checksumType
