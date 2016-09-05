using System;
using System.Collections.Generic;
using System.Linq;
using GroceryCo.Model;
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
            IEnumerable<GroceryItem> groceryItems = _repository.GetAll<GroceryItem>().ToList();

            Console.WriteLine("Select the item to end promotions for:");
            int selectedItemIndex = ConsoleHelper.SelectIndexFromStringArray(groceryItems.Select(g => g.Name).ToArray());

            GroceryItem selectedItem = groceryItems.ElementAt(selectedItemIndex);

            Promotion toRemove = _repository.GetAll<Promotion>().SingleOrDefault(p => p.GroceryItemName == selectedItem.Name);

            if (toRemove != null)
            {
                _repository.Delete(toRemove);
                Console.WriteLine($"Promotion for {selectedItem.Name} has been ended.");
            }
            else
            {
                Console.WriteLine($"No promotions for {selectedItem.Name} to end.");
            }
        }
    }
}
