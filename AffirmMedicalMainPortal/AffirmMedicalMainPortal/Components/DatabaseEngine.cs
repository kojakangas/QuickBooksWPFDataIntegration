using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using MySql.Data;
using MySql.Web;
using MySql.Data.MySqlClient;
using System.Web;
using System.Configuration;
using System.Globalization;
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.Security;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.LinqExtender;
using QBFC13Lib;
using AffirmMedicalMainPortal.Components;
using System.Convert;



namespace AffirmMedicalMainPortal
{
    class DatabaseEngine : DataBaseParent
    {
        //affirmmedicalweightlossEntities db = new affirmmedicalweightlossEntities();

        public static String AddCustomer(String firstName, String lastName, String phone, String eMail, String address, String city, String state, String zip, char sex, String dateOfBirth)
        {
            string connStr = ConfigurationManager.ConnectionStrings["affirmWL"].ConnectionString;

            MySqlConnection msqcon = new MySqlConnection(connStr);
            MySqlCommand msqcom = new MySqlCommand(connStr);

            String messageFromSDK = "";

            String Salutation = "";
            if (sex == 'M')
            {
                Salutation = "Mr.";
            }
            else
            {
                Salutation = "Ms.";
            }

            //create a new Customer object that we will add to the DB through Entity Framework
            customer newCustomer = new customer();

            //instantiate properties we already know about the Customer to add
            newCustomer.Name = (firstName + " " + lastName);
            newCustomer.FirstName = firstName;
            newCustomer.LastName = lastName;
            newCustomer.BillAddress_Addr1 = address;
            newCustomer.BillAddress_City = city;
            newCustomer.BillAddress_State = state;
            newCustomer.BillAddress_PostalCode = zip;
            newCustomer.Phone = phone;
            newCustomer.Email = eMail;
            newCustomer.Salutation = Salutation;
            newCustomer.Birthday = dateOfBirth;

            //add customer to QuickBooks file
            try
            {
                //Create the session Manager object
                QBSessionManager sessionManager = new QBSessionManager();

                //Create the message set request object to hold our request
                IMsgSetRequest requestMsgSet = sessionManager.CreateMsgSetRequest("US", 8, 0);

                requestMsgSet.Attributes.OnError = ENRqOnError.roeContinue;

                //Connect to QuickBooks and begin a session
                sessionManager.OpenConnection(@"C:\Users\User\Documents\Affirm Medical Weightloss.QBW", "Affirm Medical Main Data Portal");
                //sessionManager.OpenConnection(@"\\USER-PC\quickbooks_affirm14\Affirm Medical Weightloss", "Affirm Medical Main Data Portal");
                //sessionManager.BeginSession("C:/Users/User/Documents/Affirm Medical Weightloss.QBW", ENOpenMode.omDontCare);
                sessionManager.BeginSession("", ENOpenMode.omDontCare);
                //sessionBegun = true;

                //find customer



                //add the customer
                ICustomerAdd customerAddRq = requestMsgSet.AppendCustomerAddRq();
                customerAddRq.Name.SetValue(newCustomer.Name);
                customerAddRq.FirstName.SetValue(firstName);
                customerAddRq.LastName.SetValue(lastName);
                customerAddRq.Phone.SetValue(phone);
                customerAddRq.Email.SetValue(eMail);
                customerAddRq.BillAddress.Addr1.SetValue(address);
                customerAddRq.BillAddress.City.SetValue(city);
                customerAddRq.BillAddress.State.SetValue(state);
                customerAddRq.BillAddress.PostalCode.SetValue(zip);
                customerAddRq.Salutation.SetValue(Salutation);

                //modify CustomField1 (Birthday field) for the new customer
                IDataExtMod customerExtModRq = requestMsgSet.AppendDataExtModRq();
                customerExtModRq.OwnerID.SetValue("0");
                customerExtModRq.DataExtName.SetValue("Birthday");
                customerExtModRq.DataExtValue.SetValue(dateOfBirth);
                customerExtModRq.ORListTxn.ListDataExt.ListDataExtType.SetValue(ENListDataExtType.ldetCustomer);
                customerExtModRq.ORListTxn.ListDataExt.ListObjRef.FullName.SetValue(newCustomer.Name);

                //Send the request and get the response from QuickBooks
                IMsgSetResponse responseMsgSet = sessionManager.DoRequests(requestMsgSet);
                IResponse response = responseMsgSet.ResponseList.GetAt(0);
                ICustomerRet customerRet = (ICustomerRet)response.Detail;

                messageFromSDK = responseMsgSet.ResponseList + response.StatusMessage + customerRet;

                newCustomer.ListID = customerRet.ListID.GetValue();
                newCustomer.TimeCreated = System.Convert.ToString(customerRet.TimeCreated.GetValue());
                newCustomer.TimeModified = System.Convert.ToString(customerRet.TimeModified.GetValue());
                newCustomer.EditSequence = customerRet.EditSequence.GetValue();

                sessionManager.EndSession();

                sessionManager.CloseConnection();

                //Customer.QuickBooksID = customerRet.ListID.GetValue();
            }

            catch (Exception ex)
            {
                return "There was an error attempting to connect to\nQuickBooks for customer additions:\n" + ex.Message;
            }

            //add customer to local DB
            try
            {
                msqcon.Open();
                String query = "SELECT COUNT(*) FROM customer WHERE firstName = '" + firstName + "' AND lastName = '" + lastName + "'";
                MySqlCommand msqfetch = new MySqlCommand(query, msqcon);
                MySqlDataReader msqfind = msqfetch.ExecuteReader();
                int duplicates = 0;
                while (msqfind.Read())
                {
                    duplicates = System.Convert.ToInt32(msqfind["COUNT(*)"]);
                }
                if (duplicates > 0)
                {
                    msqcon.Close();
                    return "Duplicates: " + duplicates;
                }
                else
                {
                    msqcon.Close();
                    db.Entry(newCustomer).State = EntityState.Added;
                    db.SaveChanges();
                    //query = "INSERT INTO customers (FirstName, LastName, Phone, Email, BillAddress_Addr1, BillAddress_City, BillAddress_State, BillAddress_PostalCode, Salutation, DateOfBirth)" +
                    //    "Values ('" + firstName + "', '" + lastName + "', '" + phone + "', '" + eMail + "', '" + address +
                    //    "', '" + city + "', '" + state + "', '" + zip + "', '" + Salutation + "', '" + dateOfBirth + "')";
                    //msqcom = new MySqlCommand(query, msqcon);
                    //msqcom.ExecuteNonQuery();
                    //msqcon.Close();
                }
            }
            catch (MySqlException e)
            {
                return "Error connecting to database! Check your connection\nand make sure your database is online and accessible: "
                    + e.Message;
            }


            //return "Customer successfully added to database and QuickBooks.";
            return "Operation successful, following message returned from QuickBooks: " + messageFromSDK;
        }

