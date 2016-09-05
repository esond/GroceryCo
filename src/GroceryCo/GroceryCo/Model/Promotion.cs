using System;

namespace GroceryCo.Model
{
    public class Promotion : Entity
    {
        public Promotion()
        {
        }

        public Promotion(string groceryItemName, PromotionType type, int requiredItems)
        {
            GroceryItemName = groceryItemName;
            PromotionType = type;
            RequiredItems = requiredItems;
        }

        public string GroceryItemName { get; set; }

        public PromotionType PromotionType { get; set; }

        public double Discount { get; set; }

        public int RequiredItems { get; set; }
    }

    public enum PromotionType
    {
        OnSale = 1,
        Group = 2,
        AdditionalProduct = 3
    }
}
