using System;
using System.IO;

namespace Supports2EA
{
    public class Program
    {

        private static string[] helpStringArr =
        {
            "Supports2EA. Usage:",
            "./supports2ea <inputFilename> [<charactersFilename>]",
            "",
            "Available options:",
            "--help",
            "   Displays this message.",
            "See online instructions for more information."
        };

        private static readonly string helpString = System.Linq.Enumerable.Aggregate(helpStringArr,
            (string a, string b) => { return a + '\n' + b; });

        static int Main(string[] args)
        {

            if (args[0] == "-h" || args[0] == "--help")
            {
                Console.Out.WriteLine(helpString);
                return 0;
            }






            return 0;
        }

    }


}