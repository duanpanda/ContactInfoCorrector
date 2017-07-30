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

            Console.WriteLine("------------------------------------------------------");
            List<ContactRecord> contacts = new List<ContactRecord>();
            contacts.Add(new ContactRecord("Craig", "KielBUrger", "233 carlton St.",
                "TORONTO", "Ontario", "M3K 1L3", "USA"));
            contacts[0].print();
            contacts[0].FirstName = CapitalizeString(contacts[0].FirstName);
            contacts[0].LastName = CapitalizeString(contacts[0].LastName);
            contacts[0].Street = CapitalizeWord(contacts[0].Street);
            string[] o = new string[3];
            o[0] = contacts[0].City;
            o[1] = contacts[0].Province;
            o[2] = contacts[0].Country;
            string[] r = cc.CorrectCityAddress(o);
            contacts[0].City = r[0];
            contacts[0].Province = r[1];
            contacts[0].Country = r[2];
            contacts[0].print();
        }

        static string CapitalizeString(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1).ToLower();
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

    class ContactRecord
    {
        private string firstName;
        private string lastName;
        private string street;
        private string city;
        private string province; // or state
        private string postalCode;
        private string country;

        public ContactRecord(string firstname, string lastname, string street,
            string city, string province, string postalCode, string country)
        {
            firstName = firstname;
            lastName = lastname;
            this.street = street;
            this.city = city;
            this.province = province;
            this.postalCode = postalCode;
            this.country = country;
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        public string Street
        {
            get { return street; }
            set { street = value; }
        }
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        public string Province
        {
            get { return province; }
            set { province = value; }
        }
        public string PostalCode
        {
            get { return postalCode; }
            set { postalCode = value; }
        }
        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        public void print()
        {
            Console.WriteLine(firstName + " " + lastName);
            Console.WriteLine(street);
            Console.WriteLine(city + ", " + province + " " + postalCode);
            Console.WriteLine(country);
        }
    }
}
