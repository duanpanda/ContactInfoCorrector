using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(CapitalizeString("samuel"));
            Console.WriteLine(CapitalizeString("julia"));
            Console.WriteLine(CapitalizeString("john smith"));
            string[] words = "john smith".Split(' ');
            foreach (string s in words)
            {
                Console.Write(CapitalizeString(s) + ' ');
            }
            Console.Write('\n');
            string inString = "test test2 test test2 test test2";
            Console.WriteLine(CapitalizeWord(inString));
        }

        static string CapitalizeString(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        static string CapitalizeWord(string s)
        {
            TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;
            string output = cultInfo.ToTitleCase(s);
            return output;
        }
    }
}
