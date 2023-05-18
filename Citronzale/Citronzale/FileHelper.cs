using System;
using System.IO;

namespace FileUtils
{
    public class FileHelper
    {
        public static void PrintUserData(string filePath)
        {
            PrintFileContents(filePath);
        }

        public static void PrintLocations(string filePath)
        {
            PrintFileContents(filePath);
        }

        public static void PrintTrainers(string filePath)
        {
            PrintFileContents(filePath);
        }

        public static void PrintFileContents(string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    Console.WriteLine(line);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"File not found: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            }
        }
    }
}