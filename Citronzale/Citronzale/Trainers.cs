using System;
using System.IO;
using System.Text;

namespace Trainers
{
    class GymTrainers
    {

        public string ChooseTrainer()
        {
            string[] fields = File.ReadAllLines("user.txt");

            string trainer;
            string choice;

            Console.WriteLine("Choose your trainer (write number 1-4)");
            Console.WriteLine("======================================================================");
            PrintTrainers();
            Console.WriteLine("======================================================================");

            choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    trainer = "Antons Kalns";
                    fields = AddTrainerToFields(fields, trainer);
                    break;
                case "2":
                    trainer = "Grigorijs Mats";
                    fields = AddTrainerToFields(fields, trainer);
                    break;
                case "3":
                    trainer = "Ina Mina";
                    fields = AddTrainerToFields(fields, trainer);
                    break;
                case "4":
                    trainer = "Lana Sika";
                    fields = AddTrainerToFields(fields, trainer);
                    break;
                case "5":
                    trainer = "No trainer";
                    fields = AddTrainerToFields(fields, trainer);
                    break;
                default:
                    Console.WriteLine("Incorrect option. Please try again!");
                    return ChooseTrainer(); // Return the recursive call
            }

            // Write the updated data back to the file
            File.WriteAllLines("user.txt", fields);

            Console.WriteLine("Success!");

            return trainer; // Return the selected trainer
        }

        private string[] AddTrainerToFields(string[] fields, string trainer)
        {
            // Get the index of the last line (current user's data)
            int userIndex = fields.Length - 1;

            // Split the user's data into individual fields
            string[] userFields = fields[userIndex].Split(',');

            // Add the selected trainer to the last field of the user's data
            userFields = userFields.Take(userFields.Length - 1).ToArray(); // Remove the previous trainer if any
            userFields = userFields.Append(trainer).ToArray();

            // Combine the fields back into a single line
            fields[userIndex] = string.Join(",", userFields);

            return fields;
        }

        public void PrintTrainers()
        {
            var path = "trainers.txt";
            string content = File.ReadAllText(path, Encoding.UTF8);
            Console.WriteLine(content);
        }
    }
}
