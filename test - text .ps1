Import-Module "./bin/Release/compliance.dll" -Verbose




Get-ChildItem -Path C:\work\test\test -Filter *.pdf -Recurse -File | ForEach-Object {
    Write-Host $_.FullName

    Get-DocumentText -File $_.FullName 
}


Remove-Module -Name "compliance" -Verbose
 