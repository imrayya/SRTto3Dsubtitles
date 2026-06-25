# Publish all targets for SRTto3Dsubtitles
# Usage: ./publish.ps1

$ErrorActionPreference = "Stop"

$targets = @(
    @{ Project = "ConvertSRTto3DASS.Cli";     Runtime = "linux-x64";    Name = "ConvertSRTto3DASS-linux-x64" },
    @{ Project = "ConvertSRTto3DASS.Cli";     Runtime = "win-x64";      Name = "ConvertSRTto3DASS-win-x64.exe" },
    @{ Project = "ConvertSRTto3DASS.Cli";     Runtime = "osx-x64";      Name = "ConvertSRTto3DASS-osx-x64" },
    @{ Project = "ConvertSRTto3DASS.Cli";     Runtime = "osx-arm64";    Name = "ConvertSRTto3DASS-osx-arm64" },
    @{ Project = "ConvertSRTto3DASS.Gui";     Runtime = "win-x64";      Name = "ConvertSRTto3DASS-GUI-Win-x64.exe" }
)

foreach ($t in $targets) {
    Write-Host "Publishing $($t.Name)..." -ForegroundColor Cyan
    dotnet publish $t.Project -c Release -r $t.Runtime `
        --self-contained false `
        -p:PublishSingleFile=true `
        -p:IncludeNativeLibrariesForSelfExtract=true `
        -p:OutputType=Exe `
        -p:OutputPath="publish\$($t.Runtime)\"

    $src = "publish\$($t.Runtime)\$($t.Project).exe"
    $dst = "publish\$($t.Name)"
    if (Test-Path $src) {
        Move-Item $src $dst -Force
        Write-Host "  -> $dst" -ForegroundColor Green
    }
}

Write-Host "`nDone! Published files in publish/`n" -ForegroundColor Green
