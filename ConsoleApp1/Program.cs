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
            ContactInfoCorrector corrector = new ContactInfoCorrector();
            corrector.AddRule(new CapitalizeName());
            corrector.AddRule(new CapitalizeStreet());
            corrector.AddRule(new CapitalizeCity());
            CityProvinceCountryCorrector cc = new CityProvinceCountryCorrector();
            cc.LoadCityTable();
            corrector.AddRule(cc);
            Expander expander = new Expander();
            expander.Add("St.", "Street");
            expander.Add("Av.", "Avenue");
            expander.Add("Ave", "Avenue");
            expander.Add("Rd.", "Road");
            corrector.AddRule(expander);

            //string inputFileName = "in.csv";
            string inputFileName = "test.csv";
            List<ContactRecord> contactList = new List<ContactRecord>();
            ReadFromCsvFile(inputFileName, contactList);

            WriteToCsvFile("in_orig.csv", contactList);

            corrector.ApplyRules(contactList); // contactList is changed

            WriteToCsvFile("out.csv", contactList);
        }

        static void ReadFromCsvFile(string filePath, List<ContactRecord> contactList)
        {
            List<string> columns = new List<string>();
            using (var reader = new CsvFileReader(filePath))
            {
                while (reader.ReadRow(columns))
                {
                    contactList.Add(new ContactRecord(
                        columns[2], columns[3], // FirstName, LastName
                        columns[7], columns[4], // Street, City
                        columns[6], columns[8], columns[5])); // Province, PostalCode, Country
                }
            }
        }

        static void WriteToCsvFile(string filePath, List<ContactRecord> contactList)
        {
            using (CsvFileWriter writer = new CsvFileWriter(filePath))
            {
                foreach (ContactRecord c in contactList)
                {
                    List<string> columns = c.GetFields().ToList();
                    writer.WriteRow(columns);
                }
            }
        }
    }
}
