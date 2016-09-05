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
                ApplyPromotion(promotion);
        }

        public void ApplyPromotion(Promotion promotion)
        {
            throw new NotImplementedException();
        }

    }
}
