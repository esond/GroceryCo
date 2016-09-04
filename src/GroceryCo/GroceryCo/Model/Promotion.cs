using System;

namespace GroceryCo.Model
{
    public class Promotion : Entity
    {
        public Promotion(DiscountType discountType)
        {
            DiscountType = discountType;
        }

        public DiscountType DiscountType { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public int DaysInEffect { get; set; }

        public Guid GroceryItemId { get; set; }
    }

    public enum DiscountType
    {
        OnSale,
        Group,
        AdditionalProduct
    }
}
