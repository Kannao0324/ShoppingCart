using ShoppingCartSW.Models.Data;
using ShoppingCartSW.Models.DTOs;

namespace ShoppingCartSW.Models.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        //Readonly variable to store a reference to our context class
        private readonly ShoppingCartDBContext _context;
        //Request the context from the dependency injection by naming it in the constructor
        public AuthenticationRepository(ShoppingCartDBContext context)
        {
            _context = context;
        }
        public AppUser Authenticate(LoginDTO loginDTO)
        {
            //Find the user that has the same username as the one provided in the login DTO
            var userDetails = _context.AppUsers.Where(u => u.Email.Equals(loginDTO.Email)).FirstOrDefault();
            //If no user was found, return null to let the caller know that the login failed.
            if (userDetails == null)
            {
                return null;
            }
            //Use BCrypt to check the password provided in the user DTO agains
            //the hashed password stored in the user account we just recieved.
            if (BCrypt.Net.BCrypt.EnhancedVerify(loginDTO.Password, userDetails.Password))
            {
                //if they match, return the user details to the caller to let them know it worked
                return userDetails;
            }
            // If the check failed, return null to let the caller know that the login failed.
            return null;
        }

        public AppUser CreateUser(CreateUserDTO userDTO)
        {
            //Find the user that has the same username as the one provided in the login DTO
            var userDetails = _context.AppUsers.Where(u => u.Email.Equals(userDTO.Email)).FirstOrDefault();
            //if the username returns a record, meaning the username is already taken.
            if (userDetails != null)
            {
                //Return null to the caller to let them know the account couldn't be created.
                return null;
            }

            var user = new AppUser
            {
                Email = userDTO.Email,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(userDTO.Password)
            };
            //Add the user to the context class then save the chages to the database
            _context.AppUsers.Add(user);
            _context.SaveChanges();
            //Return the user details to the caller to confirm it worked.
            return user;

        }
    }
}
