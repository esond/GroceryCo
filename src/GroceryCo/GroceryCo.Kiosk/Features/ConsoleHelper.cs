using System;
using System.Collections.Generic;
using System.Linq;

namespace GroceryCo.Kiosk.Features
{
    public static class ConsoleHelper
    {
        public static T SelectFrom<T>(IEnumerable<T> options, string prompt = null)
        {
            T[] items = options.ToArray();

            if (!items.Any())
                throw new ArgumentException("options cannot be emtpty", nameof(options));

            while (true)
            {
                Console.WriteLine(prompt ?? $"Select a {typeof(T).Name}...");

                for (int i = 1; i <= items.Length; i++)
                {
                    Console.WriteLine($"\t[{i}] {items[i - 1]}");
                }

                Console.Write("Selection:");

                string input = Console.ReadLine();

                int selection;

                if (int.TryParse(input, out selection) && (selection < items.Length))
                    return items[selection - 1];

                Console.WriteLine($"Selection {input} is not valid.");
            }
        }
    }
}
