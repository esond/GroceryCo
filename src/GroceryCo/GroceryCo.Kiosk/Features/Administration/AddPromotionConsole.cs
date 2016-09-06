using System;
using System.Collections.Generic;
using System.Linq;
using GroceryCo.Kiosk.Infrastructure;
using GroceryCo.Model;
using GroceryCo.Repository;

namespace GroceryCo.Kiosk.Features.Administration
{
    public class AddPromotionConsole
    {
        private readonly IRepository _repository;

        public AddPromotionConsole(IRepository repository)
        {
            _repository = repository;
        }

        public void Run()
        {
            GroceryItem toPromote = SelectGroceryItem();

            if (GroceryItemHasExistingPromotions(toPromote.Name))
                return;

            Console.WriteLine("Select the type of promotion you want to create...");

            PromotionType type = ConsoleHelper.SelectFrom(Enum.GetValues(typeof(PromotionType)).Cast<PromotionType>());
            
            switch (type)
            {
                case PromotionType.OnSale:
                    AddOnSalePromotion(toPromote);
                    break;
                case PromotionType.Group:
                    AddGroupPromotion(toPromote);
                    break;
                case PromotionType.AdditionalProduct:
                    AddAdditionalProductPromotion(toPromote);
                    break;
                default:
                    Console.WriteLine("Invalid selection.");
                    return;
            }

            Console.WriteLine($"Promotion added for {toPromote.Name}.");
        }

        private void AddOnSalePromotion(GroceryItem groceryItem)
        {
            decimal salePrice = GetSalePrice();

            Promotion promotion = PromotionFactory.CreatePromotion(groceryItem, PromotionType.OnSale, 0, salePrice);
            _repository.Create(promotion);
        }

        private void AddGroupPromotion(GroceryItem groceryItem)
        {
            int requiredItems = GetRequiredItems();
            decimal salePrice = GetSalePrice();

            Promotion promotion = PromotionFactory.CreatePromotion(groceryItem, PromotionType.Group, requiredItems, salePrice);
            _repository.Create(promotion);
        }

        private void AddAdditionalProductPromotion(GroceryItem groceryItem)
        {
            int requiredItems = GetRequiredItems();

            Console.Write("Enter the discount (%) for this promotion:");
            double discount = double.Parse(Console.ReadLine());

            if (discount > 1)
                discount = discount/100;

            Promotion promotion = PromotionFactory.CreatePromotion(groceryItem, PromotionType.AdditionalProduct, requiredItems, discount);
            _repository.Create(promotion);
        }

        private GroceryItem SelectGroceryItem()
        {
            Console.WriteLine("Select the item for this promotion:");

            IEnumerable<GroceryItem> items = _repository.GetAll<GroceryItem>().ToList();

            return ConsoleHelper.SelectFrom(items);
        }

        private decimal GetSalePrice()
        {
            Console.Write("Enter the sale price (0.00):");
            return decimal.Parse(Console.ReadLine());
        }

        private int GetRequiredItems()
        {
            Console.Write("Enter the number of items required for eligibility:");
            return int.Parse(Console.ReadLine());
        }

        private bool GroceryItemHasExistingPromotions(string groceryItemName)
        {
            if (_repository.GetAll<Promotion>().All(p => p.GroceryItemName != groceryItemName))
                return false;

            Console.WriteLine("There is already a promotion running for this item.");
            return true;
        }

        
    }
}
