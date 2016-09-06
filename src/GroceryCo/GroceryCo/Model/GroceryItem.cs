namespace GroceryCo.Model
{
    public class GroceryItem : Entity
    {
        public GroceryItem(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public override string ToString() => Name;
    }
}