        public static String AddSimilar(String firstName, String lastName, String phone, String eMail, String address, String city, String state, int zip, char sex, String dateOfBirth, int dupInc)
        {
            string connStr = ConfigurationManager.ConnectionStrings["affirmWL"].ConnectionString;

            MySqlConnection msqcon = new MySqlConnection(connStr);
            MySqlCommand msqcom = new MySqlCommand(connStr);
            try
            {
                msqcon.Open();
                String query = "SELECT * FROM customers WHERE firstName = '" + firstName + "' AND lastName = '" + lastName + "'";
                MySqlCommand msqfetch = new MySqlCommand(query, msqcon);
                MySqlDataReader msqfind = msqfetch.ExecuteReader();
                String addressMatch = "";
                String cityMatch = "";
                String stateMatch = "";
                String dateOfBirthMatch = "";
                int zipMatch = 0;
                while (msqfind.Read())
                {
                    addressMatch = System.Convert.ToString(msqfind["BillAddress_Addr1"]);
                    cityMatch = System.Convert.ToString(msqfind["BillAddress_City"]);
                    stateMatch = System.Convert.ToString(msqfind["BillAddress_State"]);
                    zipMatch = System.Convert.ToInt32(msqfind["BillAddress_PostalCode"]);
                    dateOfBirthMatch = System.Convert.ToString(msqfind["Birthday"]);
                }
                if (address.Equals(addressMatch) && city.Equals(cityMatch) && state.Equals(stateMatch) && (zip == zipMatch) && dateOfBirth.Equals(dateOfBirthMatch))
                {
                    msqcon.Close();
                    return "No duplicate entries allowed (exact same address information).";
                }
                else
                {
                    msqfind.Close();
                    msqcon.Close();

                    String Salutation = "";
                    if (sex == 'M')
                    {
                        Salutation = "Mr.";
                    }
                    else
                    {
                        Salutation = "Ms.";
                    }

                    customer newCustomer = new customer();

                    //instantiate properties we already know about the Customer to add
                    newCustomer.Name = (firstName + " " + lastName);
                    newCustomer.FirstName = firstName;
                    newCustomer.LastName = lastName;
                    newCustomer.BillAddress_Addr1 = address;
                    newCustomer.BillAddress_City = city;
                    newCustomer.BillAddress_State = state;
                    newCustomer.BillAddress_PostalCode = System.Convert.ToString(zip);
                    newCustomer.Phone = phone;
                    newCustomer.Email = eMail;
                    newCustomer.Salutation = Salutation;
                    newCustomer.Birthday = dateOfBirth;
                    //query = "INSERT INTO customers (firstName, lastName, primaryPhone, primaryEmail, address, city, state, zip, gender)" +
                    //    "Values ('" + firstName + "', '" + lastName + "', '" + phone + "', '" + eMail + "', '" + address +
                    //    "', '" + city + "', '" + state + "', '" + zip + "', '" + sex + "')";
                    //msqcom = new MySqlCommand(query, msqcon);
                    //msqcom.ExecuteNonQuery();
                    msqcon.Close();
                }
            }
            catch (MySqlException e)
            {
                return "Error connecting to database! Check your connection\nand make sure your database is online and accessible.";
            }

            try
            {
                //Create the session Manager object
                QBSessionManager sessionManager = new QBSessionManager();

                //Create the message set request object to hold our request
                IMsgSetRequest requestMsgSet = sessionManager.CreateMsgSetRequest("US", 8, 0);

                requestMsgSet.Attributes.OnError = ENRqOnError.roeContinue;

                //Connect to QuickBooks and begin a session
                sessionManager.OpenConnection(@"C:\Users\User\Documents\Affirm Medical Weightloss.QBW", "Affirm Medical Main Data Portal");
                //sessionManager.OpenConnection(@"\\USER-PC\quickbooks_affirm14\Affirm Medical Weightloss", "Affirm Medical Main Data Portal");
                //sessionManager.BeginSession("C:/Users/User/Documents/Affirm Medical Weightloss.QBW", ENOpenMode.omDontCare);
                sessionManager.BeginSession("C:\\Users\\User\\Documents\\Affirm Medical Weightloss.QBW", ENOpenMode.omDontCare);
                //sessionBegun = true;

                //add the customer
                ICustomerAdd customerAddRq = requestMsgSet.AppendCustomerAddRq();
                customerAddRq.Name.SetValue(firstName + " " + lastName + "(" + dupInc + ")");
                customerAddRq.FirstName.SetValue(firstName);
                customerAddRq.LastName.SetValue(lastName);
                customerAddRq.Phone.SetValue(phone);
                customerAddRq.Email.SetValue(eMail);
                customerAddRq.BillAddress.Addr1.SetValue(address);
                customerAddRq.BillAddress.City.SetValue(city);
                customerAddRq.BillAddress.State.SetValue(state);
                customerAddRq.BillAddress.PostalCode.SetValue(System.Convert.ToString(zip));

                if (sex.Equals('M'))
                {
                    customerAddRq.Salutation.SetValue("Mr.");
                }
                else
                {
                    customerAddRq.Salutation.SetValue("Ms.");
                }

                //modify CustomField1 (Birthday field) for the new customer
                IDataExtMod customerExtModRq = requestMsgSet.AppendDataExtModRq();
                customerExtModRq.OwnerID.SetValue("0");
                customerExtModRq.DataExtName.SetValue("Birthday");
                customerExtModRq.DataExtValue.SetValue(dateOfBirth);
                customerExtModRq.ORListTxn.ListDataExt.ListDataExtType.SetValue(ENListDataExtType.ldetCustomer);
                customerExtModRq.ORListTxn.ListDataExt.ListObjRef.FullName.SetValue(firstName + " " + lastName + "(" + dupInc + ")");

                //Send the request and get the response from QuickBooks
                IMsgSetResponse responseMsgSet = sessionManager.DoRequests(requestMsgSet);
                IResponse response = responseMsgSet.ResponseList.GetAt(0);
                ICustomerRet customerRet = (ICustomerRet)response.Detail;

                sessionManager.EndSession();

                sessionManager.CloseConnection();

                //Customer.QuickBooksID = customerRet.ListID.GetValue();
            }

            catch (Exception ex)
            {
                return "There was an error attempting to connect to\nQuickBooks for customer additions:\n" + ex.Message;
            }
            return "This similar customer was successfully\nadded to database and QuickBooks.";
        }

