using GroceryCo.Model;

namespace GroceryCo.Kiosk.Features.Cashier
{
    public class PurchaseItem
    {
        public PurchaseItem(GroceryItem groceryItem)
        {
            GroceryItem = groceryItem;
            DiscountedPrice = groceryItem.Price;
        }

        public GroceryItem GroceryItem { get; set; }

        public decimal DiscountedPrice { get; set; }
    }
}
