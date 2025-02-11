using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartSW.Models.DTOs;
using ShoppingCartSW.Models.Repositories;
using System.Security.Claims;

namespace ShoppingCartSW.Controllers
{
    public class AuthenticationController : Controller
    {
        //Private field to hold a reference to our repository
        private readonly IAuthenticationRepository _repository;
        //Requesting the repository from the dependency injection by naming it in our constructor.
        public AuthenticationController(IAuthenticationRepository repository)
        {
            _repository = repository;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Login([FromQuery] string ReturnUrl)
        {
            LoginDTO loginDTO = new LoginDTO
            {
                //
                ReturnURL = string.IsNullOrWhiteSpace(ReturnUrl) ? "/Home" : ReturnUrl
            };
            return View(loginDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginDTO loginDTO)
        {
            if (ModelState.IsValid == false)
            {
                return View(loginDTO);
            }
            //Pass the DTO to the repository to see if the login details were correct.
            var user = _repository.Authenticate(loginDTO);
            //If the login failed
            if (user == null)
            {
                //Pass an error message that the viewbag then send the user back to the login page
                ViewBag.LoginMessage = "Email or Password is invalid.";
                return View(loginDTO);
            }
            //If the login was successful, add an entry to hte session data to track the user being logged in.
            //HttpContext.Session.SetString("Authenticated", "true");

            var claims = new List<Claim>
            {
                //Set the user's email in a claim that we can check for their access permissions in our [Authorize] tags
                new Claim(ClaimTypes.Email, user.Email),
                // Store the users id in their claims so we can reference it later
                new Claim("ID",user.Id.ToString())
            };
            //Holds any identities associated with the user as well as their claims.
            //This forms the data container that is held when they are logged in to our system.
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
            //Lets you set additional settings associated with the user's login and can even allow overriding some of the default cookie settings.
            var authProperties = new AuthenticationProperties
            {
                //Allows you to override the sliding expiry in the cookie settings
                AllowRefresh = true,
                //Makes the cookie stay active even after the browser closes, until the timeout lapses
                IsPersistent = true

            };
            //Signs the user in using the values we have set
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);


            //Redirect the user to the appropriate URL.
            return Redirect(loginDTO.ReturnURL);
        }

        public ActionResult Logoff()
        {
            ////Change the validated entry in the session data to track the user no longer being logged in.
            //HttpContext.Session.SetString("Authenticated", "false");

            //Clear the authentication cookie data to log out the user.
            HttpContext.SignOutAsync();
            //Redirect the user to the home page.
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Create()
        {
            //string Authenticated = HttpContext.Session.GetString("Authenticated") ?? "false";
            //if (Authenticated.Equals("false"))
            //{
            //    return RedirectToAction("Login", "Authentication");
            //}
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateUserDTO userDTO)
        {
            if (userDTO.Password.Equals(userDTO.PasswordConfirmation) == false)
            {
                //Generate an error message and go back to the form view
                ViewBag.CreateUserError = "Password and confirmation password don't match.";
                return View(userDTO);
            }
            //Check that the provided data meets the rules in the data model
            if (ModelState.IsValid == false)
            {
                return View(userDTO);
            }
            //Attempt the save the user to the database
            var user = _repository.CreateUser(userDTO);
            //If the repository returns null to indicate an error
            if (user == null)
            {
                //Generate an error message and go back to the form view
                ViewBag.CreateUserError = "Username already exists. Please choose a different username";
                return View(userDTO);

            }
            //If successful, generate a success message and clear the model in the form
            ViewBag.CreateUserConfirmation = "User Account Created";
            ModelState.Clear();
            //Return to the view page
            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}
