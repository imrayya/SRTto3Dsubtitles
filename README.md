# SRT subtitles to 3D subtitles
## The problem
I wanted watch a movie with subtitle, like I always do, but the subtitles for the movie I have was a traditonal .srt while the movie I was watching was a 3D movie which was **H**alf **S**ide **B**y **S**ide (HSBS). The subtitles in question were centered which was very disorienting to read while watching the movie. The solution? Write my own program to convert it for me

## Convert
Currently just a command line program to convert a .srt subtitle file to a .ass subtitle file while making it compatable with the HSBS format. Either drag the SRT file on the .exe to convert the .srt file or run the command `SRTto3D.exe [insert your file name here]` and the program will convert it. Word of warning, I have yet to put in formating and the program will strip off any formatting of the srt. It will also likely fail with .srt file that contain a positional information. 

## Features
_Basically nothing right now_
- [x] Convert a .srt to an .ass/.ssa
  - [x] For HSBS media 
  - [ ] For **S**ide **B**y **S**ide (SBS) media 
  - [ ] For **O**ver **U**nder (OU) media
  - [ ] For Anaglyph 3D media (red and green media)
  - [ ] For traditionally media (pancake mode).
    - _I suggest to just use ffmpeg for this use case_
- [ ] Convert formatting of .srt subtitles to transfer to the converted version
  - [ ] Convert positional data of .srt subtitles
- [ ] Add changes which allows custom:
  - [ ] Font
  - [ ] Color
  - [ ] Position
  - [ ] Margins
  - [ ] Encoding
- [ ] Add a proper GUI
- [ ] Any form of error handling or unit testing


## Known Issues
For some reason that I have yet to understand, Plex Transcorder doesn't not like the converted subtitles (it has a a werid issue) but it works perfectly in VLC

## Warning
The code is a complete and utter mess with very little comments in it. This was meant to be a private project and I haven't had the chance to clean it up yet
