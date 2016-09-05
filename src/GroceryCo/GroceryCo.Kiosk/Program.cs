using System;
using System.Configuration;
using GroceryCo.Kiosk.Features;
using GroceryCo.Kiosk.Features.Administration;
using GroceryCo.Kiosk.Features.Cashier;
using GroceryCo.Repository;

namespace GroceryCo.Kiosk
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            IRepository repository = new LocalFileEntityRepository(ConfigurationManager.AppSettings["RepositoryFolder"]);

            while (true)
            {
                Console.Write("Select application mode ([A]dmin, [C]ashier)");

                string mode = Console.ReadLine()?.ToLowerInvariant();

                switch (mode)
                {
                    case "a":
                        Console.WriteLine("Entering admin mode...");
                        AdministrationConsole admin = new AdministrationConsole(repository);
                        admin.Run();
                        break;

                    case "c":
                        Console.WriteLine("Entering Kiosk mode...");
                        CashierConsole cashierConsole = new CashierConsole(repository);
                        cashierConsole.Run();
                        break;

                    default:
                        Console.WriteLine("Invalid selection.");
                        break;
                }
            }
        }
    }
}
