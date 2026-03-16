using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Student_Tracker_App.Services
{
    public static class ValidationHelper
    {
        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;

            // REGEX BREAKDOWN:
            // (?=.*[a-z]) : Must contain at least one lowercase letter
            // (?=.*[A-Z]) : Must contain at least one uppercase letter
            // (?=.*\d)    : Must contain at least one number
            // .{6,}       : Must be at least 6 characters long
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$");
            return regex.IsMatch(password);
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            // REGEX BREAKDOWN:
            // ^[^@\s]+ : Starts with characters that are not '@' or whitespace
            // @        : Must contain the '@' symbol
            // [^@\s]+  : Domain name (no '@' or whitespace)
            // \.       : Must contain a literal dot '.'
            // [^@\s]+$ : Extension at the end (no '@' or whitespace)
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email);
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return false;

            // REGEX BREAKDOWN:
            // \d{3}-\d{3}-\d{4}   : Matches exactly 555-555-5555
            // |                   : OR
            // \(\d{3}\) \d{3}-\d{4} : Matches exactly (555) 555-5555
            // Because it only looks for \d (digits) and specific punctuation, letters will instantly fail.
            var regex = new Regex(@"^(\d{3}-\d{3}-\d{4}|\(\d{3}\) \d{3}-\d{4})$");
            return regex.IsMatch(phoneNumber);
        }
    }
}
