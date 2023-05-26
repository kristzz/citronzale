using LogSignIn;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Trainers;

namespace Profile
{
    public class UserInfoReader
    {
        public static void ReadUserInfo(string filePath, string targetUsername)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(fileStream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] fields = line.Split(',');

                        if (fields.Length >= 7)
                        {
                            string username = fields[2];

                            if (username == targetUsername)
                            {
                                Console.WriteLine("======================================================================");
                                Console.WriteLine("Name: " + fields[0]);
                                Console.WriteLine("Last Name: " + fields[1]);
                                Console.WriteLine("Username: " + fields[2]);
                                Console.WriteLine("Subscription type: " + fields[4]);
                                Console.WriteLine("Location: " + fields[5]);
                                Console.WriteLine("Trainer: " + fields[6]);
                                Console.WriteLine("======================================================================\n");
                                return;
                            }
                        }
                    }
                }

                Console.WriteLine("User not found.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("User file not found.");
            }
            catch (IOException)
            {
                Console.WriteLine("Error reading user file.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Access to user file denied.");
            }
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
        public static void EditUserProfile(string filePath, string targetUsername)
        {
            var signedInoptions = new LogSign();
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string[] fields = line.Split(',');

                    if (fields.Length >= 7 && fields[2] == targetUsername)
                    {
                        Console.WriteLine("What do you want to edit/change?");
                        Console.WriteLine("======================================================================");
                        Console.WriteLine("1. Password");
                        Console.WriteLine("2. Gym location");
                        Console.WriteLine("3. Membership");
                        Console.WriteLine("4. Change Trainer");
                        Console.WriteLine("5. Delete the account");
                        Console.WriteLine("6. Return back");
                        Console.WriteLine("======================================================================");

                        string editChoice = Console.ReadLine();

                        switch (editChoice)
                        {
                            case "1":
                                Console.Clear();
                                Console.WriteLine("Enter a new password: ");
                                string newPassword = ReadPassword();
                                string hashedPassword = HashPassword(newPassword);
                                fields[3] = hashedPassword;
                                Console.WriteLine("Password updated successfully.");
                                Console.WriteLine("Your new password will be active after you reopen our app");
                                Thread.Sleep(2000);
                                break;

                            case "2":
                                ChangeLocation(fields);
                                break;

                            case "3":
                                ChangeMembership(fields);
                                break;

                            case "4":
                                ChangeTrainer(fields);
                                break;

                            case "5":
                                Console.Clear();
                                DeleteUserAccount(filePath, targetUsername);
                                return;
                            case "6":
                                Console.Clear();
                                signedInoptions.LoggedInOptions(targetUsername);
                                break;

                            default:
                                Console.WriteLine("Invalid choice.");
                                break;
                        }

                        lines[i] = string.Join(",", fields);
                        break;
                    }
                }

                File.WriteAllLines(filePath, lines);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("User file not found.");
            }
            catch (IOException)
            {
                Console.WriteLine("Error reading or writing user file.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Access to user file denied.");
            }
        }

        private static void ChangeLocation(string[] fields)
        {
            Console.Clear();
            Console.WriteLine("Enter a new gym location (choose from 1-3):");
            Console.WriteLine("======================================================================");
            Console.WriteLine("1. Maskavas iela 64. Imanta");
            Console.WriteLine("2. Bumbieru iela 12. Plavinieki");
            Console.WriteLine("3. Maiznieku iela 2. Bolderaja");
            Console.WriteLine("======================================================================");
            string temp = Console.ReadLine();

            switch (temp)
            {
                case "1":
                    fields[5] = "Maskavas iela 64. Imanta";
                    Console.WriteLine("Gym location updated successfully.");
                    Thread.Sleep(1000);
                    break;
                case "2":
                    fields[5] = "Bumbieru iela 12. Plavinieki";
                    Console.WriteLine("Gym location updated successfully.");
                    Thread.Sleep(1000);
                    break;
                case "3":
                    fields[5] = "Maiznieku iela 2. Bolderaja";
                    Console.WriteLine("Gym location updated successfully.");
                    Thread.Sleep(1000);
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        private static void ChangeTrainer(string[] fields)
        {
            Console.Clear();
            Console.WriteLine("Enter a new trainer (choose from 1-4):");
            Console.WriteLine("======================================================================");
            Console.WriteLine("1. Antons Kalns");
            Console.WriteLine("2. Grigorijs Mats");
            Console.WriteLine("3. Ina Mina");
            Console.WriteLine("4. Lana Sika");
            Console.WriteLine("======================================================================");
            string temp = Console.ReadLine();

            switch (temp)
            {
                case "1":
                    fields[6] = "Antons Kalns";
                    Console.WriteLine("Trainer updated successfully.");
                    Thread.Sleep(1000);
                    break;
                case "2":
                    fields[6] = "Grigorijs Mats";
                    Console.WriteLine("Trainer updated successfully.");
                    Thread.Sleep(1000);
                    break;
                case "3":
                    fields[6] = "Ina Mina";
                    Console.WriteLine("Trainer updated successfully.");
                    Thread.Sleep(1000);
                    break;
                case "4":
                    fields[6] = "Lana Sika";
                    Console.WriteLine("Trainer updated successfully.");
                    Thread.Sleep(1000);
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        private static void ChangeMembership(string[] fields)
        {
            Console.Clear();
            Console.WriteLine("Enter the new membership you want to switch to (choose from 1-4):");
            Console.WriteLine("======================================================================");
            Console.WriteLine("1. Regular");
            Console.WriteLine("2. Flex");
            Console.WriteLine("3. Super");
            Console.WriteLine("4. Deluxe");
            Console.WriteLine("======================================================================");
            string temp = Console.ReadLine();

            switch (temp)
            {
                case "1":
                    fields[4] = "Regular";
                    Console.WriteLine("Membership type updated successfully.");
                    Thread.Sleep(1000);
                    break;
                case "2":
                    fields[4] = "Flex";
                    Console.WriteLine("Membership type updated successfully.");
                    Thread.Sleep(1000);
                    break;
                case "3":
                    fields[4] = "Super";
                    Console.WriteLine("Membership type updated successfully.");
                    Thread.Sleep(1000);
                    break;
                case "4":
                    fields[4] = "Deluxe";
                    Console.WriteLine("Membership type updated successfully.");
                    Thread.Sleep(1000);
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        public static void DeleteUserAccount(string filePath, string targetUsername)
        {
            var signedInoptions = new LogSign();
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string[] fields = line.Split(',');

                    if (fields.Length >= 7 && fields[2] == targetUsername)
                    {
                        Console.WriteLine("Are you sure you want to delete your account? (Y/N)");
                        string choice = Console.ReadLine();

                        if (choice.ToLower() == "y")
                        {
                            lines[i] = string.Empty; // Clear the line by setting it to an empty string
                            Console.WriteLine("Account deleted successfully.");
                            Thread.Sleep(2000);
                        }
                        else
                        {
                            Console.WriteLine("Account deletion cancelled.");
                            Thread.Sleep(2000);
                        }

                        // Remove empty lines from the array
                        lines = lines.Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

                        File.WriteAllLines(filePath, lines);
                        Console.Clear();
                        signedInoptions.LoggedInOptions(targetUsername);
                        return;
                    }
                }

                Console.WriteLine("User not found.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("User file not found.");
            }
            catch (IOException)
            {
                Console.WriteLine("Error reading or writing user file.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Access to user file denied.");
            }
        }
    }
}