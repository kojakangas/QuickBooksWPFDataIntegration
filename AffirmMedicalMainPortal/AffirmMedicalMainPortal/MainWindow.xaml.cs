using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AffirmMedicalMainPortal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        String enterFirst = "";
        String enterLast = "";
        String enterPhone = "";
        String enterEmail = "";
        String enterAddress = "";
        String enterCity = "";
        String enterState = "";
        int enterZip = 0;
        char enterSex = 'U';
        int foundCustFlag = 0;
        TextBlock confirmPrompt = WindowEngine.CreateTextBlock("");
        StackPanel footer = new StackPanel();
        Button foundCustomer = WindowEngine.CreateButton("Show");

        //public delegate void addCust(String enterFirst, String enterLast);

        //addCust moreCust = DatabaseEngine.AddCustomer;

        private void ButtonData_Click(object sender, RoutedEventArgs e)
        {
            NotificationBase popup = new NotificationBase("Customer Database");
            popup.Width = 400;
            popup.Height = 550;
            TextBlock whiteSpace = WindowEngine.CreateTextBlock(" ");
            TextBlock instructText = WindowEngine.CreateTextBlock("Please enter customer information to optimize search or additions:", "Center", "Top");
            //create first name section
            StackPanel firstName = new StackPanel();
            firstName.Orientation = Orientation.Horizontal;
            TextBlock firstNameLabel = WindowEngine.CreateTextBlock("First Name: ");
            TextBox firstNameField = WindowEngine.CreateTextBox(firstNameLabel.Margin.Left + 20, firstNameLabel.Margin.Top, firstNameLabel.Margin.Right, firstNameLabel.Margin.Bottom);
            //create last name section
            StackPanel lastName = new StackPanel();
            lastName.Orientation = Orientation.Horizontal;
            TextBlock lastNameLabel = WindowEngine.CreateTextBlock("Last Name: ");
            TextBox lastNameField = WindowEngine.CreateTextBox(lastNameLabel.Margin.Left + 20, lastNameLabel.Margin.Top, lastNameLabel.Margin.Right, lastNameLabel.Margin.Bottom);
            //create phone number section
            StackPanel phoneNumber = new StackPanel();
            phoneNumber.Orientation = Orientation.Horizontal;
            TextBlock phoneNumberLabel = WindowEngine.CreateTextBlock("Phone Number: ");
            TextBox phoneNumberField = WindowEngine.CreateTextBox(phoneNumberLabel.Margin.Left + 20, phoneNumberLabel.Margin.Top, phoneNumberLabel.Margin.Right, phoneNumberLabel.Margin.Bottom);
            //create eMail section
            StackPanel eMail = new StackPanel();
            eMail.Orientation = Orientation.Horizontal;
            TextBlock eMailLabel = WindowEngine.CreateTextBlock("Email Address: ");
            TextBox eMailField = WindowEngine.CreateTextBox(eMailLabel.Margin.Left + 20, eMailLabel.Margin.Top, eMailLabel.Margin.Right, eMailLabel.Margin.Bottom);
            //create street address section
            StackPanel address = new StackPanel();
            address.Orientation = Orientation.Horizontal;
            TextBlock addressLabel = WindowEngine.CreateTextBlock("Address: ");
            TextBox addressField = WindowEngine.CreateTextBox(addressLabel.Margin.Left + 20, addressLabel.Margin.Top, addressLabel.Margin.Right, addressLabel.Margin.Bottom);
            //create city section
            StackPanel city = new StackPanel();
            city.Orientation = Orientation.Horizontal;
            TextBlock cityLabel = WindowEngine.CreateTextBlock("City: ");
            TextBox cityField = WindowEngine.CreateTextBox(cityLabel.Margin.Left + 20, cityLabel.Margin.Top, cityLabel.Margin.Right, cityLabel.Margin.Bottom);
            //create state section
            StackPanel state = new StackPanel();
            state.Orientation = Orientation.Horizontal;
            TextBlock stateLabel = WindowEngine.CreateTextBlock("State: ");
            TextBox stateField = WindowEngine.CreateTextBox(stateLabel.Margin.Left + 20, stateLabel.Margin.Top, stateLabel.Margin.Right, stateLabel.Margin.Bottom);
            //create ZIP section
            StackPanel zip = new StackPanel();
            zip.Orientation = Orientation.Horizontal;
            TextBlock zipLabel = WindowEngine.CreateTextBlock("ZIP Code: ");
            TextBox zipField = WindowEngine.CreateTextBox(zipLabel.Margin.Left + 20, zipLabel.Margin.Top, zipLabel.Margin.Right, zipLabel.Margin.Bottom);
            //create gender section
            StackPanel sex = new StackPanel();
            sex.Orientation = Orientation.Horizontal;
            TextBlock sexLabel = WindowEngine.CreateTextBlock("Gender: ");
            RadioButton male = WindowEngine.CreateRadioButton("Male", sexLabel.Margin.Left + 20, sexLabel.Margin.Top, sexLabel.Margin.Right, sexLabel.Margin.Bottom);
            RadioButton female = WindowEngine.CreateRadioButton("Female", male.Margin.Left + 20, male.Margin.Top, male.Margin.Right, male.Margin.Bottom);
            ////create dateofbirth section
            StackPanel dob = new StackPanel();
            dob.Orientation = Orientation.Horizontal;
            TextBlock dobLabel = WindowEngine.CreateTextBlock("Date of Birth: ");
            DatePicker dobField = WindowEngine.CreateDatePicker(dobLabel.Margin.Left + 20, dobLabel.Margin.Top, dobLabel.Margin.Right, dobLabel.Margin.Bottom);
            
            //create footer section for feedback and options
            footer.Orientation = Orientation.Horizontal;
            //TextBlock confirmPrompt = WindowEngine.CreateTextBlock("");
            
            // create button that submits info in these text fields
            Button submitCustomer = WindowEngine.CreateButton("Add Customer");
            submitCustomer.Click += delegate { PassNewCustomer(firstNameField.Text.Trim(), lastNameField.Text.Trim(), phoneNumberField.Text.Trim(), eMailField.Text.Trim(), addressField.Text.Trim(), cityField.Text.Trim(), stateField.Text.Trim(), zipField.Text.Trim(), male, female, dobField.Text.Trim()); };
            // create button that searches for info matching these fields
            Button findCustomer = WindowEngine.CreateButton("Find Customer");
            findCustomer.Click += delegate { SearchCustomer(firstNameField.Text.Trim(), lastNameField.Text.Trim(), phoneNumberField.Text.Trim(), eMailField.Text.Trim(), addressField.Text.Trim(), cityField.Text.Trim(), stateField.Text.Trim(), zipField.Text.Trim(), male, female); };
            // create button that pops up customer window
            Button listCustomer = WindowEngine.CreateButton("Show ALL Customers");
            listCustomer.Click += delegate { PopUpCustList(); };
            popup.Options.Children.Add(instructText);
            popup.Options.Children.Add(firstName);
            firstName.Children.Add(firstNameLabel);
            firstName.Children.Add(firstNameField);
            popup.Options.Children.Add(WindowEngine.CreateTextBlock(" "));
            popup.Options.Children.Add(lastName);
            lastName.Children.Add(lastNameLabel);
            lastName.Children.Add(lastNameField);
            popup.Options.Children.Add(WindowEngine.CreateTextBlock(" "));
            popup.Options.Children.Add(phoneNumber);
            phoneNumber.Children.Add(phoneNumberLabel);
            phoneNumber.Children.Add(phoneNumberField);
            popup.Options.Children.Add(WindowEngine.CreateTextBlock(" "));
            popup.Options.Children.Add(eMail);
            eMail.Children.Add(eMailLabel);
            eMail.Children.Add(eMailField);
            popup.Options.Children.Add(WindowEngine.CreateTextBlock(" "));
            popup.Options.Children.Add(address);
            address.Children.Add(addressLabel);
            address.Children.Add(addressField);
            popup.Options.Children.Add(WindowEngine.CreateTextBlock(" "));
            popup.Options.Children.Add(city);
            city.Children.Add(cityLabel);
            city.Children.Add(cityField);
            popup.Options.Children.Add(WindowEngine.CreateTextBlock(" "));
            popup.Options.Children.Add(state);
            state.Children.Add(stateLabel);
            state.Children.Add(stateField);
            popup.Options.Children.Add(WindowEngine.CreateTextBlock(" "));
            popup.Options.Children.Add(zip);
            zip.Children.Add(zipLabel);
            zip.Children.Add(zipField);
            popup.Options.Children.Add(WindowEngine.CreateTextBlock(" "));
            popup.Options.Children.Add(sex);
            sex.Children.Add(sexLabel);
            sex.Children.Add(male);
            sex.Children.Add(female);
            popup.Options.Children.Add(WindowEngine.CreateTextBlock(" "));
            popup.Options.Children.Add(dob);
            dob.Children.Add(dobLabel);
            dob.Children.Add(dobField);
            popup.Options.Children.Add(WindowEngine.CreateTextBlock(" "));
            popup.Options.Children.Add(submitCustomer);
            popup.Options.Children.Add(findCustomer);
            popup.Options.Children.Add(listCustomer);
            popup.Options.Children.Add(WindowEngine.CreateTextBlock(" "));
            popup.Options.Children.Add(footer);
            footer.Children.Add(confirmPrompt);
            if (foundCustFlag == 1)
            {
                foundCustomer.Margin = new Thickness(confirmPrompt.Margin.Left + 20, confirmPrompt.Margin.Top, confirmPrompt.Margin.Right, confirmPrompt.Margin.Bottom);
                footer.Children.Add(foundCustomer);
            }
            popup.Show();
            this.Close();
        }

        public void PopUpCustList()
        {
            CustomerList custListWindow = new CustomerList();
            custListWindow.Width = 400;
            custListWindow.Height = 150;
            custListWindow.Show();
        }

        public void PassNewCustomer(String firstName, String lastName, String phone, String email, String address, String city, String state, String zip, RadioButton male, RadioButton female, String birthday)
        {
            char sex = 'U';
            String[] args = { firstName, lastName, phone, email, address, city, state, zip };
            int zipcode;
            bool zipcheck = Int32.TryParse(zip, out zipcode);
            if (args.Contains(""))
            {
                confirmPrompt.Text = "Please make sure you have no empty fields.";
            }
            else if (male.IsChecked == false && female.IsChecked == false)
            {
                confirmPrompt.Text = "You must specify the gender of the new patient.";
            }
            else if (!(zipcheck))
            {
                confirmPrompt.Text = "Your ZIP code must be a number.";
            }
            else
            {
                if (male.IsChecked == true)
                {
                    confirmPrompt.Text = DatabaseEngine.AddCustomer(firstName, lastName, phone, email, address, city, state, zip, 'M', birthday);
                    sex = 'M';
                }
                else
                {
                    confirmPrompt.Text = DatabaseEngine.AddCustomer(firstName, lastName, phone, email, address, city, state, zip, 'F', birthday);
                    sex = 'F';
                }
            }
            if (footer.Children.Contains(foundCustomer))
            {
                footer.Children.Remove(foundCustomer);
            }
            if (confirmPrompt.Text.Contains("Duplicates"))
            {
                enterFirst = firstName;
                enterLast = lastName;
                enterPhone = phone;
                enterEmail = email;
                enterAddress = address;
                enterCity = city;
                enterState = state;
                enterZip = zipcode;
                //String convSex = System.Convert.ToString(sex);
                //enterSex = System.Convert.ToChar(convSex.Substring(0,0));
                NotificationBase popup = new NotificationBase("Duplicate Entries");
                popup.Width = 400;
                popup.Height = 150;
                String numDup = confirmPrompt.Text.Substring(12);
                int dupCount = System.Convert.ToInt32(numDup);
                if (dupCount == 1)
                {
                    TextBlock dupContent = WindowEngine.CreateTextBlock("There is already " + dupCount + " entry of a customer with that name.");
                    dupContent.HorizontalAlignment = HorizontalAlignment.Center;
                    popup.Options.Children.Add(dupContent);
                }
                else
                {
                    TextBlock dupContent = WindowEngine.CreateTextBlock("There are already " + dupCount + " entries of customers with that name.");
                    dupContent.HorizontalAlignment = HorizontalAlignment.Center;
                    popup.Options.Children.Add(dupContent);
                }
                TextBlock whitespace = WindowEngine.CreateTextBlock("");
                popup.Options.Children.Add(whitespace);
                StackPanel choose = new StackPanel();
                choose.HorizontalAlignment = HorizontalAlignment.Center;
                choose.Orientation = Orientation.Vertical;
                popup.Options.Children.Add(choose);
                Button overwrite = WindowEngine.CreateButton("Overwrite");
                overwrite.Click += delegate { OverwriteCust(enterFirst, enterLast, enterPhone, enterEmail, enterAddress, enterCity, enterState, enterZip, enterSex); popup.Close(); };
                Button addEntry = WindowEngine.CreateButton("Add New");
                addEntry.Click += delegate { AddSimilarCust(enterFirst, enterLast, enterPhone, enterEmail, enterAddress, enterCity, enterState, enterZip, sex, birthday, dupCount); popup.Close(); };
                Button cancel = WindowEngine.CreateButton("Cancel");
                cancel.Click += delegate { popup.Close(); };
                choose.Children.Add(overwrite);
                choose.Children.Add(addEntry);
                choose.Children.Add(cancel);
                popup.Show();
            }
        }

        public void SearchCustomer(String firstName, String lastName, String phone, String email, String address, String city, String state, String zip, RadioButton male, RadioButton female)
        {
            String[] args = { firstName, lastName, phone, email, address, city, state, zip };

            if (Array.TrueForAll(args, String.IsNullOrWhiteSpace))
            {
                confirmPrompt.Text = "A search for a patient cannot be conducted on blank credentials.";
            }
            else
            {
                confirmPrompt.Text = DatabaseEngine.FindCustomer(firstName, lastName, phone, email, address, city, state, zip);
                String check = confirmPrompt.Text;
                if (!(check.Contains("None")) && !(check.Contains("Error")) && !(footer.Children.Contains(foundCustomer)))
                {
                    footer.Children.Add(foundCustomer);
                }
                else if (footer.Children.Contains(foundCustomer) && (check.Contains("None") || check.Contains("Error")))
                {
                    footer.Children.Remove(foundCustomer);
                }
            }
        }

        public void OverwriteCust(String firstName, String lastName, String phone, String email, String address, String city, String state, int zip, char sex)
        {
            confirmPrompt.Text = "This feature not available at this time.";
        }

        public void AddSimilarCust(String firstName, String lastName, String phone, String email, String address, String city, String state, int zip, char sex, String dateOfBirth, int dup)
        {
            confirmPrompt.Text = DatabaseEngine.AddSimilar(firstName, lastName, phone, email, address, city, state, zip, sex, dateOfBirth, dup);
        }
    }
}
