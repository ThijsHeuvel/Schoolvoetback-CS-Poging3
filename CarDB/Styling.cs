using CarDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CarDB
{
    internal class Styling
    {
        private static ConsoleColor DefaultColor = ConsoleColor.White;
        private static ConsoleColor LineColor = ConsoleColor.DarkCyan;
        private static ConsoleColor HeaderColor = ConsoleColor.DarkYellow;
        private static ConsoleColor ListOptionColor = ConsoleColor.Cyan;
        private static ConsoleColor ErrorColor = ConsoleColor.Red;
        private static ConsoleColor InfoColor = ConsoleColor.Blue;
        private static ConsoleColor BalanceColor = ConsoleColor.Green;

        private static void SetColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        private static void ResetColor()
        {
            Console.ForegroundColor = DefaultColor;
        }

        public static void SkipLine()
        {
            Console.WriteLine("");
        }

        public static void AddLine()
        {
            SetColor(LineColor);
            Console.WriteLine("============================================================");
            ResetColor();
        }

        public static void AddHeader(string message)
        {
            SetColor(HeaderColor);
            Console.WriteLine(message);
            Console.WriteLine("------------------------------------------------------------");
            ResetColor();
            SkipLine();
        }

        public static void AddListOption(string text)
        {
            SetColor(ListOptionColor);
            Console.WriteLine(text);
            ResetColor();
        }

        public static void ShowError(string message)
        {
            SetColor(ErrorColor);
            Console.WriteLine(message);
            ResetColor();
        }

        public static void ShowInfo(string message)
        {
            SetColor(InfoColor);
            Console.WriteLine(message);
            ResetColor();
        }

        public static void ShowBalance(User user)
        {
            SetColor(BalanceColor);
            Console.WriteLine($"Balans: ${user.Dollars}");
            ResetColor();
        }
    }
}