        public static String ReplaceSimilar(String firstName, String lastName, String phone, String eMail, String address, String city, String state, int zip, char sex, int dupInc, int dupSel)
        {
            string connStr = ConfigurationManager.ConnectionStrings["affirmWL"].ConnectionString;

            MySqlConnection msqcon = new MySqlConnection(connStr);
            MySqlCommand msqcom = new MySqlCommand(connStr);
            if (dupInc == 1)
            {
                try
                {
                    msqcon.Open();
                    String query = "UPDATE customers SET firstName = 'Replaced', lastName = 'Replaced', primaryPhone = 'Replaced'," +
                    "primaryEmail = 'Replaced', address = 'Replaced', city = 'Replaced', state = 'Replaced', zip = '1111', gender = 'U' " +
                    "WHERE firstName = '" + firstName + "' AND lastName = '" + lastName + "';";
                    msqcom = new MySqlCommand(query, msqcon);
                    msqcom.ExecuteNonQuery();
                    msqcon.Close();
                }
                catch (MySqlException e)
                {
                    return "Error connecting to database! Check your connection\nand make sure your database is online and accessible.";
                }

                try
                {
                    //Create the session Manager object
                    QBSessionManager sessionManager = new QBSessionManager();

                    //Create the message set request object to hold our request
                    IMsgSetRequest requestMsgSet = sessionManager.CreateMsgSetRequest("US", 8, 0);

                    requestMsgSet.Attributes.OnError = ENRqOnError.roeContinue;

                    //Connect to QuickBooks and begin a session
                    sessionManager.OpenConnection(@"C:\Users\User\Documents\Affirm Medical Weightloss.QBW", "Affirm Medical Main Data Portal");
                    //sessionManager.OpenConnection(@"\\USER-PC\quickbooks_affirm14\Affirm Medical Weightloss", "Affirm Medical Main Data Portal");
                    //sessionManager.BeginSession("C:/Users/User/Documents/Affirm Medical Weightloss.QBW", ENOpenMode.omDontCare);
                    sessionManager.BeginSession("C:\\Users\\User\\Documents\\Affirm Medical Weightloss.QBW", ENOpenMode.omDontCare);
                    //sessionBegun = true;

                    //add the customer

                    ICustomerQuery customerFindRq = requestMsgSet.AppendCustomerQueryRq();
                    customerFindRq.ORCustomerListQuery.CustomerListFilter.FromModifiedDate.SetValue(System.Convert.ToDateTime(""), false);

                    ICustomerMod customerRepRq = requestMsgSet.AppendCustomerModRq();
                    customerRepRq.Name.SetValue(firstName + " " + lastName + "(" + dupInc + ")");
                    customerRepRq.FirstName.SetValue(firstName);
                    customerRepRq.LastName.SetValue(lastName);
                    customerRepRq.Phone.SetValue(phone);
                    customerRepRq.Email.SetValue(eMail);
                    customerRepRq.BillAddress.Addr1.SetValue(address);
                    customerRepRq.BillAddress.City.SetValue(city);
                    customerRepRq.BillAddress.State.SetValue(state);
                    customerRepRq.BillAddress.PostalCode.SetValue(System.Convert.ToString(zip));
                    if (sex.Equals('M'))
                    {
                        customerRepRq.Salutation.SetValue("Mr.");
                    }
                    else
                    {
                        customerRepRq.Salutation.SetValue("Ms.");
                    }

                    //Send the request and get the response from QuickBooks
                    IMsgSetResponse responseMsgSet = sessionManager.DoRequests(requestMsgSet);
                    IResponse response = responseMsgSet.ResponseList.GetAt(0);
                    ICustomerRet customerRet = (ICustomerRet)response.Detail;

                    sessionManager.EndSession();

                    sessionManager.CloseConnection();

                    //Customer.QuickBooksID = customerRet.ListID.GetValue();
                }

                catch (Exception ex)
                {
                    return "There was an error attempting to connect to\nQuickBooks for customer additions:\n" + ex.Message;
                }
                return "This similar customer was successfully\nreplaced in database and QuickBooks.";
            }
            else
            {
                return "You can only overwrite one record at this time.";
            }
        }

