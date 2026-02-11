$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $MyInvocation.MyCommand.Path

$debugExe = Join-Path $root 'PolicyPlus\bin\Debug\Policy Plus.exe'
$releaseExe = Join-Path $root 'PolicyPlus\bin\Release\Policy Plus.exe'

$exe = if (Test-Path $releaseExe) { $releaseExe } elseif (Test-Path $debugExe) { $debugExe } else { $null }

if (-not $exe) {
    Write-Host 'Executável não encontrado. Compile o projeto primeiro.' -ForegroundColor Red
    Write-Host ('Procurado em:' + "`n - $releaseExe`n - $debugExe")
    exit 1
}

Write-Host "Abrindo: $exe" -ForegroundColor Green
Start-Process -FilePath $exe -Verb RunAs
