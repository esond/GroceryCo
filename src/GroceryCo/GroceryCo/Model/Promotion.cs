using System;

namespace GroceryCo.Model
{
    public class Promotion : Entity
    {
        public PromotionType PromotionType { get; set; }

        public Guid GroceryItemId { get; set; }

        public decimal SalePrice { get; set; }

        public int RequiredItems { get; set; }
    }

    public enum PromotionType
    {
        OnSale = 1,
        Group = 2,
        AdditionalProduct = 3
    }
}