        public static String FindCustomer(String firstName, String lastName, String phone, String eMail, String address, String city, String state, String zip)
        {
            string connStr = ConfigurationManager.ConnectionStrings["affirmWL"].ConnectionString;
            MySqlConnection msqcon = new MySqlConnection(connStr);
            MySqlCommand msqcom = new MySqlCommand(connStr);
            List<AffirmCustomer> customers = new List<AffirmCustomer>();
            try
            {
                msqcon.Open();
                String query = "SELECT * FROM customer WHERE firstName = '" + firstName + "' OR " +
                "lastName = '" + lastName + "' OR Phone = '" + phone + "' OR Email = '" + eMail
                + "' OR address = '" + address + "' OR city = '" + city + "' OR state = '" + state
                + "' OR zip = '" + zip + "'";
                MySqlCommand msqfetch = new MySqlCommand(query, msqcon);
                MySqlDataReader msqfind = msqfetch.ExecuteReader();
                while (msqfind.Read())
                {
                    customers.Add(new AffirmCustomer(System.Convert.ToString(msqfind["firstName"]),
                        System.Convert.ToString(msqfind["lastName"]), System.Convert.ToString(msqfind["primaryPhone"]),
                        System.Convert.ToString(msqfind["primaryEmail"]), System.Convert.ToString(msqfind["address"]),
                        System.Convert.ToString(msqfind["city"]), System.Convert.ToString(msqfind["state"]),
                        System.Convert.ToInt32(msqfind["zip"]), System.Convert.ToChar(msqfind["gender"])));
                }
                msqcon.Close();
            }
            catch (MySqlException e)
            {
                return "Error connecting to database! Check your connection\nand make sure your database is online and accessible.";
            }
            if (customers.Count == 1)
            {
                return "We found " + customers.Count + " possible match.  ";
            }
            else if (customers.Count > 1)
            {
                return "We found " + customers.Count + " possible matches.  ";
            }
            else
            {
                return "None of these customers exist.";
            }
        }

