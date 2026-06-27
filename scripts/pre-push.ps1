Write-Host "Running dotnet format validation..."

$solutionPath = "ECommerceApp\ECommerce.sln"

if (-not (Test-Path $solutionPath)) {
    Write-Host "Solution file not found: $solutionPath"
    exit 1
}

dotnet format $solutionPath --verify-no-changes

if ($LASTEXITCODE -ne 0) {
    Write-Host "Code formatting issues found."
    exit 1
}

Write-Host "Formatting validation passed."
exit 0
