using Microsoft.CodeAnalysis.Scripting;
using System.Security.Cryptography;
using System.Text;

namespace User_Registration_System.Security
{
    public static class PasswordHash
    {
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password), "پسورد نمی‌تواند خالی باشد.");
            }
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string storedPassword)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(storedPassword))
            {
                return false;
            }

            if (storedPassword.StartsWith("$2a$") ||
                storedPassword.StartsWith("$2b$") ||
                storedPassword.StartsWith("$2y$"))
            {
                return BCrypt.Net.BCrypt.Verify(password, storedPassword);
            }

            return password == storedPassword;
        }
    }
}
