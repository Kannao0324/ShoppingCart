namespace ShoppingCartSW.Models.DTOs
{
    public class LoginDTO
    {
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ReturnURL { get; set; } = string.Empty;
    }
}
