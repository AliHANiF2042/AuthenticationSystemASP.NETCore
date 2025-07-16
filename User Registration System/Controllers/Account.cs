using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using User_Registration_System.Context;
using User_Registration_System.Models.ViewModels;
using User_Registration_System.Models;
using User_Registration_System.Security;

namespace User_Registration_System.Controllers
{
    public class Account : Controller
    {
        private readonly context _context;
        private readonly iUserServices _iUserServices;

        public Account(context context, iUserServices iUserServices)
        {
            _context = context;
            _iUserServices = iUserServices;
        }

        #region GET
        public IActionResult Login(bool EditProfile = false)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home", new { area = "UserPanel" });
            }

            ViewBag.EditProfile = EditProfile;
            return View();
        }

        public IActionResult SuccessRegister()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        #endregion

        #region POST
        [HttpPost]
        public IActionResult Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            if (_iUserServices.isExistUserName(register.userName))
            {
                ModelState.AddModelError("userName", "این نام کاربری قبلا ثبت شده است!");
                return View(register);
            }
            if (_iUserServices.isExistEmail(FixedEmail.FixEmail(register.userEmail)))
            {
                ModelState.AddModelError("userEmail", "ایمیل وارد شده قبلا ثبت شده است!");
                return View(register);
            }

            User user = new User()
            {
                userName = register.userName,
                userEmail = FixedEmail.FixEmail(register.userEmail),
                password = PasswordHash.HashPassword(register.password),
                registerDate = DateTime.UtcNow,
                userProfile = "",

            };
            _iUserServices.addUser(user);

            return View("SuccessRegister", user);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var user = _iUserServices.LoginUser(login);

            if (user != null)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.userId.ToString()),
                    new Claim(ClaimTypes.Name, user.userName),
                    new Claim(ClaimTypes.Email, user.userEmail)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var properties = new AuthenticationProperties
                {
                    IsPersistent = login.rememberMe
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);
                ViewBag.IsSuccess = true;

                return RedirectToAction("Profile", "Home", new { area = "UserPanel" });
            }

            ModelState.AddModelError("userEmail", "کاربری با مشخصات وارد شده یافت نشد");
            return View(login);
        }


        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
        #endregion

    }
}
