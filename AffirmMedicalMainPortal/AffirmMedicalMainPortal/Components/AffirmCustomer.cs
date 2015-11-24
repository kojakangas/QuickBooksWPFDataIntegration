using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AffirmMedicalMainPortal.Components
{
    class AffirmCustomer
    {
        //instance variables for customer object
        String firstName;
        String lastName;
        String primaryPhone;
        String homePhone;
        String mobilePhone;
        String workPhone;
        String primaryEmail;
        String secondaryEmail;
        String address;
        String city;
        String state;
        int zip;
        char gender;

        public AffirmCustomer()
        {

        }

        public AffirmCustomer(String firstName, String lastName)
        {
            this.firstName = firstName;
            this.lastName = lastName;
        }

        public AffirmCustomer(String firstName, String lastName, String phone)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.primaryPhone = phone;
        }

        //public Customer(String firstName, String lastName, String email)
        //{
        //    this.firstName = firstName;
        //    this.lastName = lastName;
        //    this.primaryEmail = email;
        //}

        public AffirmCustomer(String firstName, String lastName, String phone, String email)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.primaryPhone = phone;
            this.primaryEmail = email;
        }

        public AffirmCustomer(String firstName, String lastName, String phone, String email, String address, String city, String state, int zip, char sex)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.primaryPhone = phone;
            this.primaryEmail = email;
            this.address = address;
            this.city = city;
            this.zip = zip;
            this.gender = sex;
        }

        //getters and setters for our customers
        public String FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public String LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public String PrimaryPhone
        {
            get { return primaryPhone; }
            set { primaryPhone = value; }
        }

        public String HomePhone
        {
            get { return homePhone; }
            set { homePhone = value; }
        }

        public String MobilePhone
        {
            get { return mobilePhone; }
            set { mobilePhone = value; }
        }

        public String WorkPhone
        {
            get { return workPhone; }
            set { workPhone = value; }
        }

        public String PrimaryEmail
        {
            get { return primaryEmail; }
            set { primaryEmail = value; }
        }

        public String SecondaryEmail
        {
            get { return secondaryEmail; }
            set { secondaryEmail = value; }
        }

        public String Address
        {
            get { return address; }
            set { address = value; }
        }

        public String City
        {
            get { return city; }
            set { city = value; }
        }

        public String State
        {
            get { return state; }
            set { state = value; }
        }

        public int ZIP
        {
            get { return zip; }
            set { zip = value; }
        }

        public char Gender
        {
            get { return gender; }
            set { gender = value; }
        }
    }
}
