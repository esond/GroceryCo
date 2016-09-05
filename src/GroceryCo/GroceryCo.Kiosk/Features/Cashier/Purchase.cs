using System;
using System.Collections.Generic;
using System.Linq;
using GroceryCo.Model;

namespace GroceryCo.Kiosk.Features.Cashier
{
    public class Purchase
    {
        public Purchase(IEnumerable<PurchaseItem> purchaseItems)
        {
            PurchaseItems = purchaseItems;
        }

        public IEnumerable<PurchaseItem> PurchaseItems { get; set; }

        public decimal Total => PurchaseItems.Sum(pi => pi.DiscountedPrice);

        public void ApplyManyPromotions(IEnumerable<Promotion> promotions)
        {
            foreach (Promotion promotion in promotions)
            {
                ApplyPromotion(promotion);
            }
        }

        public void ApplyPromotion(Promotion promotion)
        {
            switch (promotion.PromotionType)
            {
                case PromotionType.OnSale:
                    ApplyOnSalePromotion(promotion);
                    break;

                case PromotionType.Group:
                    ApplyGroupPromotion(promotion);
                    break;

                case PromotionType.AdditionalProduct:
                    ApplyAdditionalProductPromotion(promotion);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ApplyOnSalePromotion(Promotion promotion)
        {
            foreach (PurchaseItem item in PurchaseItems.Where(pi => pi.GroceryItemName == promotion.GroceryItemName))
            {
                double discountedPrice = (double) item.DiscountedPrice*promotion.Discount;

                item.DiscountedPrice = Math.Round(Convert.ToDecimal(discountedPrice), 2);
            }
        }

        private void ApplyGroupPromotion(Promotion promotion)
        {
            IEnumerable<PurchaseItem> applicableItems =
                PurchaseItems.Where(pi => pi.GroceryItemName == promotion.GroceryItemName).ToList();

            if (promotion.RequiredItems == 0)
                return;

            for (int i = 0; i < applicableItems.Count(); i += promotion.RequiredItems)
            {
                IEnumerable<PurchaseItem> group = applicableItems.Skip(i).Take(promotion.RequiredItems).ToList();

                if (group.Count() != promotion.RequiredItems) continue;

                foreach (PurchaseItem item in group)
                {
                    double discountedPrice = (double)item.DiscountedPrice * promotion.Discount;

                    item.DiscountedPrice = Math.Round(Convert.ToDecimal(discountedPrice), 2);
                }
            }
        }

        private void ApplyAdditionalProductPromotion(Promotion promotion)
        {
            IEnumerable<PurchaseItem> applicableItems =
                PurchaseItems.Where(pi => pi.GroceryItemName == promotion.GroceryItemName).ToList();

            for (int i = 0; i < applicableItems.Count(); i += promotion.RequiredItems + 1)
            {
                IEnumerable<PurchaseItem> group = applicableItems.Skip(i).Take(promotion.RequiredItems + 1).ToList();

                if (group.Count() != promotion.RequiredItems + 1) continue;

                PurchaseItem item = group.Last();

                double discountedPrice = (double) item.DiscountedPrice * promotion.Discount;

                item.DiscountedPrice = Math.Round(Convert.ToDecimal(discountedPrice), 2);
            }
        }
    }
}
