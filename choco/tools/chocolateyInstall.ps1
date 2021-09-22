$packageName = 'GC-Menu'
$url = 'https://github.com/oleg-shilo/GC-Menu/releases/download/v1.0.1.0/gc-menu.7z'

Stop-Process -Name "gc-menu"

$installDir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"

$cheksum = '38A0EA67C1644076FB14810F098651E1572C1DE14BBD59F66FC9C8FFC4D19B0E'
$checksumType = "sha256"

# Download and unpack a zip file
Install-ChocolateyZipPackage "$packageName" "$url" "$installDir" -checksum $checksum -checksumType $checksumType
