using System;
using GroceryCo.Repository;

namespace GroceryCo.Kiosk.Features.Administration
{
    public class AdministrationConsole
    {
        private readonly IRepository _repository;

        public AdministrationConsole(IRepository repository)
        {
            _repository = repository;
        }

        public void Run()
        {
            bool goBack = false;

            while (!goBack)
            {
                Console.WriteLine("Select an action...");
                Console.Write("[A]dd a new Promotion | [E]nd an existing Promotion | Go [b]ack:");

                string action = Console.ReadLine()?.ToLowerInvariant();

                switch (action)
                {
                    case "a":
                        AddPromotionConsole addPromotionConsole = new AddPromotionConsole(_repository);
                        addPromotionConsole.Run();
                        break;

                    case "e":
                        EndPromotion();
                        break;

                    case "b":
                        goBack = true;
                        break;

                    default:
                        Console.WriteLine("Invalid selection.");
                        break;
                }
            }
        }

        public void EndPromotion()
        {
            throw new NotImplementedException();
        }
    }
}
