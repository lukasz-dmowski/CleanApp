using System.Collections.ObjectModel;

namespace CleanApp.Models
{
    public class OrderBook
    {
        public ObservableCollection<Order> Orders { get; } = new();

        public void AddOrder(Order order)
        {
            Orders.Add(order);
        }
        public void DeleteOrder(Order order)
        {
            Orders.Remove(order);
        }

        public void UpdateOrder(Order order)
        {
            var existingOrder = Orders.FirstOrDefault(o => o.Id == order.Id);

            if (existingOrder == null)
                return;

            existingOrder.ClientName = order.ClientName;
            existingOrder.TelephoneNumber = order.TelephoneNumber;
            existingOrder.StartDate = order.StartDate;
            existingOrder.DueDate = order.DueDate;
            existingOrder.EndDate = order.EndDate;
            existingOrder.ReadinessStatus = order.ReadinessStatus;
            existingOrder.NotificationStatus = order.NotificationStatus;
            existingOrder.CollectionStatus = order.CollectionStatus;

            existingOrder.Items.Clear();

            foreach (var item in order.Items)
            {
                existingOrder.Items.Add(new Item
                {
                    Id = item.Id,

                    ItemNumber = item.ItemNumber,
                    Price = item.Price,
                    Type = item.Type,
                    ReadinessStatus = item.ReadinessStatus
                });
            }

            existingOrder.UpdateStatus();
        }

        public int GetNextOrderNumber()
        {
            if (Orders.Count == 0)
            {
                return 1;
            }

            int nextNumber = Orders.Max(i => i.OrderNumber) + 1;
            return nextNumber;
        }
    }
}
