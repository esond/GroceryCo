namespace GroceryCo.Kiosk.Model
{
    public class GroceryItem
    {
        public GroceryItem(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
