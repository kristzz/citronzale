using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Profile;

namespace LogSignIn
{
    class LogSign
    {
        static Dictionary<string, string> userCredentials = new Dictionary<string, string>();
        const string fileName = "user.txt";
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
            Console.Write("Enter your username: ");
            string username = Console.ReadLine();
            Console.Write("Enter your password: ");
            string password = Console.ReadLine();

            if (userCredentials.ContainsKey(username) && userCredentials[username] == password)
            {
                Console.Clear();
                Console.WriteLine("Login successful!");
                Thread.Sleep(2000); // sleep 2 sek
                Console.Clear();
                LoggedInOptions();

                void LoggedInOptions()
                {
                    Console.WriteLine("1. View profile");
                    Console.WriteLine("2. Log out");
                    Console.WriteLine("3. Exit");

                    string choice = Console.ReadLine();

                    string targetUsername = username;

                    bool stopProgram = false;

                    while (!stopProgram) {

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

                            case "3":
                                stopProgram = true;
                                break;

                            default:
                                Console.WriteLine("Invalid choice! Please try again.");
                                Thread.Sleep(1500);
                                Console.Clear();
                                LoggedInOptions();

                                break;



                        }

                    }

                }
            }


            else
            {
                Console.Clear();
                Console.WriteLine("Invalid username or password. Please try again.");
                Thread.Sleep(1500); // sleep 1,5 sek
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
            string password = Console.ReadLine();

            if (userCredentials.ContainsKey(username))
            {
                Console.WriteLine("Username already exists. Please choose a different username.");
                Thread.Sleep(1500); //1.5s sleep
                Console.Clear();
                SignUp();
                return;
            }

            string membership = GetMembershipOption();

            // Save user information in the dictionary
            userCredentials[username] = password;

            // Save user information in a text file
            string userData = $"{name},{surname},{username},{password},{membership}";
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
    }
}