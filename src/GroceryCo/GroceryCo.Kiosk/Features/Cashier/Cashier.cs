using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
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
            string receiptPath = Path.Combine(ConfigurationManager.AppSettings["RepositoryFolder"], "receipt.txt");
            
            using (StreamWriter file = new StreamWriter(receiptPath))
            {
                file.Write(ReceiptGenerator.GenerateReciept(purchase));
            }

            Process.Start(receiptPath);
        }
    }
}
