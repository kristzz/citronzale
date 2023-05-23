using LogSignIn;
using System;
using System.IO;
using Locations;

namespace Profile
{
    public class UserInfoReader
    {
        public static void ReadUserInfo(string filePath, string targetUsername)
        {
            // Attempt to open the file with FileShare.ReadWrite flag to allow reading while it is open by another process
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                // Read the contents of the text file
                using (var reader = new StreamReader(fileStream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] fields = line.Split(','); // Split the line using the comma as the delimiter


                        // Assuming the order of fields is: name, last name, username, password, and additional details
                        string name = fields[0];
                        string lastName = fields[1];
                        string username = fields[2];
                        string additionalDetails = fields[4];
                        string loaction = fields[5];
                        

                        if (username == targetUsername)
                        {
                            Console.WriteLine("======================================================================");
                            
                            Console.WriteLine("Name: " + name);
                            Console.WriteLine("Last Name: " + lastName);
                            Console.WriteLine("Username: " + username);
                            Console.WriteLine("Subscription type: " + additionalDetails);
                            Console.WriteLine("Location: " + loaction);

                            Console.WriteLine("======================================================================");
                            Console.WriteLine();
                            return; // Exit the method after finding the matching profile
                        }
                    }
                }
            }

            Console.WriteLine("User not found.");
        }
    }
}
