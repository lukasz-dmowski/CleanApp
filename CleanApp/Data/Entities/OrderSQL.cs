using CleanApp.Enums;

namespace CleanApp.Data.Entities
{
    public class OrderSQL
    {
        //key for SQLite purposes
        public int Id { get; set; }

        //key for UI purposes
        public int OrderNumber { get; set; }

        public int EnterpriseId { get; set; }
        public EnterpriseSQL Enterprise { get; set; } = null!;

        public string ClientName { get; set; }
        public string TelephoneNumber { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? EndDate { get; set; }

        public OrderScheduleStatus ScheduleStatus { get; set; }
        public OrderReadinessStatus ReadinessStatus { get; set; }
        public OrderNotificationStatus NotificationStatus { get; set; }
        public OrderCollectionStatus CollectionStatus { get; set; }

        public List<ItemSQL> Items { get; set; } = new();
    }
}
