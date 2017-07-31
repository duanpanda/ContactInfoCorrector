using System;

namespace ConsoleApp1
{
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
}
