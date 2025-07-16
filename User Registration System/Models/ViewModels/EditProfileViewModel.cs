using System.ComponentModel.DataAnnotations;

namespace User_Registration_System.Models.ViewModels
{
    public class EditProfileViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی‌تواند بیش‌تر از {1} کاراکتر باشد")]
        public string userName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی‌تواند بیش‌تر از {1} کاراکتر باشد")]
        [EmailAddress(ErrorMessage = "فرمت ایمیل نامعتبر است")]
        public string userEmail { get; set; }

        public IFormFile userProfile { get; set; }
        public string avatarName { get; set; }
    }
}
