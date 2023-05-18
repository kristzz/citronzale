using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Citronzale.Admin
{
    public class AdminFeatures
    {
        public void AdminCommands(StreamWriter file)
        {
            LogSignIn.LogSign options = new LogSignIn.LogSign();

            if (file == null)
            {
                string filePath = @"C:\Users\emils\source\repos\citronzale\Citronzale\Citronzale\bin\Debug\net6.0\user.txt";
                file = new StreamWriter(filePath, true);
            }

            Console.WriteLine("======================================================================");
            Console.WriteLine("1. Print out the users txt file");
            Console.WriteLine("2. nothing yet");
            Console.WriteLine("3. Log out of admin");
            Console.WriteLine("======================================================================");
            string choice = Console.ReadLine();
            Console.Clear();

            switch (choice)
            {
                case "1":
                    PrintFile("user.txt", "User", file);
                    file.Close();
                    break;
                case "2":
                    break;
                case "3":
                    Console.Clear();
                    options.Options();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    Thread.Sleep(1500); // 1.5s sleep
                    Console.Clear();
                    AdminCommands(file);
                    break;
            }
        }

        public static void PrintFile(string filePath, string fileType, StreamWriter file)
        {
            Console.WriteLine($"Printing {fileType} contents of {filePath}:");
            Console.Write("Do you want to sort the data? (Y/N): ");
            string sortChoice = Console.ReadLine();

            List<string> fileData = ReadFileContents(filePath);

            if (sortChoice.Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Sort options:");
                Console.WriteLine("1. Sort by name");
                Console.WriteLine("2. Sort by surname");
                Console.WriteLine("3. Sort by gym membership");
                Console.Write("Choose a sort option (1-3): ");
                string sortOption = Console.ReadLine();

                switch (sortOption)
                {
                    case "1":
                        fileData.Sort((line1, line2) =>
                        {
                            string[] fields1 = line1.Split(',');
                            string[] fields2 = line2.Split(',');

                            string name1 = fields1[0]; // Extract the name field from line1
                            string name2 = fields2[0]; // Extract the name field from line2

                            return string.Compare(name1, name2, StringComparison.OrdinalIgnoreCase);
                        });
                        break;
                    case "2":
                        fileData.Sort((line1, line2) =>
                        {
                            string[] fields1 = line1.Split(',');
                            string[] fields2 = line2.Split(',');

                            string surname1 = fields1[1]; // Extract the surname field from line1
                            string surname2 = fields2[1]; // Extract the surname field from line2

                            return string.Compare(surname1, surname2, StringComparison.OrdinalIgnoreCase);
                        });
                        break;
                    case "3":
                        fileData.Sort((line1, line2) =>
                        {
                            string[] fields1 = line1.Split(',');
                            string[] fields2 = line2.Split(',');

                            string membership1 = fields1[4]; // Extract the membership field from line1
                            string membership2 = fields2[4]; // Extract the membership field from line2

                            return string.Compare(membership1, membership2, StringComparison.OrdinalIgnoreCase);
                        });
                        break;
                    default:
                        Console.WriteLine("Invalid sort option. Skipping sorting.");
                        break;
                }

                Console.WriteLine("Sorted Data:");
            }
            else
            {
                Console.WriteLine("Original Data:");
            }

            foreach (string line in fileData)
            {
                Console.WriteLine(line);
            }

            Console.WriteLine();

            bool fileLocked = true;
            int retryCount = 0;
            while (fileLocked && retryCount < 10) // Retry for a maximum of 10 times
            {
                try
                {
                    // Write the sorted data to a temporary file
                    string tempFilePath = Path.Combine(Path.GetDirectoryName(filePath), "temp.txt");
                    using (StreamWriter writer = new StreamWriter(tempFilePath, false))
                    {
                        foreach (string line in fileData)
                        {
                            writer.WriteLine(line);
                        }
                    }

                    // Delete the original file
                    File.Delete(filePath);

                    // Rename the temporary file to the original file name
                    File.Move(tempFilePath, filePath);

                    fileLocked = false; // If the file operations succeed, set fileLocked to false to exit the loop
                }
                catch (IOException)
                {
                    retryCount++;
                    Thread.Sleep(1000); // Wait for 1 second before retrying
                }
            }

            if (fileLocked)
            {
                var optionsaftersort = new AdminFeatures();
                optionsaftersort.OptionsAfterSort(file);
            }
        }




        private static List<string> ReadFileContents(string filePath)
        {
            List<string> lines = new List<string>();

            if (File.Exists(filePath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader sr = new StreamReader(fs))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }
            }

            return lines;
        }

        public void OptionsAfterSort(StreamWriter file)
        {
            Console.WriteLine("Choose what to do next:");
            Console.WriteLine("1. Sort again but differently");
            Console.WriteLine("2. Go back to the admin menu");

            string OptionAfterSort = Console.ReadLine();
            switch (OptionAfterSort)
            {
                case "1":
                    Console.Clear();
                    PrintFile("user.txt", "User", file);
                    file.Close();
                    break;
                case "2":
                    file.Close(); // Close the file
                    Console.Clear();
                    AdminCommands(file);
                    break;
            }
        }
    }
}