        //our method to check and update the DB from QuickBooks
        public static String checkSync()
        {
            try
            {
                //Create the session Manager object
                QBSessionManager sessionManager = new QBSessionManager();

                //Create the message set request object to hold our request
                IMsgSetRequest requestMsgSet = sessionManager.CreateMsgSetRequest("US", 8, 0);

                requestMsgSet.Attributes.OnError = ENRqOnError.roeContinue;

                //Connect to QuickBooks and begin a session
                sessionManager.OpenConnection(@"C:\Users\User\Documents\Affirm Medical Weightloss.QBW", "Affirm Medical Main Data Portal");
                //sessionManager.OpenConnection(@"\\USER-PC\quickbooks_affirm14\Affirm Medical Weightloss", "Affirm Medical Main Data Portal");
                //sessionManager.BeginSession("C:/Users/User/Documents/Affirm Medical Weightloss.QBW", ENOpenMode.omDontCare);
                sessionManager.BeginSession("C:\\Users\\User\\Documents\\Affirm Medical Weightloss.QBW", ENOpenMode.omDontCare);
                //sessionBegun = true;

                //check for new customers
                ICustomerQuery customerFindRq = requestMsgSet.AppendCustomerQueryRq();
                customerFindRq.ORCustomerListQuery.CustomerListFilter.FromModifiedDate.SetValue(System.Convert.ToDateTime(""), false);
                customerFindRq.ORCustomerListQuery.CustomerListFilter.ToModifiedDate.SetValue(System.Convert.ToDateTime(""), false);
                
                //check to see if we have our private sync date extention for our customers. If not, we add one.
                IDataExtDefQuery dataExtRq = requestMsgSet.AppendDataExtDefQueryRq();

                dataExtRq.ORDataExtDefQuery.AssignToObjectList.Add(ENAssignToObject.atoCustomer);
                
               
                customerFindRq.ORCustomerListQuery.CustomerListFilter.ActiveStatus.SetValue(ENActiveStatus.asActiveOnly);

                IMsgSetResponse responseSet = sessionManager.DoRequests(requestMsgSet);
                sessionManager.EndSession();
                sessionManager.CloseConnection();

                IResponse response;
                ENResponseType responseType;

                for (int i = 0; i < responseSet.ResponseList.Count; i++)
                {
                    response = responseSet.ResponseList.GetAt(i);

                    if (response.Detail == null)
                        continue;

                    responseType = (ENResponseType)response.Type.GetValue();
                    if (responseType == ENResponseType.rtCustomerQueryRs)
                    {
                        ICustomerRetList customerList = (ICustomerRetList)response.Detail;
                        for (int customerIndex = 0; customerIndex < customerList.Count; customerIndex++)
                        {
                            ICustomerRet customer = (ICustomerRet)customerList.GetAt(customerIndex);

                            if (customer != null && customer.Name != null && (customer.TimeCreated == customer.TimeModified))
                                db.Entry(customer).State = EntityState.Added;
                        }
                    }
                }

                return "Application has been synced with QuickBooks file.";

            }

            catch (Exception ex)
            {
                return "There was an error attempting to connect to\nQuickBooks for customer additions:\n" + ex.Message;
            }


        }

    }
}
