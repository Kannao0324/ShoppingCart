namespace ShoppingCartSW.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public DateTime? FinalisedDate { get; set; }
        public double Total { get; set; }

        //Linking reference between the cart and user for the FK management
        public virtual AppUser User { get; set; }
        //Linking reference between the carts and any cart item it might be in the FK management
        public virtual List<ShoppingOrderLine> CartItem { get; set; }
    }
}
