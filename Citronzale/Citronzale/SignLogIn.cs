using System;
using System.Threading;


namespace LogSignIn
{
    class LogSign
    {
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

            Console.WriteLine("======================================================================");
            Console.WriteLine("1. Log In to your profile");
            Console.WriteLine("2. Sign Up and create a new profile");
            Console.WriteLine("3. Exit");
            Console.WriteLine("======================================================================");
            double input = double.Parse(Console.ReadLine());
            Console.Clear();

            switch (input)
            {
                case 1:
                    login.LogIn();
                    break;
                case 2:
                    signup.SignUp();
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
            Console.WriteLine("LogIn");
        }
        public void SignUp()
        {
            Console.WriteLine("SignUp");
        }
        public void Exit()
        {
            Environment.Exit(0);
        }
    }
}