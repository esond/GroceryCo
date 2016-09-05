using System;
using System.Collections.Generic;
using System.Linq;
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

            Dictionary<int, string> promotionTypes =
                Enum.GetValues(typeof(PromotionType))
                    .Cast<PromotionType>()
                    .ToDictionary(type => (int)type, type => type.ToString());

            int promotionTypeCode = ConsoleHelper.SelectFromStringArray(promotionTypes.Values.ToArray());

            switch ((PromotionType)promotionTypeCode)
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

            Promotion promotion = CreatePromotion(groceryItem, PromotionType.OnSale, 0, salePrice);
            _repository.Create(promotion);
        }

        private void AddGroupPromotion(GroceryItem groceryItem)
        {
            int requiredItems = GetRequiredItems();
            decimal salePrice = GetSalePrice();

            Promotion promotion = CreatePromotion(groceryItem, PromotionType.Group, requiredItems, salePrice);
            _repository.Create(promotion);
        }

        private void AddAdditionalProductPromotion(GroceryItem groceryItem)
        {
            int requiredItems = GetRequiredItems();

            Console.Write("Enter the discount (%) for this promotion:");
            double discount = double.Parse(Console.ReadLine());

            if (discount > 1)
                discount = discount/100;

            Promotion promotion = CreatePromotion(groceryItem, PromotionType.AdditionalProduct, requiredItems, discount);
            _repository.Create(promotion);
        }

        private GroceryItem SelectGroceryItem()
        {
            Console.WriteLine("Select the item for this promotion:");

            IEnumerable<GroceryItem> items = _repository.GetAll<GroceryItem>().ToList();
            int itemIndex = ConsoleHelper.SelectFromStringArray(items.Select(i => i.Name).ToArray());

            return items.ElementAt(itemIndex);
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

        public static Promotion CreatePromotion(GroceryItem groceryItem, PromotionType type, int requiredItems,
            decimal salePrice)
        {
            Promotion promotion = new Promotion(groceryItem.Name, type, requiredItems);

            promotion.Discount = groceryItem.Price == decimal.Zero
                ? promotion.Discount = 0
                : promotion.Discount = (double)(salePrice / groceryItem.Price);
            
            return promotion;
        }

        public static Promotion CreatePromotion(GroceryItem groceryItem, PromotionType type, int requiredItems,
            double discount)
        {
            Promotion promotion = new Promotion(groceryItem.Name, type, requiredItems);
            promotion.Discount = discount;

            return promotion;
        }
    }
}
