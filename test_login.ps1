Write-Host "=== TEST 1: Wrong password ===" -ForegroundColor Yellow
try {
    $resp = Invoke-RestMethod -Uri http://localhost:5098/api/Auth/login -Method Post -ContentType "application/json" -Body '{"email":"yashsen89@gmail.com","password":"wrong123"}'
    Write-Host "Status: 200 OK (BAD - should have been 401)" -ForegroundColor Red
    $resp | ConvertTo-Json
} catch {
    $statusCode = $_.Exception.Response.StatusCode.value__
    Write-Host "Status: $statusCode" -ForegroundColor Green
    $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
    $responseBody = $reader.ReadToEnd()
    $reader.Close()
    Write-Host $responseBody
}

Write-Host "`n=== TEST 2: Wrong password for non-existent user ===" -ForegroundColor Yellow
try {
    $resp = Invoke-RestMethod -Uri http://localhost:5098/api/Auth/login -Method Post -ContentType "application/json" -Body '{"email":"nonexistent@test.com","password":"anything"}'
    Write-Host "Status: 200 OK (BAD)" -ForegroundColor Red
} catch {
    $statusCode = $_.Exception.Response.StatusCode.value__
    Write-Host "Status: $statusCode" -ForegroundColor Green
    $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
    $responseBody = $reader.ReadToEnd()
    $reader.Close()
    Write-Host $responseBody
}

Write-Host "`n=== TEST 3: Correct password for registered user ===" -ForegroundColor Yellow
try {
    $resp = Invoke-RestMethod -Uri http://localhost:5098/api/Auth/login -Method Post -ContentType "application/json" -Body '{"email":"yashsen89@gmail.com","password":"adsfghj"}'
    Write-Host "Status: 200 OK" -ForegroundColor Green
    Write-Host "Token received: $($resp.token.Length -gt 0)" -ForegroundColor $(if ($resp.token.Length -gt 0) { "Green" } else { "Red" })
    Write-Host "Message: $($resp.message)"
} catch {
    $statusCode = $_.Exception.Response.StatusCode.value__
    Write-Host "Status: $statusCode" -ForegroundColor Red
    $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
    $responseBody = $reader.ReadToEnd()
    $reader.Close()
    Write-Host $responseBody
}
