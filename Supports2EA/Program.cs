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

            IEnumerable<string> output;

            //for each entry in dict, write output data

        }

    }

}
