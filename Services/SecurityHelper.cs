using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Student_Tracker_App.Services
{
    public static class SecurityHelper
    {
        // This method takes a plain text password and returns a hashed version using SHA256
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return null;

            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2")); // Convert to hexadecimal
                }
                //Return the hexadecimal string representation of the hash
                return builder.ToString();
            }
        }
    }
}
