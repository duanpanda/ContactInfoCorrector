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
            Abbreviator ae = new Abbreviator();
            ae.Add("Ontario", "ON");
            Console.WriteLine(ae.Abbrev("Ontario"));

            Console.WriteLine("------------------------------------------------------");
            List<ContactRecord> contacts = new List<ContactRecord>();
             contacts.Add(new ContactRecord("Craig", "KielBUrger", "233 carlton St.",
                "TORONTO", "Ontario", "M3K 1L3", "USA"));
            contacts[0].print();

            ContactInfoCorrector corrector = new ContactInfoCorrector();
            corrector.AddRule(new CapitalizeName());
            corrector.AddRule(new CapitalizeStreet());
            CityProvinceCountryCorrector cc = new CityProvinceCountryCorrector();
            cc.LoadCityTable();
            corrector.AddRule(cc);
            corrector.ApplyRules(contacts[0]);

            contacts[0].print();
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

    abstract class CorrectorRule
    {
        public abstract void ApplyOn(ContactRecord contact);
    }

    class ContactInfoCorrector
    {
        private List<CorrectorRule> rules = new List<CorrectorRule>();

        public void AddRule(CorrectorRule r)
        {
            rules.Add(r);
        }

        public void ApplyRules(ContactRecord contact)
        {
            foreach (CorrectorRule rule in rules)
            {
                rule.ApplyOn(contact);
            }
        }
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
}
