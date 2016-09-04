using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            throw new NotImplementedException();
        }

        public void AddPromotion()
        {
            throw new NotImplementedException();
        }

        public void EndPromotion()
        {
            throw new NotImplementedException();
        }

        private Promotion CreateOnSalePromotion(Guid groceryItemId, int daysInEffect, decimal salePrice)
        {
            Promotion promotion = new Promotion();

            promotion.GroceryItemId = groceryItemId;
            promotion.DiscountType = DiscountType.OnSale;
            promotion.DaysInEffect = daysInEffect;
            promotion.RequiredItems = 0;
            promotion.SalePrice = salePrice;
            

            return promotion;
        }

        private Promotion CreateGroupPromotion(Guid groceryItemId, int daysInEffect, int requiredItems, decimal groupPrice)
        {
            Promotion promotion = new Promotion();

            promotion.GroceryItemId = groceryItemId;
            promotion.DiscountType = DiscountType.Group;
            promotion.DaysInEffect = daysInEffect;
            promotion.RequiredItems = requiredItems;

            promotion.SalePrice = groupPrice / requiredItems;

            return promotion;
        }

        private Promotion CreateAdditionalProductPromotion(Guid groceryItemId, int daysInEffect, int requiredItems, double discount)
        {
            Promotion promotion = new Promotion();

            promotion.GroceryItemId = groceryItemId;
            promotion.DiscountType = DiscountType.Group;
            promotion.DaysInEffect = daysInEffect;
            promotion.RequiredItems = requiredItems;

            GroceryItem item = _repository.GetAll<GroceryItem>().Single(g => g.Id == groceryItemId);

            double calculatedSalePrice = decimal.ToDouble(item.Price)*discount;

            promotion.SalePrice = new decimal(Math.Round(calculatedSalePrice, 2));

            return promotion;
        }
    }
}
