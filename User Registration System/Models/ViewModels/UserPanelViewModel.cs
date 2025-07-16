using System.ComponentModel.DataAnnotations;

namespace User_Registration_System.Models.ViewModels
{
    public class UserPanelViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string userName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [EmailAddress]
        public string userEmail { get; set; }
        public DateTime registerDate { get; set; }
        public int userWallet { get; set; }

        [Display(Name = "پروفایل")]
        public string userProfile { get; set; }
        public EditProfileViewModel editProfile { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Display(Name = "پسورد قدیمی")]
        [Required(ErrorMessage = "پسورد قدیمی را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [MinLength(10, ErrorMessage = "{0} باید حداقل {1} کاراکتر باشد.")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*[0-9]).*$", ErrorMessage = "پسورد باید ترکیبی از حروف و اعداد باشد.")]
        public string oldPassword { get; set; }

        [Display(Name = "پسورد جدید")]
        [Required(ErrorMessage = "پسورد جدید را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [MinLength(10, ErrorMessage = "{0} باید حداقل {1} کاراکتر باشد.")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*[0-9]).*$", ErrorMessage = "پسورد باید ترکیبی از حروف و اعداد باشد.")]
        public string newPassword { get; set; }

        [Display(Name = "تکرار پسورد جدید")]
        [Required(ErrorMessage = "لطفا تکرار پسورد جدید را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [MinLength(10, ErrorMessage = "{0} باید حداقل {1} کاراکتر باشد.")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*[0-9]).*$", ErrorMessage = "پسورد باید ترکیبی از حروف و اعداد باشد.")]
        [Compare("newPassword", ErrorMessage = "پسورد های جدید وارد شده، مطابقت ندارند!")]
        public string rePassword { get; set; }
    }
}
