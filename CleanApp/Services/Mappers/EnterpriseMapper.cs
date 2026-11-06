using CleanApp.Data.Entities;
using CleanApp.Models;

namespace CleanApp.Services.Mappers
{
    public static class EnterpriseMapper
    {
        public static EnterpriseSQL ToSQL(Enterprise enterprise)
        {
            return new EnterpriseSQL
            {
                Id = enterprise.Id,

                Name = enterprise.Name,
                Owner = enterprise.Owner,
                Street = enterprise.Street,
                BuildingNumber = enterprise.BuildingNumber,
                City = enterprise.City,
                PostalCode = enterprise.PostalCode,
                TelephoneNumber = enterprise.TelephoneNumber,
                ExtraInfo = enterprise.ExtraInfo
            };
        }

        public static Enterprise FromSQL(EnterpriseSQL enterpriseSql, OrderBook orderBook)
        {
            Enterprise enterprise = new Enterprise(
                enterpriseSql.Name,
                enterpriseSql.Owner,
                enterpriseSql.Street,
                enterpriseSql.BuildingNumber,
                enterpriseSql.City,
                enterpriseSql.PostalCode,
                enterpriseSql.TelephoneNumber,
                enterpriseSql.ExtraInfo,
                orderBook);

            //assignign enterprise ID from database
            enterprise.Id = enterpriseSql.Id;

            return enterprise;
        }
    }
}
