using System;
using System.Collections.Generic;
using GroceryCo.Model;

namespace GroceryCo.Kiosk.Features.Cashier
{
    public class Cashier
    {
        public static void DoCheckout(IEnumerable<PurchaseItem> basketItems,
            IEnumerable<Promotion> effectivePromotions)
        {
            Purchase purchase = new Purchase(basketItems);

            purchase.ApplyManyPromotions(effectivePromotions);

            DisplayReciept(purchase);
        }

        private static void DisplayReciept(Purchase purchase)
        {
            string recieptText = ReceiptGenerator.GenerateRecieptText(purchase);

            //todo: display the reciept
            throw new NotImplementedException();
        }
    }
}
