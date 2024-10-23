using BLL.Helpers;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PL.ViewModels;
using System.Threading.Tasks;

namespace PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor; // Use the interface here

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor; // Correctly set here
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                // Check if the email is already in use
                var existingEmailUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingEmailUser != null)
                {
                    ModelState.AddModelError("Email", "This email is already in use.");
                    return View(model);
                }

                // Create a new user
                var user = new User
                {
                    UserName = model.UserName, // No uniqueness check for username
                    Email = model.Email,
                };

                // Create the user with the specified password
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                // Add errors to the model state if the user creation failed
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Find the user by email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.CheckPasswordAsync(user, model.Password)))
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }

            // Sign in the user
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                User? user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    string url = Url.Action("ResetPassword", "Account", new { email = user.Email, token = token }, "HTTPS", _httpContextAccessor.HttpContext.Request.Host.ToString());

                    // Prepare and send the email
                    Email email = new Email()
                    {
                        To = user.Email,
                        From = "entertainmentcamb@gmail.com",
                        Title = "Reset Password",
                        Body = url
                    };
                    EmailSetting.SendEmail(email);
                    return View("CheckEmail");
                }
            }
            ModelState.AddModelError("", "Email does not exist.");
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword([FromQuery]string email , string token)
        {
            ResetPasswordVM model = new ResetPasswordVM();
            TempData["token"]=token;
            TempData["email"]=email;
            return View(model);
        }

        [HttpPost]
        public async  Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            string token = TempData["token"] as string;
            string email = TempData["email"] as string;
            if (ModelState.IsValid) 
            {
                User? user = await _userManager.FindByEmailAsync(email);
                if (user is not null) {
                    IdentityResult result = await _userManager.ResetPasswordAsync(user, token, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Login");
                    }
                }
              
            }
            return View("ResetPassword",model);

        }
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
