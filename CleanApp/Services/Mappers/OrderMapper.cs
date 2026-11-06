using CleanApp.Data.Entities;
using CleanApp.Models;
using System.Collections.ObjectModel;

namespace CleanApp.Services.Mappers
{
    public static class OrderMapper
    {
        public static OrderSQL ToSQL(Order order)
        {
            return new OrderSQL
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,

                ClientName = order.ClientName,
                TelephoneNumber = order.TelephoneNumber,
                StartDate = order.StartDate,
                DueDate = order.DueDate,
                EndDate = order.EndDate,

                ScheduleStatus = order.ScheduleStatus,
                ReadinessStatus = order.ReadinessStatus,
                NotificationStatus = order.NotificationStatus,
                CollectionStatus = order.CollectionStatus,

                Items = order.Items.Select(item => new ItemSQL
                {
                    Id = item.Id,
                    ItemNumber = item.ItemNumber,

                    Price = item.Price,
                    Type = item.Type,
                    ReadinessStatus = item.ReadinessStatus
                }).ToList()
            };
        }

        public static Order FromSQL(OrderSQL orderSql)
        {
            return new Order
            {
                Id = orderSql.Id,
                OrderNumber = orderSql.OrderNumber,

                ClientName = orderSql.ClientName,
                TelephoneNumber = orderSql.TelephoneNumber,
                StartDate = orderSql.StartDate,
                DueDate = orderSql.DueDate,
                EndDate = orderSql.EndDate,
                ScheduleStatus = orderSql.ScheduleStatus,
                ReadinessStatus = orderSql.ReadinessStatus,
                NotificationStatus = orderSql.NotificationStatus,
                CollectionStatus = orderSql.CollectionStatus,

                Items = new ObservableCollection<Item>(
                    orderSql.Items.Select(itemSql => new Item
                    {
                        Id = itemSql.Id,
                        ItemNumber = itemSql.ItemNumber,

                        Price = itemSql.Price,
                        Type = itemSql.Type,
                        ReadinessStatus = itemSql.ReadinessStatus
                    })
                )
            };
        }
    }
}
