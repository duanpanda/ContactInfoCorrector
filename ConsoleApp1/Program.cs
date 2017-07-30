using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Data;       //To have the data objects like data tables/sets
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ContactRecord> contacts = new List<ContactRecord>();
            contacts.Add(new ContactRecord("Craig", "KielBUrger", "233 carlton St.",
                "TORONTO", "Ontario", "M3K 1L3", "USA"));
            contacts.Add(new ContactRecord("ryan", "dUaN", "14 torekqd street", "Toronto", "Nova Scotia", "M3L 2KW", "China"));

            ContactInfoCorrector corrector = new ContactInfoCorrector();
            corrector.AddRule(new CapitalizeName());
            corrector.AddRule(new CapitalizeStreet());
            CityProvinceCountryCorrector cc = new CityProvinceCountryCorrector();
            cc.LoadCityTable();
            corrector.AddRule(cc);
            Expander expander = new Expander();
            expander.Add("St.", "Street");
            corrector.AddRule(expander);

            corrector.ApplyRules(contacts);

            contacts[0].print();
            contacts[1].print();

            var contacts2 = from l in File.ReadAllLines(@"C:/Users/duanp/Desktop/enterprise_apps_interview_data.csv").Skip(1)
                        let x = l.Split(new[] { ',' })
                        select new ContactRecord
                        {
                            FirstName = x[2],
                            LastName = x[3],
                            Street = x[7],
                            City = x[4],
                            Province = x[6],
                            PostalCode = x[8],
                            Country = x[5]
                        };

            // print the first record
            IEnumerator<ContactRecord> enumerator = contacts2.GetEnumerator();
            enumerator.MoveNext();
            enumerator.Current.print();

            List<ContactRecord> contactList = contacts2.ToList();
            corrector.ApplyRules(contactList); // contactList is changed, but contacts2 is not.

            StreamWriter writer = new StreamWriter("C:/Users/duanp/Desktop/out.csv");
            foreach (ContactRecord c in contactList)
            {
                string[] columns = c.GetFields();
                int i;
                // TODO: BUG: Quotes are NOT handled!
                for (i = 0; i <= columns.Length - 2; ++i)
                {
                    writer.Write(columns[i] + ",");
                }
                writer.Write(columns[i] + "\n");
            }
            writer.Close();
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

        public ContactRecord() { }

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
            Console.WriteLine("---------------");
            Console.WriteLine(firstName + " " + lastName);
            Console.WriteLine(street);
            Console.WriteLine(city + ", " + province + " " + postalCode);
            Console.WriteLine(country);
        }

        public string[] GetFields()
        {
            string[] fields = new string[7];
            fields[0] = firstName;
            fields[1] = lastName;
            fields[2] = street;
            fields[3] = city;
            fields[4] = province;
            fields[5] = postalCode;
            fields[6] = country;
            return fields;
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

        public void ApplyRules(List<ContactRecord> contacts)
        {
            foreach (ContactRecord contact in contacts)
            {
                foreach (CorrectorRule rule in rules)
                {
                    rule.ApplyOn(contact);
                }
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
