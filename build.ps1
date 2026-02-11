[CmdletBinding()]
param(
    [string]$Solution = "$PSScriptRoot\PolicyPlus.sln",
    [string]$Configuration = "Release",
    [switch]$Run
)

function Find-MSBuild {
    $ms = Get-Command msbuild -ErrorAction SilentlyContinue
    if ($ms) { return $ms.Path }

    $possible = @(
        "$env:ProgramFiles(x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe",
        "$env:ProgramFiles(x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe",
        "$env:ProgramFiles(x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe",
        "$env:ProgramFiles(x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\MSBuild.exe"
    )
    foreach ($p in $possible) { if (Test-Path $p) { return $p } }

    $vswhere = "$env:ProgramFiles(x86)\Microsoft Visual Studio\Installer\vswhere.exe"
    if (Test-Path $vswhere) {
        $inst = & $vswhere -latest -products * -requires Microsoft.Component.MSBuild -property installationPath 2>$null
        if ($inst) {
            $candidates = @(
                Join-Path $inst "MSBuild\Current\Bin\MSBuild.exe",
                Join-Path $inst "MSBuild\15.0\Bin\MSBuild.exe"
            )
            foreach ($c in $candidates) { if (Test-Path $c) { return $c } }
        }
    }

    return $null
}

Push-Location $PSScriptRoot
try {
    Write-Host "Executando version.bat (se existir)..."
    $vb = Join-Path $PSScriptRoot "version.bat"
    if (Test-Path $vb) { & $vb } else { Write-Host "version.bat não encontrado — pulando." }

    $msbuild = Find-MSBuild
    if (-not $msbuild) {
        Write-Error "MSBuild não encontrado. Instale "Build Tools for Visual Studio" ou abra o Developer Command Prompt do VS e tente novamente."
        exit 1
    }

    Write-Host "Usando MSBuild em: $msbuild"
    & $msbuild $Solution /t:Build /p:Configuration=$Configuration /m /verbosity:minimal
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Build falhou (exit code $LASTEXITCODE)."
        exit $LASTEXITCODE
    }

    $exe = Get-ChildItem -Path (Join-Path $PSScriptRoot "PolicyPlus") -Recurse -Filter "PolicyPlus.exe" -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($exe) {
        Write-Host "Build concluído: $($exe.FullName)"
        if ($Run) {
            Write-Host "Executando com elevação..."
            Start-Process -FilePath $exe.FullName -Verb RunAs
        }
    } else {
        Write-Warning "Não foi possível localizar PolicyPlus.exe em pastas 'bin'. Verifique saída do build."
    }
} finally {
    Pop-Location
}
