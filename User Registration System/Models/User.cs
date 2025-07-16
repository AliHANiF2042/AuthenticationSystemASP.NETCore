using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Xml.Linq;

namespace User_Registration_System.Models
{
    public class User
    {
        public User()
        {

        }

        [Key]
        public int userId { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string userName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string userEmail { get; set; }

        [Display(Name = "پسورد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [MinLength(10, ErrorMessage = "{0} باید حداقل {1} کاراکتر باشد.")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*[0-9]).*$", ErrorMessage = "پسورد باید ترکیبی از حروف و اعداد باشد.")]
        public string password { get; set; }

        [Display(Name = "پروفایل")]
        public string userProfile { get; set; }

        [Display(Name = "تاریخ ثبت نام")]
        public DateTime registerDate { get; set; }
        public bool isDelete { get; set; }

    }
}
