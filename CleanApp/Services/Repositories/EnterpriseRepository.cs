using CleanApp.Data;
using CleanApp.Data.Entities;
using CleanApp.Models;
using CleanApp.Services.Mappers;
using Microsoft.EntityFrameworkCore;

namespace CleanApp.Services.Repositories
{
    public class EnterpriseRepository
    {
        private readonly AppDbContext _db;

        public EnterpriseRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Enterprise?> LoadAsync()
        {
            //loading the whole "tree" structure from database: 1 enterprise -> N orders, 1 order -> N items
            EnterpriseSQL? enterpriseSql = await _db.Enterprises.AsNoTracking().Include(e => e.Orders).ThenInclude(o => o.Items).FirstOrDefaultAsync();

            if (enterpriseSql == null)
                return null;


            //loading orders from orderbook, which is one of enterprise's property
            OrderBook orderBook = new OrderBook();

            foreach (OrderSQL orderSql in enterpriseSql.Orders)
            {
                Order order = OrderMapper.FromSQL(orderSql);

                orderBook.Orders.Add(order);
            }

            //loading enterprise data
            Enterprise enterprise = EnterpriseMapper.FromSQL(enterpriseSql, orderBook);

            return enterprise;
        }

        public async Task UpdateAsync(Enterprise enterprise)
        {
            //finding enterprise by ID
            EnterpriseSQL? enterpriseSql = await _db.Enterprises.FirstOrDefaultAsync(e => e.Id == enterprise.Id);

            if (enterpriseSql == null)
                return;

            //changing enterprise data
            enterpriseSql.Name = enterprise.Name;
            enterpriseSql.Owner = enterprise.Owner;
            enterpriseSql.Street = enterprise.Street;
            enterpriseSql.BuildingNumber = enterprise.BuildingNumber;
            enterpriseSql.City = enterprise.City;
            enterpriseSql.PostalCode = enterprise.PostalCode;
            enterpriseSql.TelephoneNumber = enterprise.TelephoneNumber;
            enterpriseSql.ExtraInfo = enterprise.ExtraInfo;

            await _db.SaveChangesAsync();
        }
    }
}
