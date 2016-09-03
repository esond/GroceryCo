using System;

namespace GroceryCo.Kiosk.Model
{
    public class Promotion : Entity
    {
        public Promotion(DiscountType discountType, GroceryItem groceryItem)
        {
            DiscountType = discountType;
            GroceryItem = groceryItem;
        }

        public DiscountType DiscountType { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public int DaysInEffect { get; set; }

        public GroceryItem GroceryItem { get; set; }
    }

    public enum DiscountType
    {
        OnSale,
        Group,
        AdditionalProduct
    }
}
