using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Profile;
using Citronzale.Admin;
using Citronzale;
using Locations;

namespace LogSignIn
{
    class LogSign
    {
        static Dictionary<string, string> userCredentials = new Dictionary<string, string>();
        const string fileName = "user.txt";

        private static string filePath = "user.txt";
        private static StreamWriter file;

        public static void Main(string[] args)
        {
            var save = new LogSign();
            save.LoadUserCredentials();
            var starter = new LogSign();
            starter.Starter();
        }

        public void Starter()
        {
            var options = new LogSign();

            Console.WriteLine("======================================================================");
            Console.WriteLine("Welcome to Citronzale Gym!");
            Console.WriteLine("We offer many great gym locations and trainers to train with!");
            Console.WriteLine("Choose what you want to do:");

            options.Options();
        }

        public void Options()
        {
            var login = new LogSign();
            var signup = new LogSign();
            var exit = new LogSign();
            var options = new LogSign();

            AdminFeatures admin = new AdminFeatures();

            Console.WriteLine("======================================================================");
            Console.WriteLine("1. Sign Up and create a new profile");
            Console.WriteLine("2. Log In to your profile");
            Console.WriteLine("3. Exit");
            Console.WriteLine("======================================================================");
            string choice = Console.ReadLine();
            Console.Clear();

            switch (choice)
            {
                case "1":
                    SignUp();
                    break;
                case "2":
                    LogIn();
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    Thread.Sleep(1500); // 1.5s sleep
                    Console.Clear();
                    Options();
                    break;
            }
        }

        public void LogIn()
        {
            AdminFeatures admin = new AdminFeatures();

            Console.Write("Enter your username: ");
            string username = Console.ReadLine();
            Console.Write("Enter your password: ");
            string password = ReadPassword();

            if (userCredentials.ContainsKey(username) && ValidatePassword(password, userCredentials[username]))
            {
                Console.Clear();
                Console.WriteLine("Login successful!");
                Thread.Sleep(2000); // sleep 2 seconds
                Console.Clear();
                LoggedInOptions();

                void LoggedInOptions()
                {
                    Console.WriteLine("======================================================================");
                    Console.WriteLine("1. View profile");
                    Console.WriteLine("2. Log out");
                    Console.WriteLine("======================================================================");

                    string choice = Console.ReadLine();

                    string targetUsername = username;

                    bool stopProgram = false;

                    while (!stopProgram)
                    {

                        switch (choice)
                        {
                            case "1":
                                Console.Clear();
                                UserInfoReader.ReadUserInfo(fileName, targetUsername);
                                Console.WriteLine();
                                Console.WriteLine("Press any key to return to the selection...");
                                Console.ReadKey();
                                Console.Clear();
                                LoggedInOptions();
                                return;

                            case "2":
                                Console.Clear();
                                Options();
                                break;

                            default:
                                Console.Clear();
                                Console.WriteLine("Invalid choice! Please try again.");
                                Thread.Sleep(1500);
                                Console.Clear();
                                LoggedInOptions();
                                break;
                        }
                    }
                }
            }
            else if (username == "admin" && password == "admin")
            {
                Console.Clear();
                using (var file = new StreamWriter(filePath, true))
                {
                    AdminFeatures adminfeatures = new AdminFeatures();
                    adminfeatures.AdminCommands(file); // Pass the file as an argument to the AdminCommands method
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Invalid username or password. Please try again.");
                Thread.Sleep(1500); // sleep 1.5 seconds
                Console.Clear();
                LogIn();
            }
        }

        public void SignUp()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.Write("Enter your surname: ");
            string surname = Console.ReadLine();
            Console.Write("Enter your username: ");
            string username = Console.ReadLine();
            Console.Write("Enter your password: ");
            string password = ReadPassword();
            GymLocations gymLocations = new GymLocations();
            string location = gymLocations.ChooseLocation();

            if (userCredentials.ContainsKey(username))
            {
                Console.WriteLine("Username already exists. Please choose a different username.");
                Thread.Sleep(1500); //1.5s sleep
                Console.Clear();
                SignUp();
                return;
            }

            string membership = GetMembershipOption();

            // Hash the password
            string hashedPassword = HashPassword(password);

            // Save user information in the dictionary
            userCredentials[username] = hashedPassword;

            // Save user information in a text file
            string userData = $"{name},{surname},{username},{hashedPassword},{membership}, {location}";
            SaveUserData(userData);

            Console.WriteLine("Sign up successful! You can now log in!");
            Thread.Sleep(1500); //1.5s sleep
            Console.Clear();
            Options();
        }

        static string GetMembershipOption()
        {
            Console.WriteLine("Membership Options:");
            Console.WriteLine("1. Regular");
            Console.WriteLine("2. Flex");
            Console.WriteLine("3. Super");
            Console.WriteLine("4. Deluxe");
            Console.Write("Choose your membership option (1-4): ");
            string membershipOption = Console.ReadLine();

            // Validate membership option
            if (!int.TryParse(membershipOption, out int membershipIndex) || membershipIndex < 1 || membershipIndex > 4)
            {
                Console.WriteLine("Invalid membership option. Please try again.");
                return GetMembershipOption();
            }

            return GetMembershipByIndex(membershipIndex);
        }

        static string GetMembershipByIndex(int index)
        {
            switch (index)
            {
                case 1:
                    return "Regular";
                case 2:
                    return "Flex";
                case 3:
                    return "Super";
                case 4:
                    return "Deluxe";
                default:
                    return string.Empty;
            }
        }

        public void LoadUserCredentials()
        {
            if (File.Exists(fileName))
            {
                string[] lines = File.ReadAllLines(fileName);
                foreach (string line in lines)
                {
                    string[] userData = line.Split(',');
                    string username = userData[2];
                    string password = userData[3];
                    userCredentials[username] = password;
                }
            }
        }

        static void SaveUserData(string userData)
        {
            using (StreamWriter sw = File.AppendText(fileName))
            {
                sw.WriteLine(userData);
            }
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private static bool ValidatePassword(string password, string hashedPassword)
        {
            string inputHashedPassword = HashPassword(password);
            return inputHashedPassword == hashedPassword;
        }

        // Securely read password input with asterisks
        private static string ReadPassword()
        {
            StringBuilder password = new StringBuilder();
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password.Append(key.KeyChar);
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password.Remove(password.Length - 1, 1);
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password.ToString();
        }
    }
}
