using System;
using System.IO;
using System.Collections.Generic;

namespace Supports2EA
{
    public class Program
    {
        private Dictionary<string, IList<Character>> characters;
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



            //enumerate characters into character objects
            enumerateCharacters(characters, cfile);

            //parse input script
            parseScript(characters, ifile);

            //write output file
            outputFile(characters, ofile);

            Console.Out.WriteLine("Finished successfully.");

            return 0;
        }

        private void enumerateCharacters(Dictionary<string, IList<Character>> dict, string filepath)
        {
            //parse the character input file
            //for each line see if it starts with #define (ignoring whitespace)
            //if so, next 2 arguments are dict identifier and ID for associated character



        }

        private void parseScript(Dictionary<string, IList<Character>> dict, string filepath)
        {

        }

        private void outputFile(Dictionary<string, IList<Character>> dict, string filepath)
        {

        }

    }

}
