$FilePath = '.\video.mp4';
$URL = 'http://localhost:8080/api/funcDurableFunctionsClient?testFileUpload=666';

$fileBytes = [System.IO.File]::ReadAllBytes($FilePath);
$fileEnc = [System.Text.Encoding]::GetEncoding('UTF-8').GetString($fileBytes);
$boundary = [System.Guid]::NewGuid().ToString(); 
$LF = "`r`n";

$bodyLines = ( 
    "--$boundary",
    "Content-Disposition: form-data; name=`"file`"; filename=`"$((Get-Item $FilePath).Name)`"",
    "Content-Type: application/octet-stream$LF",
    $fileEnc,
    "--$boundary--$LF" 
) -join $LF

Write-Host "Calling DurableFunctionsHttpClient at $url"
$result = Invoke-RestMethod -Uri $URL -Method Post -ContentType "multipart/form-data; boundary=`"$boundary`"" -Body $bodyLines

$result

Write-Host "Sleeping for 15 seconds..."
Sleep 15

Write-Host "Returning response from Durable Function Status Query URI $($result.statusQueryGetUri)"
$result = Invoke-WebRequest -Uri $result.statusQueryGetUri -Method Get 
$result.content | ConvertFrom-Json

