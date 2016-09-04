using System;
using System.Collections.Generic;
using System.Linq;
using GroceryCo.Model;
using GroceryCo.Repository;

namespace GroceryCo.Kiosk.Features
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
                        AddPromotion();
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

        //todo: this method is death and needs to be broken out into other classes
        public void AddPromotion()
        {
            bool goBack = false;

            while (!goBack)
            {
                Console.WriteLine("Select the type of promotion you want to create...");

                Dictionary<int, string> promotionTypes =
                    Enum.GetValues(typeof(PromotionType))
                        .Cast<PromotionType>()
                        .ToDictionary(type => (int) type, type => type.ToString());

                int promotionTypeCode = ConsoleHelper.SelectFromStringArray(promotionTypes.Values.ToArray());
                
                switch ((PromotionType) promotionTypeCode)
                {
                    case PromotionType.OnSale:
                        AddOnSalePromotion();
                        break;
                    case PromotionType.Group:
                        AddGroupPromotion();
                        break;
                    case PromotionType.AdditionalProduct:
                        AddAdditionalProductPromotion();
                        break;
                    default:
                        Console.WriteLine("Invalid selection.");
                        break;
                }
            }
        }

        private void AddOnSalePromotion()
        {


            throw new NotImplementedException();
        }

        private void AddGroupPromotion()
        {
            throw new NotImplementedException();
        }

        private void AddAdditionalProductPromotion()
        {
            throw new NotImplementedException();
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

        private Promotion CreateGroupPromotion(Guid groceryItemId, int requiredItems, decimal groupPrice)
        {
            Promotion promotion = new Promotion();

            promotion.GroceryItemId = groceryItemId;
            promotion.PromotionType = PromotionType.Group;
            promotion.RequiredItems = requiredItems;

            promotion.SalePrice = groupPrice / requiredItems;

            return promotion;
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

        public void EndPromotion()
        {
            throw new NotImplementedException();
        }
    }
}
