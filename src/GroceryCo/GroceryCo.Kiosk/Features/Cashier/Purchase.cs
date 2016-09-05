using System;
using System.Collections.Generic;
using GroceryCo.Model;

namespace GroceryCo.Kiosk.Features.Cashier
{
    public class Purchase
    {
        public Purchase()
        {
        }

        public Purchase(IEnumerable<PurchaseItem> purchaseItems)
        {
            PurchaseItems = purchaseItems;
        }

        public IEnumerable<PurchaseItem> PurchaseItems { get; set; }

        public void ApplyPromotions(IEnumerable<Promotion> promotion)
        {
            throw new NotImplementedException();
        }
    }
}
