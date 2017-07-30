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

            Abbreviator ae = new Abbreviator();
            ae.Add("Ontario", "ON");
            Console.WriteLine(ae.Abbrev("Ontario"));

            CityProvinceCountryCorrector cc = new CityProvinceCountryCorrector();
            if (cc.LoadCityTable())
            {
                string[] record = { "Toronto", "Nova Scotia", "China" };
                string[] result = cc.CorrectCityAddress(record);
                Console.WriteLine(record[0] + ", " + record[1] + ", " + record[2] + " ==> " +
                    result[0] + ", " + result[1] + ", " + result[2]);
            }
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

    class Abbreviator
    {
        private Dictionary<string, string> map;
        public Abbreviator()
        {
            map = new Dictionary<string, string>();
        }

        public void Add(string expand, string abbrev)
        {
            map.Add(expand, abbrev);
        }

        public string Abbrev(string word)
        {
            string abbreviation;
            // if found the key, return the abbreviation, if not, return the itself
            if (map.TryGetValue(word, out abbreviation))
            {
                return abbreviation;
            }
            else
            {
                return word;
            }
        }
    }

    class CityProvinceCountryCorrector
    {
        private List<string[]> cityTable;

        public Boolean LoadCityTable()
        {
            cityTable = new List<string[]>();

            string[] city = new string[3];
            city[0] = "Toronto";
            city[1] = "ON";
            city[2] = "Canada";

            cityTable.Add(city);
            return true;
        }

        // return 
        public string[] CorrectCityAddress(string[] cityProvinceCountry)
        {
            for (int i = 0; i < cityTable.Count; ++i)
            {
                if (String.Compare(cityTable[i][0], cityProvinceCountry[0]) == 0)
                {
                    return cityTable[i];
                }
            }
            // not found, return the original
            return cityProvinceCountry;
        }
    }
}
