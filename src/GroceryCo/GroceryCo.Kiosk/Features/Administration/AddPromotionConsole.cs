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

            if (GroceryItemHasExistingPromotions(toPromote.Id))
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
                    AddOnSalePromotion(toPromote.Id);
                    break;
                case PromotionType.Group:
                    AddGroupPromotion(toPromote.Id);
                    break;
                case PromotionType.AdditionalProduct:
                    AddAdditionalProductPromotion(toPromote.Id);
                    break;
                default:
                    Console.WriteLine("Invalid selection.");
                    return;
            }

            Console.WriteLine($"Promotion added for {toPromote.Name}.");
        }

        private void AddOnSalePromotion(Guid groceryItemId)
        {
            decimal salePrice = GetSalePrice();

            Promotion promotion = CreateOnSalePromotion(groceryItemId, salePrice);
            _repository.Create(promotion);
        }

        private Promotion CreateOnSalePromotion(Guid groceryItemId, decimal salePrice)
        {
            Promotion promotion = new Promotion();

            promotion.GroceryItemId = groceryItemId;
            promotion.PromotionType = PromotionType.OnSale;
            promotion.RequiredItems = 0;
            promotion.SalePrice = salePrice;

            return promotion;
        }

        private void AddGroupPromotion(Guid groceryItemId)
        {
            int requiredItems = GetRequiredItems();
            decimal salePrice = GetSalePrice();

            Promotion promotion = CreateGroupPromotion(groceryItemId, requiredItems, salePrice);
            _repository.Create(promotion);
        }

        private Promotion CreateGroupPromotion(Guid groceryItemId, int requiredItems, decimal groupPrice)
        {
            Promotion promotion = new Promotion();

            promotion.GroceryItemId = groceryItemId;
            promotion.PromotionType = PromotionType.Group;
            promotion.RequiredItems = requiredItems;

            promotion.SalePrice = groupPrice / requiredItems;

            return promotion;
        }

        private void AddAdditionalProductPromotion(Guid groceryItemId)
        {
            int requiredItems = GetRequiredItems();

            Console.Write("Enter the discount (%) for this promotion:");
            double discount = double.Parse(Console.ReadLine());

            if (discount > 1)
                discount = discount/100;

            Promotion promotion = CreateAdditionalProductPromotion(groceryItemId, requiredItems, discount);
            _repository.Create(promotion);
        }

        private Promotion CreateAdditionalProductPromotion(Guid groceryItemId, int requiredItems, double discount)
        {
            Promotion promotion = new Promotion();

            promotion.GroceryItemId = groceryItemId;
            promotion.PromotionType = PromotionType.Group;
            promotion.RequiredItems = requiredItems;

            GroceryItem item = _repository.GetAll<GroceryItem>().Single(g => g.Id == groceryItemId);

            double calculatedSalePrice = decimal.ToDouble(item.Price) * discount;

            promotion.SalePrice = new decimal(Math.Round(calculatedSalePrice, 2));

            return promotion;
        }

        private GroceryItem SelectGroceryItem()
        {
            Console.WriteLine("Select the item for this promotion:");

            List<GroceryItem> items = _repository.GetAll<GroceryItem>().ToList();
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

        private bool GroceryItemHasExistingPromotions(Guid groceryItemId)
        {
            if (_repository.GetAll<Promotion>().Any(p => p.GroceryItemId == groceryItemId))
            {
                Console.WriteLine("There is already a promotion running for this item.");
                return true;
            }

            return false;
        }
    }
}
