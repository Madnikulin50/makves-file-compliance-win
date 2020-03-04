Import-Module "./bin/Release/compliance.dll" -Verbose



#Get-Compliance -Text "test","—Õ»À—", "137047624348"

Get-ChildItem -Path C:\work\test\test -Filter *.doc -Recurse -File | ForEach-Object {
    Write-Host $_.FullName

    Get-Compliance -File $_.FullName -kb "C:\work\kb\research\test.json"
}

Get-ChildItem -Path C:\work\test\test -Filter *.pdf -Recurse -File | ForEach-Object {
    Write-Host $_.FullName

    Get-Compliance -File $_.FullName -kb "C:\work\kb\research\test.json"
}


Get-ChildItem -Path C:\work\test\test -Filter *.pdf -Recurse -File | ForEach-Object {
    Write-Host $_.FullName

    Get-Document-Text -File $_.FullName 
}


Remove-Module -Name "compliance" -Verbose
 