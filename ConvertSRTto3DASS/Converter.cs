using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

//SSA/ASS specification v4+ http://www.tcax.org/docs/ass-specs.htm
//SRT (SubRip) specification https://en.wikipedia.org/wiki/SubRip

namespace ConvertSRTto3DASS
{
    class Converter
    {

        static void Main(string[] args)
        {

            var extracted = ExtractSubFromSRT(args[0]);
            var style = CreateStandardStyle();
            var header = CreateHeader(args[0]);
            var events = ProcessSubs(extracted);

            var finished = "[Script Info]\n" + header + "\n\n[V4+ Styles]\n" + style + "\n\n[Events]\n" + events;
            File.WriteAllText(Path.GetFileNameWithoutExtension(args[0]) + ".ass", finished);
        }


        //TODO: Add formatting for the string. Have to look into the SSA v4.00+ specification on how to handle it
        private static string ChangeFormatting(string line)
        {
            return "";
        }


        //TODO: remove this and use the above method instead where it actually uses the formatting of the file
        private static Regex reg = new Regex("<.+?>|(\\r)"); //Used to remove html tags used and line breaks
        private static string removeFormatting(string line)
        {
            var replacement = reg.Replace(line, "");
            return replacement;

        }


        private static string ProcessSubs(List<Tuple<string, string, string, string>> srt)
        {
            string endResult = "Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text\n";
            foreach (var events in srt)
            {
                var start = ConvertTimeStamp(events.Item1);
                var end = ConvertTimeStamp(events.Item2);

                //First layer, for the right eye
                var line = "Dialogue: " + 0 + "," + start + "," + end + ",Right,,0,0,0,," + events.Item3 + "\n";
                //Second layer, for the left eye
                line += "Dialogue: " + 1 + "," + start + "," + end + ",Left,,0,0,0,," + events.Item3;

                endResult += line + "\n";
            }
            return endResult;

        }

        
        private static string ConvertTimeStamp(string timeStamp)
        {
            //Change double digit hour (for .srt) to single digit hour marker (for .ass) <- very crude
            var tmp = timeStamp.Substring(1);

            //Change thousands of a second hunderdth second
            tmp = tmp.Substring(0, tmp.Length - 1);//TODO: Do actual rounding
            return tmp.Replace(",", ".");
        }

        //TODO: Make this human readable
        //TODO: Add adjustable parameters
        private static string CreateStandardStyle()
        {
            string style = "Format: " +
                "Name, " +
                "Fontname, " +
                "Fontsize, " +
                "PrimaryColour, " +
                "SecondaryColour, " +
                "OutlineColour, " +
                "BackColour, " +
                "Bold, " +
                "Italic, " +
                "Underline, " +
                "StrikeOut, " +
                "ScaleX, " +
                "ScaleY, " +
                "Spacing, " +
                "Angle, " +
                "BorderStyle, " +
                "Outline, " +
                "Shadow, " +
                "Alignment, " +
                "MarginL, " +
                "MarginR, " +
                "MarginV, " +
                "Encoding\n" +

                "Style: " +
                "Right," +
                "Arial," +
                "16," +
                "&Hffffff," +
                "&Hffffff," +
                "&H0," +
                "&H0," +
                "0," +
                "0," +
                "0," +
                "0," +
                "100," +
                "100," +
                "0," +
                "0," +
                "1," +
                "1," +
                "0," +
                "2," +
                "192," +
                "0," +
                "10," +
                "0\n" +


                "Style: " +
                "Left," +
                "Arial," +
                "16," +
                "&Hffffff," +
                "&Hffffff," +
                "&H0," +
                "&H0," +
                "0," +
                "0," +
                "0," +
                "0," +
                "100," +
                "100," +
                "0," +
                "0," +
                "1," +
                "1," +
                "0," +
                "2," +
                "0," +
                "192 ," +
                "10," +
                "0";
            return style;
        }

        //TODO: create a system where you can actually give paramaters to it
        private static string CreateHeader(string file, int resY = 288, int resX = 384)
        {
            var name = Path.GetFileNameWithoutExtension(file);
            string scriptinfo = "; Generated by ConvertSRTto3D\n" +
                "Title: " + name + "\n" +
                "ScriptType: v4.00+\n" +
                "Collisions: Normal\n" +
                "PlayResX: " + resX + "\n" +
                "PlayResY: " + resY + "\n" +
                "ScaledBorderAndShadow: yes";
            return scriptinfo;
        }

        //start, end, text, format <- tuple format
        //Note - I want to change the tuple as format bit is useless as the data is carried in the text field for both .srt and .ass
        //but I could reprepose it for any positional data
        private static List<Tuple<string, string, string, string>> ExtractSubFromSRT(string file)
        {

            var converted = new List<Tuple<string, string, string, string>>();

            var timestamp_start = "";
            var timestamp_end = "";
            var subtitiles = "";
            string srt = File.ReadAllText(file);
            
            //Which dialog we are on (sanity check)
            int i = 1;

            //Whether to extract the time stamp or not
            int j = 0; //TODO: Change to a bool 

            int linecounter = 0; //Where we are in the .srt, meant for debugging

            foreach (string line in srt.Split('\n'))
            {
                linecounter++;
                //Empty line assumes that the next line with event number thus previous dialog is finished and can be saved
                if (line == "" | line == "\r")
                {
                    j = 0;
                    converted.Add(new Tuple<string, string, string, string>(timestamp_start, timestamp_end, removeFormatting(subtitiles), ""));
                    subtitiles = "";
                    continue;
                }
                //Try to parse the event/dialog number
                if (int.TryParse(line, out int k))
                {
                    //Event number doesn't match counted event number. Mismatch means something probably went wrong
                    if (k != i)
                    {
                        Console.Error.WriteLine("Something went wrong");
                        System.Environment.Exit(-1);
                    }
                    else
                    {
                        i++;
                        j++;
                        continue;
                    }
                }

                //If it a timestamp is expected, extract it
                if (j == 1)
                {
                    timestamp_start = line.Substring(0, 12);
                    timestamp_end = line.Substring(17, 12);
                    j++;
                }
                else
                {
                    if (subtitiles == "")
                    {
                        subtitiles = line;
                        subtitiles.Replace("\\.r", "");
                    }
                    else
                    {
                        subtitiles = subtitiles + "\\n" + line;
                    }
                }
            }
            return converted;
        }
    }
}
