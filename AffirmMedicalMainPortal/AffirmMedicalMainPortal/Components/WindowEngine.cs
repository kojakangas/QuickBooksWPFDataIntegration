using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AffirmMedicalMainPortal
{
    static class WindowEngine
    {
        public static DataTemplate CustomerComboBoxView(TemplateBindingExpression customer)
        {
            DataTemplate stuff = new DataTemplate();
            
            return stuff;
        }

        public static Button CreateButton(String text)
        {
            Button basicButton = new Button();
            basicButton.Height = 20;
            basicButton.Width = 150;
            basicButton.Content = text;
            return basicButton;
        }

        public static Button CreateButton(String text, int height, int width)
        {
            Button basicButton = new Button();
            basicButton.Height = height;
            basicButton.Width = width;
            basicButton.Content = text;
            return basicButton;
        }

        public static Button CreateButton(String text, int height, int width, String horizAlign, String vertAlign)
        {
            Button basicButton = new Button();
            basicButton.Height = height;
            basicButton.Width = width;
            basicButton.Content = text;
            basicButton.SetBinding(Button.HorizontalAlignmentProperty, horizAlign);
            basicButton.SetBinding(Button.VerticalAlignmentProperty, vertAlign);
            return basicButton;
        }

        public static Button CreateButton(String text, Delegate onClick)
        {
            Button basicButton = new Button();
            basicButton.Height = 50;
            basicButton.Width = 150;
            basicButton.Content = text;
            basicButton.Click += (RoutedEventHandler)onClick;
            return basicButton;
        }

        public static Button CreateButton(String text, int height, int width, Delegate onClick)
        {
            Button basicButton = new Button();
            basicButton.Height = height;
            basicButton.Width = width;
            basicButton.Content = text;
            basicButton.Click += (RoutedEventHandler)onClick;
            return basicButton;
        }

        public static Button CreateButton(String text, int height, int width, String horizAlign, String vertAlign, Delegate onClick)
        {
            Button basicButton = new Button();
            basicButton.Height = height;
            basicButton.Width = width;
            basicButton.Content = text;
            basicButton.SetBinding(Button.HorizontalAlignmentProperty, horizAlign);
            basicButton.SetBinding(Button.VerticalAlignmentProperty, vertAlign);
            basicButton.Click += (RoutedEventHandler)onClick;
            return basicButton;
        }

        public static TextBlock CreateTextBlock(String text)
        {
            TextBlock basicTextBlock = new TextBlock();
            basicTextBlock.Text = text;
            return basicTextBlock;
        }

        public static TextBlock CreateTextBlock(String text, String horizAlign, String vertAlign)
        {
            TextBlock basicTextBlock = new TextBlock();
            basicTextBlock.Text = text;
            basicTextBlock.Margin = new Thickness(0, 10, 0, 10);
            //basicTextBlock.SetBinding(TextBlock.VerticalAlignmentProperty, vertAlign);
            return basicTextBlock;
        }

        public static TextBox CreateTextBox()
        {
            TextBox basicTextBox = new TextBox();
            basicTextBox.Height = 25;
            basicTextBox.Width = 150;
            basicTextBox.Margin = new Thickness(10, 10, 10, 10);
            //basicTextBox.SetBinding(TextBox.VerticalAlignmentProperty, "Bottom");
            return basicTextBox;
        }

        public static TextBox CreateTextBox(double left, double top, double right, double bottom)
        {
            TextBox basicTextBox = new TextBox();
            basicTextBox.Height = 25;
            basicTextBox.Width = 150;
            basicTextBox.Margin = new Thickness(left, top, right, bottom);
            return basicTextBox;
        }

        public static ComboBox CreateComboBox(String unselected)
        {
            ComboBox cbox = new ComboBox();
            cbox.Text = unselected;
            //cbox.Background = Brushes.LightBlue;
            ComboBoxItem cboxitem = new ComboBoxItem();
            cboxitem.Content = "Created with C#";
            cbox.Items.Add(cboxitem);
            ComboBoxItem cboxitem2 = new ComboBoxItem();
            cboxitem2.Content = "Item 2";
            cbox.Items.Add(cboxitem2);
            ComboBoxItem cboxitem3 = new ComboBoxItem();
            cboxitem3.Content = "Item 3";
            cbox.Items.Add(cboxitem3);
            return cbox;
        }

        public static ComboBox CreateComboBox(String unselected, double left, double top, double right, double bottom)
        {
            ComboBox cbox = new ComboBox();
            cbox.SelectedIndex = 0;
            //cbox.Background = Brushes.LightBlue;
            ComboBoxItem cboxitem = new ComboBoxItem();
            cboxitem.Content = "Created with C#";
            
            cbox.Items.Add(cboxitem);
            ComboBoxItem cboxitem2 = new ComboBoxItem();
            cboxitem2.Content = "Item 2";
            cbox.Items.Add(cboxitem2);
            ComboBoxItem cboxitem3 = new ComboBoxItem();
            cboxitem3.Content = "Item 3";
            cbox.Items.Add(cboxitem3);
            return cbox;
        }

        public static DatePicker CreateDatePicker()
        {
            DatePicker basicDatePicker = new DatePicker();
            basicDatePicker.Height = 25;
            basicDatePicker.Width = 150;
            basicDatePicker.Margin = new Thickness(10, 10, 10, 10);
            return basicDatePicker;
        }

        public static DatePicker CreateDatePicker(double left, double top, double right, double bottom)
        {
            DatePicker basicDatePicker = new DatePicker();
            basicDatePicker.Height = 25;
            basicDatePicker.Width = 150;
            basicDatePicker.Margin = new Thickness(left, top, right, bottom);
            return basicDatePicker;
        }

        public static RadioButton CreateRadioButton(String content, double left, double top, double right, double bottom)
        {
            RadioButton rButt = new RadioButton();
            rButt.Content = content;
            rButt.Margin = new Thickness(left, top, right, bottom);
            return rButt;
        }
    }
}
