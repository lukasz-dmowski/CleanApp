using CleanApp.Data;
using CleanApp.Data.Entities;
using CleanApp.Models;
using CleanApp.Services.Mappers;
using Microsoft.EntityFrameworkCore;

namespace CleanApp.Services.Repositories
{
    public class OrderRepository
    {
        private readonly AppDbContext _db;
        private readonly int _enterpriseId;

        public OrderRepository(AppDbContext db, int enterpriseId)
        {
            _db = db;
            _enterpriseId = enterpriseId;
        }

        public async Task<List<Order>> LoadAllAsync()
        {
            //raeding sql orders from database
            List<OrderSQL> ordersSql = await _db.Orders.AsNoTracking().Include(o => o.Items).ToListAsync();

            //changing sql orders list into orders list
            List<Order> orders = ordersSql.Select(OrderMapper.FromSQL).ToList();

            return orders;
        }


        public async Task AddAsync(Order order)
        {
            //changing order into sql order
            OrderSQL orderSql = OrderMapper.ToSQL(order);
            orderSql.EnterpriseId = _enterpriseId;

            //saving sql order in database
            _db.Orders.Add(orderSql);
            await _db.SaveChangesAsync();

            //taking sql order ID from database and assigning it back to order object
            order.Id = orderSql.Id;

            foreach (Item item in order.Items)
            {
                //taking sql item ID from database and assigning it back to item object
                ItemSQL itemSql = orderSql.Items.First(x => x.ItemNumber == item.ItemNumber);
                item.Id = itemSql.Id;
            }
        }


        public async Task UpdateAsync(Order order)
        {
            //finding the slq order in database by ID
            OrderSQL? orderSql = await _db.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == order.Id);

            if (orderSql == null)
                return;

            //replacing sql order properties with order properties 
            UpdateOrderData(orderSql, order);

            //adding and updating items
            UpdateItems(orderSql, order);

            //deleting items
            DeleteItems(orderSql, order);

            await _db.SaveChangesAsync();

            //assigning sql item ID to item
            UpdateItemIds(orderSql, order);
        }


        private void UpdateOrderData(OrderSQL orderSql, Order order)
        {
            orderSql.OrderNumber = order.OrderNumber;
            orderSql.ClientName = order.ClientName;
            orderSql.TelephoneNumber = order.TelephoneNumber;

            orderSql.StartDate = order.StartDate;
            orderSql.DueDate = order.DueDate;
            orderSql.EndDate = order.EndDate;

            orderSql.ScheduleStatus = order.ScheduleStatus;
            orderSql.ReadinessStatus = order.ReadinessStatus;
            orderSql.NotificationStatus = order.NotificationStatus;
            orderSql.CollectionStatus = order.CollectionStatus;
        }

        public async Task DeleteAsync(Order order)
        {
            OrderSQL? orderSql = await _db.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);

            if (orderSql == null)
                return;

            _db.Orders.Remove(orderSql);
            await _db.SaveChangesAsync();
        }


        private void UpdateItems(OrderSQL orderSql, Order order)
        {
            foreach (Item item in order.Items)
            {
                //new item
                //when created, before obtaining ID from database, item ID is null -> 0
                if (item.Id == 0)
                {
                    ItemSQL newItemSql = new ItemSQL
                    {
                        ItemNumber = item.ItemNumber,
                        Price = item.Price,
                        Type = item.Type,
                        ReadinessStatus = item.ReadinessStatus
                    };

                    orderSql.Items.Add(newItemSql);
                }

                //existing item
                //with item ID obtained earlier
                else
                {
                    ItemSQL? itemSql = orderSql.Items.FirstOrDefault(i => i.Id == item.Id);

                    if (itemSql != null)
                    {
                        itemSql.ItemNumber = item.ItemNumber;
                        itemSql.Price = item.Price;
                        itemSql.Type = item.Type;
                        itemSql.ReadinessStatus = item.ReadinessStatus;
                    }
                }
            }
        }


        private void DeleteItems(OrderSQL orderSql, Order order)
        {
            List<ItemSQL> itemsToDelete = new List<ItemSQL>();

            foreach (ItemSQL itemSql in orderSql.Items)
            {
                //skip new sql items, not added to database yet
                if (itemSql.Id == 0)
                {
                    continue;
                }

                bool itemStillExists = false;

                //checking if each item has equivalent sql item 
                foreach (Item item in order.Items)
                {
                    if (item.Id == itemSql.Id)
                    {
                        itemStillExists = true;
                        break;
                    }
                }

                //adding items without its database counterparts to list
                if (itemStillExists == false)
                {
                    itemsToDelete.Add(itemSql);
                }
            }

            //deleting sql items in database
            foreach (ItemSQL itemSql in itemsToDelete)
            {
                _db.Items.Remove(itemSql);
            }
        }


        private void UpdateItemIds(OrderSQL orderSql, Order order)
        {
            //finding new items -> without ID
            //finding item's counterpart in database
            //assigning sql item ID to item
            foreach (Item item in order.Items.Where(i => i.Id == 0))
            {
                ItemSQL itemSql = orderSql.Items.First(i => i.ItemNumber == item.ItemNumber);
                item.Id = itemSql.Id;
            }
        }
    }

}
