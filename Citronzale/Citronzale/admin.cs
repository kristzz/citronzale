using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Citronzale.Admin
{
    public class AdminFeatures
    {
        private static string lastSortOption;

        public void AdminCommands(StreamWriter file)
        {
            LogSignIn.LogSign options = new LogSignIn.LogSign();

            if (file == null)
            {
                string filePath = "user.txt";
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

                // Update the last sort option
                lastSortOption = sortOption;

                SortData(fileData, sortOption);

                Console.WriteLine("Sorted Data:");
            }
            else
            {
                Console.WriteLine("Original Data:");
            }

            PrintTable(fileData);

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
                    Thread.Sleep(100); // Wait for 0.1 second before retrying
                }
            }

            if (fileLocked)
            {
                var optionsAfterSort = new AdminFeatures();
                optionsAfterSort.OptionsAfterSort(file);
            }
        }

        public static void PrintTable(List<string> fileData)
        {
            const int tableWidth = 90;
            const char separator = '-';
            const char corner = '-';

            Console.WriteLine(new string(corner, tableWidth));
            Console.WriteLine($"| {"Name",-20} | {"Surname",-20} | {"Username",-20} | {"Membership",-20} |");
            Console.WriteLine(new string(corner, tableWidth));

            foreach (string line in fileData)
            {
                string[] fields = line.Split(',');

                string name = fields[0];
                string surname = fields[1];
                string username = fields[2];
                string membership = fields[4];

                Console.WriteLine($"| {name,-20} | {surname,-20} | {username,-20} | {membership,-20} |");
                Console.WriteLine(new string(separator, tableWidth));
            }

            Console.WriteLine();

            ShowUserCount(fileData);
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
            Console.WriteLine("2. Find something specific");
            Console.WriteLine("3. Go back to the admin menu");

            string optionAfterSort = Console.ReadLine();
            switch (optionAfterSort)
            {
                case "1":
                    Console.Clear();
                    PrintFile("user.txt", "User", file);
                    file.Close();
                    break;
                case "2":
                    Console.Clear();
                    SearchAndPrintTable(ReadFileContents("user.txt"), file);
                    break;
                case "3":
                    file.Close(); // Close the file
                    Console.Clear();
                    AdminCommands(file);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    Thread.Sleep(1500); // 1.5s sleep
                    Console.Clear();
                    OptionsAfterSort(file);
                    break;
            }
        }

        public static void SearchAndPrintTable(List<string> fileData, StreamWriter file)
        {
            const int tableWidth = 90;
            const char separator = '-';
            const char corner = '-';
            const ConsoleColor searchHighlightColor = ConsoleColor.Yellow;

            Console.WriteLine("Enter a search term:");
            string searchTerm = Console.ReadLine()?.Trim();

            Console.WriteLine(new string(corner, tableWidth));
            Console.WriteLine($"| {"Name",-20} | {"Surname",-20} | {"Username",-20} | {"Membership",-20} |");
            Console.WriteLine(new string(corner, tableWidth));

            // Sort the data based on the last chosen sort option
            SortData(fileData, lastSortOption);

            foreach (string line in fileData)
            {
                string[] fields = line.Split(',');

                string name = fields[0];
                string surname = fields[1];
                string username = fields[2];
                string membership = fields[4];

                bool matchFound = name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                  surname.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                  username.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                  membership.Contains(searchTerm, StringComparison.OrdinalIgnoreCase);

                Console.ForegroundColor = matchFound ? searchHighlightColor : Console.ForegroundColor;
                Console.WriteLine($"| {name,-20} | {surname,-20} | {username,-20} | {membership,-20} |");
                Console.ResetColor();
                Console.WriteLine(new string(separator, tableWidth));
            }

            Console.WriteLine();

            OptionsAfterSearch(file);
        }

        public static void OptionsAfterSearch(StreamWriter file)
        {
            Console.WriteLine("Choose what to do next:");
            Console.WriteLine("1. Search again");
            Console.WriteLine("2. Sort the table");
            Console.WriteLine("3. Go back to the admin menu");

            string optionAfterSearch = Console.ReadLine();
            switch (optionAfterSearch)
            {
                case "1":
                    Console.Clear();
                    SearchAndPrintTable(ReadFileContents("user.txt"), file);
                    break;
                case "2":
                    Console.Clear();
                    PrintFile("user.txt", "User", file);
                    break;
                case "3":
                    file.Close(); // Close the file
                    Console.Clear();
                    var adminCommands = new AdminFeatures();
                    adminCommands.AdminCommands(file);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    Thread.Sleep(1500); // 1.5s sleep
                    Console.Clear();
                    OptionsAfterSearch(file);
                    break;
            }
        }

        public static void SortData(List<string> fileData, string sortOption)
        {
            switch (sortOption)
            {
                case "1":
                    fileData.Sort((a, b) =>
                    {
                        string[] fieldsA = a.Split(',');
                        string[] fieldsB = b.Split(',');
                        return string.Compare(fieldsA[0], fieldsB[0], StringComparison.OrdinalIgnoreCase);
                    });
                    break;
                case "2":
                    fileData.Sort((a, b) =>
                    {
                        string[] fieldsA = a.Split(',');
                        string[] fieldsB = b.Split(',');
                        return string.Compare(fieldsA[1], fieldsB[1], StringComparison.OrdinalIgnoreCase);
                    });
                    break;
                case "3":
                    fileData.Sort((a, b) =>
                    {
                        string[] fieldsA = a.Split(',');
                        string[] fieldsB = b.Split(',');
                        return string.Compare(fieldsA[4], fieldsB[4], StringComparison.OrdinalIgnoreCase);
                    });
                    break;
                default:
                    Console.WriteLine("Invalid sort option. Defaulting to sort by name.");
                    fileData.Sort((a, b) =>
                    {
                        string[] fieldsA = a.Split(',');
                        string[] fieldsB = b.Split(',');
                        return string.Compare(fieldsA[0], fieldsB[0], StringComparison.OrdinalIgnoreCase);
                    });
                    break;
            }
        }

        public static void ShowUserCount(List<string> fileData)
        {
            int allMemberCount = 0;
            int regularMemberCount = 0;
            int flexMemberCount = 0;
            int superMembersCount = 0;
            int deluxeMembersCount = 0;

            foreach (string line in fileData)
            {
                string[] fields = line.Split(',');

                string membershipOption = fields[4].Trim();

                switch (membershipOption)
                {
                    case "Regular":
                        regularMemberCount++;
                        break;
                    case "Flex":
                        flexMemberCount++;
                        break;
                    case "Super":
                        superMembersCount++;
                        break;
                    case "Deluxe":
                        deluxeMembersCount++;
                        break;
                    default:
                        break;
                }
            }

            allMemberCount = regularMemberCount + flexMemberCount + superMembersCount + deluxeMembersCount;

            Console.WriteLine("Users using regular membership: " + regularMemberCount);
            Console.WriteLine("Users using flex membership: " + flexMemberCount);
            Console.WriteLine("Users using super membership: " + superMembersCount);
            Console.WriteLine("Users using deluxe membership: " + deluxeMembersCount);
            Console.WriteLine("Total number of users: " + allMemberCount);
        }
    }
}
