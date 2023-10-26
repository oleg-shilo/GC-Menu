$packageName = 'GC-Menu'
$url = 'https://github.com/oleg-shilo/GC-Menu/releases/download/v1.0.2/gc-menu.7z'

Stop-Process -Name "gc-menu"

$installDir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"

$cheksum = '9E686C66E9F4B40CC34315A2DE091F520C55BF7F5822461C11C7B9B35E459F08'
$checksumType = "sha256"

# Download and unpack a zip file
Install-ChocolateyZipPackage "$packageName" "$url" "$installDir" -checksum $checksum -checksumType $checksumType
