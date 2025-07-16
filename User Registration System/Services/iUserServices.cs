using Microsoft.AspNetCore.Identity;
using NuGet.Configuration;
using User_Registration_System.Models;
using User_Registration_System.Context;
using User_Registration_System.Models.ViewModels;
using User_Registration_System.Security;

namespace User_Registration_System
{
    public interface iUserServices
    {
        bool isExistUserName(string userName);
        bool isExistEmail(string userEmail);
        int addUser(User user);
        User LoginUser(LoginViewModel login);
        User GetUserEmail(string userEmail);
        void UpdateUser(User user);
        int GetUserNameById(string userName);
        SidebarViewModel GetInfoForSidebar(string userName);


        #region UserPanel
        User GetUserName(string userName);
        User GetUserById(int id);
        int GetUserIdByName(string userName);
        bool ChangeOldPassword(string password, string userName);
        void NewUserPassword(string userName, string newPassword);
        void DeteleUserAccountFromSite(string userName);
        string GetUserProfilePicture(string userName);
        EditProfileViewModel GetInfoForEdit(string userName);
        void EditProfile(string userName, EditProfileViewModel model);
        UserPanelViewModel GetInformations(string userName);
        #endregion
    }

    public class UserServices : iUserServices
    {
        private readonly context _context;
        private readonly ILogger<UserServices> _logger;

        public UserServices(context context, ILogger<UserServices> logger)
        {
            _context = context;
            _logger = logger;
        }

        public bool isExistUserName(string userName)
        {
            return _context.Users.Any(u => u.userName == userName);
        }

        public bool isExistEmail(string userEmail)
        {
            return _context.Users.Any(u => u.userEmail == userEmail);
        }

        public int addUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return user.userId;
        }

        public User LoginUser(LoginViewModel login)
        {
            var user = _context.Users.FirstOrDefault(u => u.userEmail == login.userEmail);
            if (user != null && PasswordHash.VerifyPassword(login.password, user.password))
            {
                return user;
            }
            return null;
        }


        public User GetUserEmail(string userEmail)
        {
            return _context.Users.SingleOrDefault(u => u.userEmail == userEmail);
        }

        public void UpdateUser(User user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }



        public User GetUserName(string username)
        {
            return _context.Users.SingleOrDefault(u => u.userName == username);
        }


        public UserPanelViewModel GetInformations(string userName)
        {
            var user = _context.Users.FirstOrDefault(u => u.userName == userName);
            if (user == null)
            {
                return null;
            }

            return new UserPanelViewModel
            {
                userName = user.userName,
                userEmail = user.userEmail,
                userProfile = user.userProfile
            };
        }


        public UserPanelViewModel GetInformationsById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("شناسه کاربری نمی تواند 0 یا کمتر باشد!");
            }

            try
            {
                var user = GetUserById(id);
                if (user == null)
                {
                    throw new Exception($"کاربر با شناسه {id} مورد نظر یافت نشد!");
                }

                UserPanelViewModel userPanel = new UserPanelViewModel();
                userPanel.userName = user.userName;
                userPanel.userEmail = user.userEmail;
                userPanel.registerDate = user.registerDate;
                userPanel.userWallet = 0;

                return userPanel;
            }
            catch (Exception ex)
            {
                throw new Exception($"خطایی در ویرایش کاربران در پنل مدیریت رخ داد!{ex.Message}");
            }
        }

        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public int GetUserIdByName(string userName)
        {
            return _context.Users.Single(u => u.userName == userName).userId;
        }

        public void EditProfile(string userName, EditProfileViewModel model)
        {
            var user = _context.Users.SingleOrDefault(u => u.userName == userName);
            if (user != null)
            {
                user.userName = model.userName;
                user.userEmail = model.userEmail;
                user.userProfile = model.avatarName ?? user.userProfile;

                _context.SaveChanges();
            }
        }

        public int GetUserNameById(string userName)
        {
            return _context.Users.SingleOrDefault(u => u.userName == userName).userId;
        }

        public EditProfileViewModel GetInfoForEdit(string userName)
        {
            return _context.Users.Where(u => u.userName == userName)
                .Select(u => new EditProfileViewModel()
                {
                    avatarName = u.userProfile,
                    userEmail = u.userEmail,
                    userName = u.userName

                })
                .Single();
        }

        public bool ChangeOldPassword(string password, string userName)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("مقادیر ورودی نباید null باشند.");
            }

            var user = _context.Users.FirstOrDefault(u => u.userName == userName);
            if (user == null)
            {
                return false;
            }

            return PasswordHash.VerifyPassword(password, user.password);
        }

        public void NewUserPassword(string userName, string newPassword)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentNullException("مقادیر ورودی نباید null باشند.");
            }

            var user = _context.Users.FirstOrDefault(u => u.userName == userName);
            if (user == null)
            {
                throw new Exception("کاربر یافت نشد!");
            }

            string hashNewPassword = PasswordHash.HashPassword(newPassword);
            if (string.IsNullOrEmpty(hashNewPassword))
            {
                throw new Exception("متاسفانه هش کردن پسورد با خطا مواجه شد!");
            }

            user.password = hashNewPassword;

            UpdateUser(user);
            _context.SaveChanges();

            Console.WriteLine("پسورد جدید با موفقیت ذخیره شد: " + userName);
        }


        public SidebarViewModel GetInfoForSidebar(string userName)
        {
            var user = _context.Users.FirstOrDefault(u => u.userName == userName);
            if (user == null)
            {
                throw new Exception("کاربر با مشخصات وارد شده، یافت نشد!");
            }
            return new SidebarViewModel
            {
                userName = user.userName,
                userEmail = user.userEmail,
                userProfile = user.userProfile ?? "profile.png"
            };
        }

        public EditUserViewModel GetUserForEdit(int userId)
        {
            return _context.Users.Where(u => u.userId == userId)
                .Select(u => new EditUserViewModel
                {
                    userId = u.userId,
                    userName = u.userName,
                    userEmail = u.userEmail,
                    avatarName = u.userProfile
                })
                .SingleOrDefault();
        }


        public void DeteleUserAccountFromSite(string userName)
        {
            var user = _context.Users.FirstOrDefault(u => u.userName == userName);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public string GetUserProfilePicture(string userName)
        {
            var user = _context.Users.FirstOrDefault(u => u.userName == userName);
            return user?.userProfile;
        }

    }
}
