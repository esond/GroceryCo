using System;
using System.Configuration;
using GroceryCo.Kiosk.Features;
using GroceryCo.Repository;

namespace GroceryCo.Kiosk
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IRepository repository = new LocalFileEntityRepository(ConfigurationManager.AppSettings["RepositoryFolder"]);

            bool quit = false;

            while (!quit)
            {
                Console.Write("Select application mode ([A]dmin, [K]iosk) or [Q]uit:");

                string mode = Console.ReadLine()?.ToLowerInvariant();

                switch (mode)
                {
                    case "a":
                        Console.WriteLine("Entering admin mode...");
                        AdministrationConsole admin = new AdministrationConsole(repository);
                        admin.Run();
                        break;

                    case "k":
                        Console.WriteLine("Entering Kiosk mode...");
                        break;

                    case "q":
                        quit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid selection.");
                        break;
                }
            }
        }
    }
}
