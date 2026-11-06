using CleanApp.Data.Entities;
using CleanApp.Models;
using CleanApp.Services.Mappers;
using System.Security.Cryptography;
using System.Text;

namespace CleanApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext db)
        {
            db.Database.EnsureCreated();

            //seeding users
            //on first program launch
            SeedUsers(db);

            //sseding enterprise + orders + items
            //on first program launch
            //generates sample orders and items for learning purposes
            //sample orders and items are not created again, after they are deleted
            SeedEnterprise(db);

            db.SaveChanges();
        }


        private static void SeedUsers(AppDbContext db)
        {
            if (db.Users.Any())
                return;

            //creating admin account, with fixed name and password
            //login is possible at anytime
            db.Users.Add(new User
            {
                Username = "admin",
                PasswordHash = Hash("admin"),
                IsAdmin = true
            });

            //creating user account, with name and password that can be changed in settings tab
            //login is possible, if user knows the name and password
            db.Users.Add(new User
            {
                Username = "user",
                PasswordHash = Hash("user"),
                IsAdmin = false
            });
        }


        private static void SeedEnterprise(AppDbContext db)
        {
            if (db.Enterprises.Any())
            {
                return;
            }

            //generating sample order records
            OrderBook seedOrderBook = new OrderBook();
            OrderBookGenerator.GenerateOrders(seedOrderBook);

            //creating sample enterprise data
            Enterprise enterprise = new Enterprise("Default Name", "Default Name", "Default street", "00/00", "Default Town", "00-000", "123 456 789", "defaultwebsite.com", seedOrderBook);
            EnterpriseSQL enterpriseSql = EnterpriseMapper.ToSQL(enterprise);

            //adding generated orders with items (in form of orderbook) to enterprise
            foreach (Order order in seedOrderBook.Orders)
            {
                OrderSQL orderSql = OrderMapper.ToSQL(order);

                enterpriseSql.Orders.Add(orderSql);
            }

            db.Enterprises.Add(enterpriseSql);
        }


        private static string Hash(string password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            using SHA256 sha = SHA256.Create();

            byte[] passwordBytes =
            Encoding.UTF8.GetBytes(password);

            byte[] hashBytes =
            sha.ComputeHash(passwordBytes);

            return Convert.ToHexString(hashBytes);
        }
    }
}
