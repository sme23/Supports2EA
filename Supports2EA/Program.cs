using System;
using System.IO;
using System.Collections.Generic;

namespace Supports2EA
{
    public class Program
    {
        private Dictionary<string, Character> characters;
        private static string[] helpStringArr =
        {
            "Supports2EA. Usage:",
            "./supports2ea <inputFilename> <charactersFilename>",
            "",
            "Available options:",
            "--help",
            "   Displays this message.",
            "See online instructions for more information."
        };

        private static readonly string helpString = System.Linq.Enumerable.Aggregate(helpStringArr,
            (string a, string b) => { return a + '\n' + b; });

        public int Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.Out.WriteLine("ERROR: Too few arguments. Use '--help' for more information.");
                return 1;
            }
            if (args.Length > 2)
            {
                Console.Out.WriteLine("ERROR: Too many arguments. Use '--help' for more information.");
            }

            if (args[0] == "-h" || args[0] == "--help")
            {
                Console.Out.WriteLine(helpString);
                return 0;
            }

            string ifile = args[0];
            string cfile = args[1];
            string ofile = ifile;

            //strip down ifile into ofile directory, then append ofile filename
            if (ofile.Contains('/')) { ofile = ofile.Substring(0, ofile.LastIndexOf('/') + 1); }
            else if (ofile.Contains('\\')) { ofile = ofile.Substring(0, ofile.LastIndexOf('\\') + 1); }
            else { ofile = ""; }

            ofile = String.Concat(ofile, "SupportsInstaller.event");

            //enumerate characters into character objects
            enumerateCharacters(characters, cfile);

            //parse input script
            parseScript(characters, ifile);

            //write output file
            outputFile(characters, ofile);

            Console.Out.WriteLine("Finished successfully.");

