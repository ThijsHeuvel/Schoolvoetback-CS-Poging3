using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel;

namespace CarDB
{
    internal class Helpers
    {
        internal static string Ask(string question)
        {
            string? userInput = "";

            while((userInput is null || userInput == ""))
            {
                Console.WriteLine(question);
                userInput = Console.ReadLine();
            }

            return userInput;
        }

        internal static int AskForInt(string question)
        {
            int retVal;
            string userInput = "";

            while (!int.TryParse(userInput, out retVal))
            {
                userInput = Ask(question);
            }

            return retVal;
        }

        internal static void Pause()
        {
            Console.WriteLine("Press <ENTER> to continue...");
            Console.ReadLine();
        }

        internal static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
        }

        internal static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
        }
    }
}