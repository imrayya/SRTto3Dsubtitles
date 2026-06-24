# SRT to 3D Subtitles

Convert standard `.srt` subtitles into `.ass` format, offset so they display correctly in 3D video playback.

## The Problem

When watching 3D movies (Side-by-Side, Over-Under, Anaglyph), centered subtitles get split between the left and right eye views, making them disorienting to read. This tool converts standard `.srt` subtitles into a 3D-compatible `.ass` format that keeps each eye's subtitles in the correct position.

## Features

- **SBS (Side-by-Side)** — subtitles split left/right for each eye
- **OU (Over-Under)** — subtitles split top/bottom for each eye
- **Anaglyph (Red/Green)** — subtitles offset with red/green color channels
- **CLI** — full command-line interface with configurable options
- **GUI** — lightweight Windows interface for easier mode selection
- **Formatting support** — converts HTML tags (`<b>`, `<i>`, `<u>`, `<font color>`) to ASS format codes
- **Resolution scaling** — adjusts subtitle positioning based on target resolution

## Usage

### Command Line (CLI)

**Drag and drop:**

1. Drag and drop an `.srt` file onto `ConvertSRTto3DASS.exe`
   - Uses default settings (SBS mode, 1280x720)
2. The resulting `.ass` file will be saved in the same directory with the same name

**Command line:**

```
ConvertSRTto3DASS.exe input.srt [options]
```

Available options:

```
--mode sbs|ou|rg              3D mode (default: sbs)
--resx <number>               Output width
--resy <number>               Output height
--baseresx <number>           Scaling reference width (default: 1280)
--baseresy <number>           Scaling reference height (default: 720)
--fontsize <number>           Font size (default: 16)
--offsetx <number>            RG eye separation
--bottomoffset <number>       RG bottom offset
--sbssidemargin <number>      SBS side margin
--outopmargin <number>        OU top subtitle margin
--verticalmargin <number>     General vertical margin
--output <path>               Output file path
--help, -h, /?                Show help
```

Examples:

```
ConvertSRTto3DASS.exe movie.srt --mode sbs
ConvertSRTto3DASS.exe movie.srt --mode ou --resx 1920 --resy 1080
ConvertSRTto3DASS.exe movie.srt --mode rg --offsetx 6 --bottomoffset 24
```

### GUI

1. Launch `ConvertSRTto3DASSGUI.exe`
2. Select your `.srt` input file
3. Choose your 3D mode (SBS, OU, or Anaglyph)
4. Adjust settings as needed
5. Click **Convert**

> **Note:** The GUI is experimental. Feedback and bug reports are welcome!

## Known Issues

- Plex Transcoder has issues with the converted subtitles. VLC works correctly.
- Subtitle positional data from `.srt` files is stripped during conversion.

## Requirements

- **Windows** — the GUI requires Windows (.NET Framework 4.8)
- **CLI** — the core converter is a .NET Framework 4.8 console application

## License

MIT License — Copyright © imrayya 2021-2026

## Contributing

Contributions are welcome. Please open an issue or submit a pull request.

### Contributors

- [imrayya](https://github.com/imrayya) — project owner
- [jae0815](https://github.com/jae0815)
- [fabio-noga](https://github.com/fabio-noga)
- [kaiju466](https://github.com/kaiju466)

## Disclaimer

This is a personal hobby project. The codebase is functional but not production-grade — minimal comments, no tests, and some rough edges. Use at your own discretion.
