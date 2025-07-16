using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using User_Registration_System.Context;
using User_Registration_System.Models.ViewModels;

namespace User_Registration_System.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly iUserServices _iUserServices;
        private readonly ILogger<HomeController> _logger;
        private readonly context _context;

        public HomeController(iUserServices iUserServices, ILogger<HomeController> logger, context context)
        {
            _iUserServices = iUserServices;
            _logger = logger;
            _context = context;
        }

        #region GET
        [HttpGet]
        public IActionResult Profile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var userName = User.Identity.Name;
            var currentUser = _iUserServices.GetInformations(userName);
            if (currentUser == null)
            {
                HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Account");
            }

            var model = new UserPanelVM
            {
                UserPanelViewModel = new UserPanelViewModel
                {
                    userName = currentUser.userName,
                    userEmail = currentUser.userEmail,
                    userProfile = currentUser.userProfile
                },
                EditProfileViewModel = new EditProfileViewModel
                {
                    userName = currentUser.userName,
                    userEmail = currentUser.userEmail
                }
            };

            return View(model);
        }


        public IActionResult EditProfile()
        {
            var currentUser = _iUserServices.GetInfoForEdit(User.Identity.Name);

            var model = new UserPanelVM
            {
                EditProfileViewModel = new EditProfileViewModel
                {
                    userName = currentUser.userName,
                    userEmail = currentUser.userEmail,
                    avatarName = currentUser.avatarName
                }
            };

            return View(model);
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        public IActionResult Index()
        {
            try
            {
                var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userID, out int userId))
                {
                    return View();
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "متاسفانه خطایی رخ داد!");
                return View();
            }
        }
        #endregion


        #region POST
        [HttpPost]
        public IActionResult EditProfile(UserPanelVM model)
        {
            if (!ModelState.IsValid)
            {
                _iUserServices.EditProfile(User.Identity.Name, model.EditProfileViewModel);

                TempData["SuccessMessage"] = "اطلاعات با موفقیت ویرایش شد.";

                var updatedUser = _iUserServices.GetInformations(User.Identity.Name);

                var updatedModel = new UserPanelVM
                {
                    UserPanelViewModel = new UserPanelViewModel
                    {
                        userName = updatedUser.userName,
                        userEmail = updatedUser.userEmail,
                        userProfile = updatedUser.userProfile
                    },
                    EditProfileViewModel = new EditProfileViewModel
                    {
                        userName = updatedUser.userName,
                        userEmail = updatedUser.userEmail,
                        avatarName = updatedUser.userProfile
                    }
                };

                return View("Profile", updatedModel);
            }

            return View("Profile", model);
        }

        [HttpPost]
        public IActionResult UploadAvatar(IFormFile userProfile)
        {
            if (userProfile == null || userProfile.Length == 0)
            {
                return Json(new { success = false, message = "لطفاً یک فایل انتخاب کنید." });
            }

            try
            {
                var userName = User.Identity.Name;
                var user = _iUserServices.GetUserName(userName);
                if (user == null)
                {
                    return Json(new { success = false, message = "کاربر مورد نظر یافت نشد." });
                }

                if (user.userProfile != "profile.png")
                {
                    string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Profile", user.userProfile);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                string newFileName = Guid.NewGuid() + Path.GetExtension(userProfile.FileName);
                string newImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Profile", newFileName);

                using (var stream = new FileStream(newImagePath, FileMode.Create))
                {
                    userProfile.CopyTo(stream);
                }

                user.userProfile = newFileName;
                _iUserServices.UpdateUser(user);

                return Json(new { success = true, newFileName = newFileName });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(UserPanelVM model)
        {
            var changePassword = model.ResetPasswordViewModel;

            if (ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                TempData["ErrorMessage"] = "لطفاً اطلاعات را به درستی وارد کنید: " + string.Join(", ", errors.Select(e => e.ErrorMessage));
                return RedirectToAction("Profile");
            }

            if (string.IsNullOrEmpty(changePassword.oldPassword))
            {
                TempData["ErrorMessage"] = "لطفاً پسورد قدیمی را وارد کنید.";
                return RedirectToAction("Profile");
            }

            if (string.IsNullOrEmpty(changePassword.newPassword) || string.IsNullOrEmpty(changePassword.rePassword))
            {
                TempData["ErrorMessage"] = "لطفاً پسورد جدید و تکرار آن را وارد کنید.";
                return RedirectToAction("Profile");
            }

            if (changePassword.newPassword != changePassword.rePassword)
            {
                TempData["ErrorMessage"] = "پسوردهای وارد شده مطابقت ندارند!";
                return RedirectToAction("Profile");
            }

            string findUser = User.Identity.Name;

            if (string.IsNullOrEmpty(findUser))
            {
                TempData["ErrorMessage"] = "کاربر یافت نشد!";
                return RedirectToAction("Profile");
            }

            if (!_iUserServices.ChangeOldPassword(changePassword.oldPassword, findUser))
            {
                TempData["ErrorMessage"] = "پسورد قدیمی اشتباه است!";
                return RedirectToAction("Profile");
            }

            try
            {
                _iUserServices.NewUserPassword(findUser, changePassword.newPassword);
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                TempData["SuccessMessage"] = "پسورد شما با موفقیت تغییر یافت! لطفاً دوباره وارد شوید.";
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "خطایی رخ داده است: " + ex.Message;
                return RedirectToAction("Profile");
            }
        }

        public async Task<IActionResult> DeleteUserAccount()
        {
            string findUser = User.Identity.Name;
            if ((string.IsNullOrEmpty(findUser)))
            {
                TempData["ErrorMessage"] = "کاربر یافت نشد!";
                return RedirectToAction("Profile");
            }

            _iUserServices.DeteleUserAccountFromSite(findUser);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["SuccessMessage"] = "حساب کاربری شما با موفقیت حذف شد!";

            return RedirectToAction("Register", "Account", new { area = "" });
        }
        #endregion

    }
}
