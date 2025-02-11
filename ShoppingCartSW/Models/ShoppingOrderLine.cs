namespace ShoppingCartSW.Models
{
    public class ShoppingOrderLine
    {
        public int Id { get; set; }
        public int ShoppingCartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        //Linking reference to show the relationship between carts, books and this table.
        public virtual ShoppingCart ShoppingCart { get; set; }
        public virtual Product product { get; set; }
    }
}
