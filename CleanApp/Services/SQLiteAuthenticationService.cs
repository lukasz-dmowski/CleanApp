using CleanApp.Data;
using CleanApp.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace CleanApp.Services
{
    public class SQLiteAuthenticationService : IAuthenticationService
    {
        private readonly AppDbContext _db;

        public SQLiteAuthenticationService(AppDbContext db)
        {
            _db = db;
        }

        public bool Authenticate(string username, string password)
        {
            string hash = Hash(password);

            return _db.Users.Any(u =>
                u.Username == username &&
                u.PasswordHash == hash);
        }

        public bool IsUsernameValid(string username)
        {
            return _db.Users.Any(u => u.Username == username);
        }

        public bool IsPasswordValid(string password)
        {
            string hash = Hash(password);
            return _db.Users.Any(u => u.PasswordHash == hash);
        }

        private static string Hash(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes);
        }
    }
}
