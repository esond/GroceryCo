using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GroceryCo.Kiosk.Infrastructure;

namespace GroceryCo.Kiosk.Features.Cashier
{
    public static class ReceiptGenerator
    {
        public static string GenerateReciept(Purchase purchase)
        {
            StringBuilder builder = new StringBuilder();

            List<Tuple<string, string, string>> itemStrings = new List<Tuple<string, string, string>>();

            foreach (PurchaseItem item in purchase.PurchaseItems.OrderBy(i => i.GroceryItemName))
            {
                itemStrings.Add(new Tuple<string, string, string>(item.GroceryItemName,
                    item.GroceryItem.Price.ToString("c"), item.DiscountedPrice.ToString("c")));
            }

            builder.Append(itemStrings.ToStringTable(
                new[] {"Item Name", "Price", "Sale Price"},
                i => i.Item1, i => i.Item2, i => i.Item3));

            builder.Append($"TOTAL: {purchase.Total:c}");

            return builder.ToString();
        }
    }
}
