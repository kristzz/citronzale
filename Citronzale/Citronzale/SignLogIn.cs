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
        static List<string> lines;
        static string creationPassword = "";
        static string filepath = "pass.txt";

        public string password;

        public static void Main(string[] args)
        {
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

            if (!File.Exists(filepath))
                File.Create(filepath);
            lines = new List<string>(File.ReadAllLines(filepath));

            Console.WriteLine("======================================================================");
            Console.WriteLine("1. Sign Up and create a new profile");
            Console.WriteLine("2. Log In to your profile");
            Console.WriteLine("3. Exit");
            Console.WriteLine("======================================================================");
            double input = double.Parse(Console.ReadLine());
            Console.Clear();

            switch (input)
            {
                case 1:
                    signup.SignUp();
                    break;
                case 2:
                    login.LogIn();
                    break;
                case 3:
                    exit.Exit();
                    break;
                default:
                    Console.WriteLine("Incorrect input, try again");
                    Thread.Sleep(1500); // 1.5s sleep
                    Console.Clear();
                    options.Options();
                    break;
            }
        }

        public void LogIn()
        {
            var options = new LogSign();
            var userProfile = new PersonalProfile();

            Console.WriteLine("\nEnter Your Password");
            password = Console.ReadLine();
            if (userPass(password))
            {
                Console.WriteLine("You've successfully logged in.");
                Console.WriteLine("Sveiki " + password);
                userProfile.userInfo();
            }
            else
            {
                Console.WriteLine("Password not found");
                Thread.Sleep(1500); // 1.5s sleep
                Console.Clear();
                options.Options();
            }  
        }

        static Boolean userPass(string psw)
        {
            foreach (var item in lines)
            {
                if (item == psw)
                    return true;
            }
            return false; //japievieno lai parbauda ne tikai paroli bet ari username
        }

        public void SignUp()
        {
            var options = new LogSign();

            Console.WriteLine("Create your password:"); //japievieno lai var ari uztaisit username
            creationPassword = Console.ReadLine();
            lines.Add(creationPassword);
            File.WriteAllLines(filepath, lines);
            Console.WriteLine("You've successfully Signed Up.");
            Thread.Sleep(1500); // 1.5s sleep
            Console.Clear();
            options.Options();
        }
        public void Exit()
        {
            Environment.Exit(0);
        }
    }
}