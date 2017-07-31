using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ConsoleApp1
{
    abstract class CorrectorRule
    {
        public abstract void ApplyOn(ContactRecord contact);
    }

    class CapitalizeName : CorrectorRule
    {
        public override void ApplyOn(ContactRecord contact)
        {
            contact.FirstName = CapitalizeString(contact.FirstName);
            contact.LastName = CapitalizeString(contact.LastName);
        }

        static string CapitalizeString(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1).ToLower();
        }
    }

    class CapitalizeStreet : CorrectorRule
    {
        public override void ApplyOn(ContactRecord contact)
        {
            contact.Street = CapitalizeWord(contact.Street);
        }

        static string CapitalizeWord(string s)
        {
            TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;
            string output = cultInfo.ToTitleCase(s);
            return output;
        }
    }

    class CapitalizeCity : CorrectorRule
    {
        public override void ApplyOn(ContactRecord contact)
        {
            contact.City = CapitalizeWord(contact.City);
        }

        static string CapitalizeWord(string s)
        {
            TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;
            string output = cultInfo.ToTitleCase(s);
            return output;
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

            string[] city = new string[3];
            city[0] = "Toronto";
            city[1] = "ON";
            city[2] = "Canada";

            cityTable.Add(city);
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
}
