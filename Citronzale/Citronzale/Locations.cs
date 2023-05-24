using System;
using LogSignIn;
using System;
using System.IO;

namespace Locations
{
    public class GymLocations
    {
        // Other class members and methods...

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
                    location = "Maskavas iela 64 Imanta";
                    fields = AddLocationToFields(fields, location);
                    ShowLocation(location);
                    break;
                case "2":
                    Console.WriteLine("2. Bumbieru iela 12, Pļavinieki");
                    location = "Bumbieru iela 12 Pļavinieki";
                    fields = AddLocationToFields(fields, location);
                    ShowLocation(location);
                    break;
                case "3":
                    Console.WriteLine("3. Maiznieku iela 2, Bolderāja");
                    location = "Maiznieku iela 2 Bolderāja";
                    fields = AddLocationToFields(fields, location);
                    ShowLocation(location);
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
            for (int i = 0; i < fields.Length; i++)
            {
                // Append the gym location to each line
                fields[i] += "," + location;
            }

            return fields;
        }

        private void ShowLocation(string location)
        {
            Console.WriteLine("Location: " + location);
        }
    }
}