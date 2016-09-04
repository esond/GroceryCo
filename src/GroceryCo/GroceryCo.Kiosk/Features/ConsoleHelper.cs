using System;

namespace GroceryCo.Kiosk.Features
{
    public static class ConsoleHelper
    {
        public static int SelectFromStringArray(string[] items)
        {
            for (int i = 1; i <= items.Length; i++)
            {
                Console.WriteLine($"\t[{i}] {items[i - 1]}");
            }

            Console.Write("Selection:");

            return int.Parse(Console.ReadLine());
        }
    }
}
