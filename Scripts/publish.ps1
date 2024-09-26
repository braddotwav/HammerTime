Param(
    # Output directory
    [Parameter(HelpMessage = "The location to store the zipped builds")]
    [string]
    $output
)
# -------------------------------------------
# Set Up
# -------------------------------------------
Write-Host "Starting setup process..."
Start-Sleep -Seconds 1

Write-Host "Checking output directory..."
if (!($output) -or !(Test-Path -Path $output))
{
    $current = Get-Location
    $output = Join-Path -Path $current -ChildPath "dist"
    Write-Host "No output path was explicitly set. \n The output will now default to: $output" -ForegroundColor Yellow
}

Set-Location -Path './HammerTime'

Write-Host "Cleaning bin folder..."
Remove-Item -Path "./bin" -Recurse -Force

Write-Host "Finished setup process!" -ForegroundColor Green
Start-Sleep -Seconds 1

# -------------------------------------------
# Build Application
# -------------------------------------------
Write-Host "Starting self contained process..."
Start-Sleep -Seconds 1

# Build self-contained
dotnet publish -c Release -r win-x64 --self-contained --output "./bin/self-contained"

# Check if there was an error, if there was then exit
if ($LASTEXITCODE -ne 0)
{
    Write-Host "An error occurred while building self contained application" -ForegroundColor Red
    exit 1
}

Write-Host "Finished self contained process!" -ForegroundColor Green
Start-Sleep -Seconds 1

Write-Host "Starting framework dependant process..."
Start-Sleep -Seconds 1

# Build framework dependant
dotnet publish -c Release -r win-x64 --no-self-contained --output "./bin/framework-dependant"

# Check if there was an error, if there was then exit
if ($LASTEXITCODE -ne 0)
{
    Write-Host "An error occurred while building self contained application" -ForegroundColor Red
    exit 1    
}

Write-Host "Finished framework dependant process!" -ForegroundColor Green
Start-Sleep -Seconds 1

# -------------------------------------------
# Zipping
# -------------------------------------------
Write-Host "Starting zip process for self contained..."
Start-Sleep -Seconds 1

# Zip self-contained
$selfcontained = Join-Path -Path $output -ChildPath "hammertime-winx64-self-contained.zip"
7z a -bsp2 -r $selfcontained "./bin/self-contained/*"

# Check if there was an error, if there was then exit
if ($LASTEXITCODE -ne 0)
{
    Write-Host "An error occurred while zipping self contained" -ForegroundColor Red
    exit 1
}

Write-Host "Finished zipping self contained!" -ForegroundColor Green
Start-Sleep -Seconds 1

Write-Host "Starting zip process for framework dependant..."
Start-Sleep -Seconds 1

# Zip framework dependant
$frameworkdependant = Join-Path -Path $output -ChildPath "hammertime-winx64-framework-dependant.zip"
7z a -bsp2 -r $frameworkdependant "./bin/framework-dependant/*"

# Check if there was an error, if there was then exit
if ($LASTEXITCODE -ne 0)
{
    Write-Host "An error occurred while zipping framework dependant" -ForegroundColor Red
    exit 1
}

Write-Host "Finished zipping framework dependant!" -ForegroundColor Green
Start-Sleep -Seconds 2

Write-Host "Publish process complete!" -ForegroundColor Green