            return 0;
        }

        private void enumerateCharacters(Dictionary<string, Character> dict, string filepath)
        {

            //parse the character input file
            //for each line see if it starts with #define (ignoring whitespace)
            //if so, next 2 arguments are dict identifier and ID for associated character

            foreach (string line in System.IO.File.ReadLines(filepath))
            {
                string trimmedLine = line.Trim();
                if (trimmedLine.StartsWith("#define"))
                {
                    trimmedLine = line.Substring(7);
                    trimmedLine = trimmedLine.TrimStart();
                    string key = trimmedLine.Substring(0, trimmedLine.IndexOf(' '));
                    trimmedLine = trimmedLine.TrimStart();
                    byte index = Convert.ToByte(Convert.ToInt32(trimmedLine));
                    Character newChar = new Character();
                    newChar.init(key, index);
                    dict.Add(key, newChar);
                }
            }

        }

        private void parseScript(Dictionary<string, Character> dict, string filepath)
        {
            foreach (string line in System.IO.File.ReadLines(filepath))
            {
                string trimmedLine = line.Trim();
                trimmedLine = line.Replace(" ", "");

                string char1 = trimmedLine.Substring(0, trimmedLine.IndexOf('+') - 1);
                trimmedLine = trimmedLine.Substring(trimmedLine.IndexOf('+') + 1);
                string char2 = trimmedLine.Substring(0, trimmedLine.IndexOf('{') - 1);
                trimmedLine = trimmedLine.Substring(trimmedLine.IndexOf('{') + 1);
                byte startValue = Convert.ToByte(trimmedLine.Substring(0, trimmedLine.IndexOf(',') - 1));
                trimmedLine = trimmedLine.Substring(trimmedLine.IndexOf(',') + 1);
                byte growthValue = Convert.ToByte(trimmedLine.Substring(0, trimmedLine.IndexOf('}') - 1));

                Character partner1;
                Character partner2;
                dict.TryGetValue(char1, out partner1);
                dict.TryGetValue(char2, out partner2);

                //insert values at next free index for each
                partner1.supportPartners[partner1.numPartners] = char2;
                partner1.initialValues[partner1.numPartners] = startValue;
                partner1.growthRates[partner1.numPartners] = growthValue;
                partner1.numPartners++;

                partner2.supportPartners[partner2.numPartners] = char1;
                partner2.initialValues[partner2.numPartners] = startValue;
                partner2.growthRates[partner2.numPartners] = growthValue;
                partner2.numPartners++;

            }
        }

        private void outputFile(Dictionary<string, Character> dict, string filepath)
        {

            string[] header = {
                "//File generated by Supports2EA.",
                "",
                "#define SupportData(p1,p2,p3,p4,p5,p6,p7,i1,i2,i3,i4,i5,i6,i7,g1,g2,g3,g4,g5,g6,g7,n) \"BYTE p1 p2 p3 p4 p5 p6 p7 i1 i2 i3 i4 i5 i6 i7 g1 g2 g3 g4 g5 g6 g7 n 0 0\"",
                "#define SupportText(p1,p2,c,b,a) \"SHORT p1 p2 c b a 0 0 0\"",
                ""
            };

            string[] output1 = new string[] { };
            int i = 0;

            //for each entry in dict, write output data
            foreach (KeyValuePair<string, Character> unit in dict)
            {
                output1[i] = (unit.Value.name + "SupportData:");
                output1[i + 1] = ("SupportData(" +
                                    unit.Value.supportPartners[0].ToString() + "," +
                                    unit.Value.supportPartners[1].ToString() + "," +
                                    unit.Value.supportPartners[2].ToString() + "," +
                                    unit.Value.supportPartners[3].ToString() + "," +
                                    unit.Value.supportPartners[4].ToString() + "," +
                                    unit.Value.supportPartners[5].ToString() + "," +
                                    unit.Value.supportPartners[6].ToString() + "," +
                                    unit.Value.initialValues[0].ToString() + "," +
                                    unit.Value.initialValues[1].ToString() + "," +
                                    unit.Value.initialValues[2].ToString() + "," +
                                    unit.Value.initialValues[3].ToString() + "," +
                                    unit.Value.initialValues[4].ToString() + "," +
                                    unit.Value.initialValues[5].ToString() + "," +
                                    unit.Value.initialValues[6].ToString() + "," +
                                    unit.Value.growthRates[0].ToString() + "," +
                                    unit.Value.growthRates[1].ToString() + "," +
                                    unit.Value.growthRates[2].ToString() + "," +
                                    unit.Value.growthRates[3].ToString() + "," +
                                    unit.Value.growthRates[4].ToString() + "," +
                                    unit.Value.growthRates[5].ToString() + "," +
                                    unit.Value.growthRates[6].ToString() + "," +
                                    unit.Value.numPartners.ToString() + ")"
                );
                i = i + 2;
            }

            string[] secondHeader =
            {
                "",
                "PUSH",
                "   ORG $84784",
                "   POIN SupportTextList",
                "   ",
                "   ORG $847FC",
                "   POIN SupportTextList",
                "POP",
                "",
                "SupportTextList:",
                ""
            };

            string[] output2 = new string[] { }; 
            int j = 0;
            List<CharacterPairs> pairs = new List<CharacterPairs>();

            foreach (KeyValuePair<string, Character> unit in dict)
            {
                for (int k = 0; k < 7; k++) {
                    if (!((pairs.Contains(new CharacterPairs(unit.Value.name, unit.Value.supportPartners[k]))) || (pairs.Contains(new CharacterPairs(unit.Value.supportPartners[k], unit.Value.name)))))
                    {
                        output2[j] = ("SupportText(" +
                                unit.Value.name + "," +
                                unit.Value.supportPartners[k] + "," +
                                unit.Value.name + unit.Value.supportPartners[k] + "CSupport," +
                                unit.Value.name + unit.Value.supportPartners[k] + "BSupport," +
                                unit.Value.name + unit.Value.supportPartners[k] + "ASupport)");
                        pairs.Add(new CharacterPairs(unit.Value.name, unit.Value.supportPartners[k]));
                        j++;
                    }
                }
            }

            string footer = "SHORT 0xFFFF 0 0 0 0 0 0 0";

            if (File.Exists(filepath)) File.Delete(filepath);
            File.OpenWrite(filepath);


            string[] outputString = new string[5];
            outputString[0] = System.Linq.Enumerable.Aggregate(header, (string a, string b) => { return a + '\n' + b; });
            outputString[1] = System.Linq.Enumerable.Aggregate(output1, (string a, string b) => { return a + '\n' + b; });
            outputString[2] = System.Linq.Enumerable.Aggregate(secondHeader, (string a, string b) => { return a + '\n' + b; });
            outputString[3] = System.Linq.Enumerable.Aggregate(output2, (string a, string b) => { return a + '\n' + b; });
            outputString[4] = footer;

            File.WriteAllLines(filepath, outputString);

        }
    }
}
