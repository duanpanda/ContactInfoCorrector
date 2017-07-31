using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    abstract class CorrectorRule
    {
        public abstract void ApplyOn(ContactRecord contact);

        public static string CapitalizeString(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1).ToLower();
        }

        public static string CapitalizeWord(string s)
        {
            string[] words = s.Split(' ');
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string word in words)
            {
                stringBuilder.Append(CapitalizeString(word) + ' ');
            }
            return stringBuilder.ToString().TrimEnd(); ;
        }
    }

    class CapitalizeName : CorrectorRule
    {
        public override void ApplyOn(ContactRecord contact)
        {
            contact.FirstName = CapitalizeString(contact.FirstName);
            contact.LastName = CapitalizeString(contact.LastName);
        }
    }

    class CapitalizeStreet : CorrectorRule
    {
        public override void ApplyOn(ContactRecord contact)
        {
            contact.Street = CapitalizeWord(contact.Street);
        }
    }

    class CapitalizeCity : CorrectorRule
    {
        public override void ApplyOn(ContactRecord contact)
        {
            contact.City = CapitalizeWord(contact.City);
        }
    }

    class CityProvinceCountryCorrector : CorrectorRule
    {
        private List<string[]> cityTable;

        public override void ApplyOn(ContactRecord contact)
        {
            string[] o = new string[3];
            o[0] = contact.City;
            o[1] = contact.Province;
            o[2] = contact.Country;
            string[] r = CorrectCityAddress(o);
            contact.City = r[0];
            contact.Province = r[1];
            contact.Country = r[2];
        }

        public Boolean LoadCityTable()
        {
            cityTable = new List<string[]>();

            ReadFromCsvFile("city_table.csv", cityTable);
            return true;
        }

        private string[] CorrectCityAddress(string[] cityProvinceCountry)
        {
            for (int i = 0; i < cityTable.Count; ++i)
            {
                // string comparison (ignore case)
                if (String.Compare(cityTable[i][0], cityProvinceCountry[0], true) == 0)
                {
                    return cityTable[i];
                }
            }
            // not found, return the original
            return cityProvinceCountry;
        }

        private static void ReadFromCsvFile(string filePath, List<string[]> cities)
        {
            List<string> columns = new List<string>();
            using (var reader = new CsvFileReader(filePath))
            {
                while (reader.ReadRow(columns))
                {
                    cities.Add(columns.ToArray());
                }
            }
        }
    }

    class Expander : CorrectorRule
    {
        private Dictionary<string, string> map;

        public override void ApplyOn(ContactRecord contact)
        {
            string[] words = contact.Street.Split(' ');
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string word in words)
            {
                stringBuilder.Append(Expand(word) + ' ');
            }
            contact.Street = stringBuilder.ToString().TrimEnd(); ;
        }

        public Expander()
        {
            map = new Dictionary<string, string>();
        }

        public void Add(string word, string expandTo)
        {
            map.Add(word, expandTo);
        }

        string Expand(string word)
        {
            string expansion;
            // if found the key, return the abbreviation, if not, return the itself
            if (map.TryGetValue(word, out expansion))
            {
                return expansion;
            }
            else
            {
                return word;
            }
        }
    }

    class StreetNameCleaner : CorrectorRule
    {
        public override void ApplyOn(ContactRecord contact)
        {
            contact.Street = RemoveRedundantComma(contact.Street);
        }

        static string RemoveRedundantComma(string s)
        {
            string pattern = ",\\s*(Street|St\\.|Road|Rd\\.)";
            string replacement = " $1";
            return Regex.Replace(s, pattern, replacement);
        }
    }

    class PostalCodeFormatter : CorrectorRule
    {
        public override void ApplyOn(ContactRecord contact)
        {
            contact.PostalCode = FormatPostalCode(contact.PostalCode);
        }

        static string FormatPostalCode(string p)
        {
            if (p.Equals(String.Empty))
            {
                return p;
            }

            string s = p.ToUpper();
            if (Regex.IsMatch(s, "[A-Z]") && s.Length > 3)
            {
                int spaceCharIndex = s.IndexOf(' ');
                if (spaceCharIndex == -1) // if no Space char in it
                {
                    s = s.Insert(3, " ");
                }
            }
            return s;
        }
    }
}
