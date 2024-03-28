using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Global_Classes
{
    internal class clsValidation
    {
        /*
         
            1- Regex Class: In C#, the Regex class is used to work with regular expressions. 
            It’s part of the System.Text.RegularExpressions namespace.

            2- Creating a Regex Object: To use regex, you create an instance of the 
            Regex class and pass the pattern as a string. 

            # Regex regex = new Regex("pattern");

            3- Matching: You can check if a string matches a pattern using the IsMatch method.

            # bool isMatch = regex.IsMatch("string to test");

            4- Patterns: Patterns are the heart of regex. They define the criteria for matching strings. 
            For example:
                ^ matches the start of a string.
                $ matches the end of a string.
                . matches any single character.
                * matches zero or more occurrences of the preceding element.
                + matches one or more occurrences of the preceding element.
                ? matches zero or one occurrence of the preceding element.
                [abc] matches any single character a, b, or c.
                [^abc] matches any single character that is not a, b, or c.
                (abc) matches the exact sequence “abc”.
        */
        
        public static bool ValidateEmail(string email)
        {
            var pattern = @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";

            var regex = new Regex(pattern);

            return regex.IsMatch(email);
        }

        public static bool ValidateInteger(string Number)
        {
            var pattern = @"^[0-9]*$";

            var regex = new Regex(pattern);

            return regex.IsMatch(Number);
        }

        public static bool ValidateFloat(string Number)
        {
            var pattern = @"^[0-9]*(?:\.[0-9]*)?$";

            var regex = new Regex(pattern);

            return regex.IsMatch(Number);
        }

        public static bool IsNumber(string Number)
        {
            return (ValidateInteger(Number) || ValidateFloat(Number));
        }

        public static void KeyPressHandle(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        public static void KeyPressHandleFloatNumber(object sender, KeyPressEventArgs e)
        {
            TextBox Field = (TextBox)sender;

            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && (e.KeyChar != (char)46);
            e.Handled = e.Handled = (e.KeyChar == (char)46) && (Field.Text.IndexOf('.') > -1);
        }
    }
}

