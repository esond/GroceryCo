using GroceryCo.Model;

namespace GroceryCo.Kiosk.Infrastructure
{
    public static class PromotionFactory
    {
        public static Promotion CreatePromotion(GroceryItem groceryItem, PromotionType type, int requiredItems,
            decimal salePrice)
        {
            Promotion promotion = new Promotion(groceryItem.Name, type, requiredItems);

            promotion.Discount = groceryItem.Price == decimal.Zero
                ? promotion.Discount = 0
                : promotion.Discount = (double)(salePrice / groceryItem.Price);

            return promotion;
        }

        public static Promotion CreatePromotion(GroceryItem groceryItem, PromotionType type, int requiredItems,
            double discount)
        {
            Promotion promotion = new Promotion(groceryItem.Name, type, requiredItems);
            promotion.Discount = discount;

            return promotion;
        }
    }
}
