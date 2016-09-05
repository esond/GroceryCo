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

                item.DiscountedPrice = Convert.ToDecimal(discountedPrice);
            }
        }

        private void ApplyAdditionalProductPromotion(Promotion promotion)
        {
            throw new NotImplementedException();
        }

        private void ApplyGroupPromotion(Promotion promotion)
        {
            throw new NotImplementedException();
        }
    }
}
