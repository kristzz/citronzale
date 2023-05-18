using LogSignIn;
using System;
using System.IO;

namespace Profile
{

    public class UserInfoReader
    {
        public static void ReadUserInfo(string filePath, string targetUsername)
        {
            // Read the contents of the text file
            string[] lines = File.ReadAllLines(filePath);

            // Find the profile with the matching username and display the information in the console
            foreach (string line in lines)
            {
                string[] fields = line.Split(','); // Split the line using the comma as the delimiter

                // Assuming the order of fields is: name, last name, username, password, and additional details
                string name = fields[0];
                string lastName = fields[1];
                string username = fields[2];
                string additionalDetails = fields[4];

                if (username == targetUsername)
                {
                    Console.WriteLine("======================================================================");
                    Console.WriteLine("Name: " + name);
                    Console.WriteLine("Last Name: " + lastName);
                    Console.WriteLine("Username: " + username);
                    Console.WriteLine("Subscription type: " + additionalDetails);
                    Console.WriteLine("======================================================================");
                    Console.WriteLine();
                    break; // Exit the loop after finding the matching profile
                }
            }
        }
    }
}