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

            List<Tuple<string, string, string, string>> itemStrings = new List<Tuple<string, string, string, string>>();

            foreach (PurchaseItem item in purchase.PurchaseItems.OrderBy(i => i.GroceryItemName))
            {
                string isDiscounted = item.DiscountedPrice != item.GroceryItem.Price ? "\u2713" : string.Empty;

                itemStrings.Add(new Tuple<string, string, string, string>(item.GroceryItemName,
                    item.GroceryItem.Price.ToString("c"), item.DiscountedPrice.ToString("c"), isDiscounted));
            }

            builder.AppendLine(itemStrings.ToStringTable(
                new[] {"Item Name", "Price", "Sale Price", "Discounted"},
                i => i.Item1, i => i.Item2, i => i.Item3, i => i.Item4));

            builder.Append($"TOTAL: {purchase.Total:c}");

            return builder.ToString();
        }
    }
}
