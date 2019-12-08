Import-Module "./bin/Release/compliance.dll" -Verbose



Get-Compliance -Text "test","—Õ»À—", "137047624348"

Get-ChildItem -Path c:\work -Filter *.doc -Recurse -File | ForEach-Object {
    Write-Host $_.FullName

    Get-Compliance -File $_.FullName
}

Remove-Module -Name "compliance" -Verbose
 