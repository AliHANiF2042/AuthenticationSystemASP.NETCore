namespace User_Registration_System.Converts
{
    public class FixedEmail
    {
        public static string FixEmail(string email)
        {
            return email.Trim().ToLower();
        }
    }
}
