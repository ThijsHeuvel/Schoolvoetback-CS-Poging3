using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDB
{
    internal class Styling
    {
        private static ConsoleColor LineColor = ConsoleColor.Blue;

        private static void SetColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        private static void ResetColor()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void SkipLine()
        {
            Console.WriteLine("");
        }

        public static void AddLine()
        {
            SetColor(LineColor);
            Console.WriteLine("========================================");
            ResetColor();
        }
    }
}
