#!/bin/bash
# Publish all targets for SRTto3Dsubtitles
# Usage: ./publish.sh

set -e

declare -a targets=(
    "ConvertSRTto3DASS.Cli:linux-x64:ConvertSRTto3DASS-linux-x64"
    "ConvertSRTto3DASS.Cli:win-x64:ConvertSRTto3DASS-win-x64.exe"
    "ConvertSRTto3DASS.Cli:osx-x64:ConvertSRTto3DASS-osx-x64"
    "ConvertSRTto3DASS.Cli:osx-arm64:ConvertSRTto3DASS-osx-arm64"
    "ConvertSRTto3DASS.Gui:win-x64:ConvertSRTto3DASS-GUI-Win-x64.exe"
)

rm -rf publish
mkdir -p publish

for entry in "${targets[@]}"; do
    IFS=':' read -r project runtime name <<< "$entry"

    echo "Publishing $name..."
    dotnet publish "$project" -c Release -r "$runtime" \
        --self-contained false \
        -p:PublishSingleFile=true \
        -p:IncludeNativeLibrariesForSelfExtract=true \
        -p:OutputType=Exe \
        -p:OutputPath="publish/$runtime/"

    src="publish/$runtime/$project.exe"
    dst="publish/$name"
    if [ -f "$src" ]; then
        mv "$src" "$dst"
        echo "  -> $dst"
    fi
done

echo ""
echo "Done! Published files in publish/"
