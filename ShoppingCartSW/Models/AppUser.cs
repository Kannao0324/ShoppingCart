namespace ShoppingCartSW.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public virtual List<ShoppingCart> ShoppingCarts { get; set; }
    }
}
