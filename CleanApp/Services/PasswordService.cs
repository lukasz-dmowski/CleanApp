using CleanApp.Models;
using Microsoft.AspNetCore.Identity;

namespace CleanApp.Services
{
    public class PasswordService
    {
        private readonly PasswordHasher<User> _passwordHasher;

        public PasswordService()
        {
            _passwordHasher = new PasswordHasher<User>();
        }

        public string HashPassword(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be empty.");
            }

            string passwordHash = _passwordHasher.HashPassword(user, password);

            return passwordHash;
        }

        public bool VerifyPassword(User user, string passwordHash, string password)
        {
            PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(user, passwordHash, password);

            if (result == PasswordVerificationResult.Failed)
            {
                return false;
            }

            return true;
        }
    }
}
