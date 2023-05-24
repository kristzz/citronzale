using System;
using LogSignIn;
using System;
using System.IO;

namespace Locations
{
    public class GymLocations
    {
        public string ChooseLocation()
        {
            string[] fields = File.ReadAllLines("user.txt");

            string location;
            string choice;

            Console.WriteLine("Choose your location (write region name)");
            Console.WriteLine("======================================================================");
            Console.WriteLine("1. Maskavas iela 64, Imanta");
            Console.WriteLine("2. Bumbieru iela 12, Pļavinieki");
            Console.WriteLine("3. Maiznieku iela 2, Bolderāja");
            Console.WriteLine("======================================================================");

            choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("1. Maskavas iela 64, Imanta");
                    location = "Maskavas iela 64. Imanta";
                    fields = AddLocationToFields(fields, location);
                    DisplayLocation(location);
                    break;
                case "2":
                    Console.WriteLine("2. Bumbieru iela 12, Pļavinieki");
                    location = "Bumbieru iela 12. Pļavinieki";
                    fields = AddLocationToFields(fields, location);
                    DisplayLocation(location);
                    break;
                case "3":
                    Console.WriteLine("3. Maiznieku iela 2, Bolderāja");
                    location = "Maiznieku iela 2. Bolderāja";
                    fields = AddLocationToFields(fields, location);
                    DisplayLocation(location);
                    break;
                default:
                    Console.WriteLine("Incorrect option. Please try again!");
                    return ChooseLocation(); // Return the recursive call
            }

            // Write the updated data back to the file
            File.WriteAllLines("user.txt", fields);

            Console.WriteLine("Gym location added successfully.");

            return location; // Return the selected location
        }

        private string[] AddLocationToFields(string[] fields, string location)
        {
            // Get the index of the last line (current user's data)
            int userIndex = fields.Length - 1;

            // Split the user's data into individual fields
            string[] userFields = fields[userIndex].Split(',');

            // Add the selected location to the last field of the user's data
            userFields = userFields.Take(userFields.Length - 1).ToArray(); // Remove the previous location if any
            userFields = userFields.Append(location).ToArray();

            // Combine the fields back into a single line
            fields[userIndex] = string.Join(",", userFields);

            return fields;
        }

        private void DisplayLocation(string location)
        {
            Console.WriteLine("Location: " + location);
        }
    }
}