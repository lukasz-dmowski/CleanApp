using CleanApp.Data;
using CleanApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace CleanApp.Services.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task UpdateStandardUserAsync(string username, string password)
        {
            //there are 2 users: admin and additional user. you can update the additional user
            User? user = await _db.Users.FirstOrDefaultAsync(u => u.IsAdmin == false);

            if (user == null)
                return;

            user.Username = username;
            user.PasswordHash = Hash(password);

            await _db.SaveChangesAsync();
        }

        private static string Hash(string password)
        {
            using SHA256 sha = SHA256.Create();

            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha.ComputeHash(bytes);

            return Convert.ToHexString(hash);
        }
    }
}
