using ShoppingCartSW.Models.DTOs;

namespace ShoppingCartSW.Models.Repositories
{
    public interface IAuthenticationRepository
    {
        AppUser Authenticate(LoginDTO loginDTO);
        AppUser CreateUser(CreateUserDTO userDTO);
    }
}
