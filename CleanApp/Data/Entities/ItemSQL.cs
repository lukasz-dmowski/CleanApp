using CleanApp.Enums;

namespace CleanApp.Data.Entities
{
    public class ItemSQL
    {
        //key for SQLite purposes
        public int Id { get; set; }

        //key for UI purposes
        public int ItemNumber { get; set; }
        public int OrderId { get; set; }
        public OrderSQL Order { get; set; }

        public int? Price { get; set; }
        public ItemType Type { get; set; }
        public ItemReadinessStatus ReadinessStatus { get; set; }
    }
}
