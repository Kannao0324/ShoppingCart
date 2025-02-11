namespace ShoppingCartSW.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public double UnitPrice { get; set; }
        public string Image { get; set; }

        //Linking reference between the books and any carts it might be in the FK management
        public virtual List<ShoppingCart> CartItem { get; set; }


    }
}
