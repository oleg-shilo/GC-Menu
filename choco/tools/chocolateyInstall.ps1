$packageName = 'GC-Menu'
$url = 'https://github.com/oleg-shilo/GC-Menu/releases/download/v1.0.3/gc-menu.7z'

Stop-Process -Name "gc-menu"

$installDir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"

$checksum = '44DA8DB0BE2F1ECBA0F41E9E8FA9C68A474B6686CB799BCB2111778338F53DD4'
$checksumType = "sha256"

# Download and unpack a zip file
Install-ChocolateyZipPackage "$packageName" "$url" "$installDir" -checksum $checksum -checksumType $checksumType
