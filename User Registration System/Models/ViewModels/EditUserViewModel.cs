namespace User_Registration_System.Models.ViewModels
{
    public class EditUserViewModel
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public string userEmail { get; set; }
        public string password { get; set; }
        public IFormFile userProfile { get; set; }
        public string avatarName { get; set; }
        public List<int> userAccess { get; set; } = new List<int>();
    }
}
