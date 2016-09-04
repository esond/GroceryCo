using System;

namespace GroceryCo.Model
{
    public class Promotion : Entity
    {
        public DiscountType DiscountType { get; set; }

        public int DaysInEffect { get; set; }

        public Guid GroceryItemId { get; set; }

        public decimal SalePrice { get; set; }

        public int RequiredItems { get; set; }
    }

    
}
