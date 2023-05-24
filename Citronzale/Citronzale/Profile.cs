using LogSignIn;
using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;

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
                        string location = fields[5];

                        if (username == targetUsername)
                        {
                            Console.WriteLine("======================================================================");

                            Console.WriteLine("Name: " + name);
                            Console.WriteLine("Last Name: " + lastName);
                            Console.WriteLine("Username: " + username);
                            Console.WriteLine("Subscription type: " + additionalDetails);
                            Console.WriteLine("Location: " + location);

                            Console.WriteLine("======================================================================");
                            Console.WriteLine();
                            return; // Exit the method after finding the matching profile
                        }
                    }
                }
            }

            Console.WriteLine("User not found.");
        }
        private static string newLocation;
        public static void EditUserProfile(string filePath, string targetUsername)
        {
            var signedInoptions = new LogSign();
            
            // Read all the lines from the file
            string[] lines = File.ReadAllLines(filePath);

            // Iterate over each line and find the target user
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] fields = line.Split(',');

                string username = fields[2];

                if (username == targetUsername)
                {
                    Console.WriteLine("What do you want to edit/change?");
                    Console.WriteLine("1. Password");
                    Console.WriteLine("2. Gym location");
                    Console.WriteLine("3. Membership");
                    Console.WriteLine("4. Exit");

                    string editChoice = Console.ReadLine();

                    switch (editChoice)
                    {
                        case "1":
                            Console.WriteLine("Enter a new password: ");
                            string newPassword = ReadPassword();
                            string hashedPassword = HashPassword(newPassword);
                            fields[3] = hashedPassword;
                            Console.WriteLine("Password updated successfully.");
                            break;

                        case "2":
                            Console.WriteLine("Enter a new gym location: (choose from 1-3 with numbers");
                            Console.WriteLine("======================================================================");
                            Console.WriteLine("1. Maskavas iela 64, Imanta");
                            Console.WriteLine("2. Bumbieru iela 12, P?avinieki");
                            Console.WriteLine("3. Maiznieku iela 2, Bolder?ja");
                            Console.WriteLine("======================================================================");
                            string temp = Console.ReadLine();
                            
                            switch (temp)
                            {
                                case "1":
                                    newLocation = "Maskavas iela 64. Imanta";
                                    break;
                                case "2":
                                    newLocation = "Bumbieru iela 12. Plavinieki";
                                    break;
                                case "3":
                                    newLocation = "Maiznieku iela 2. Bolderaja";
                                    break;
                            }
                            fields[5] = newLocation;
                            Console.WriteLine("Gym location updated successfully.");
                            break;

                        case "3":
                            Console.WriteLine("Enter the new membership you want to switch to (choose from 1-4 with numbers");
                            string newMembership = Console.ReadLine();
                            break;

                        case "4":
                            Console.Clear();
                            signedInoptions.LoggedInOptions(targetUsername);
                            return;

                        default:
                            Console.WriteLine("Invalid choice.");
                            return;
                    }

                    // Update the modified line in the lines array
                    lines[i] = string.Join(",", fields);
                    break;
                }
            }

            // Write the updated lines back to the file
            File.WriteAllLines(filePath, lines);
        }


        private static string ReadPassword()
        {
            StringBuilder password = new StringBuilder();
            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    Console.Write("\b \b");
                    password.Length--;
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    password.Append(keyInfo.KeyChar);
                }
            } while (keyInfo.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password.ToString();
        }

        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
