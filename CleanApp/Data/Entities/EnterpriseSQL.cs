namespace CleanApp.Data.Entities
{
    public class EnterpriseSQL
    {
        //key for SQLite purposes
        public int Id { get; set; }

        public string Name { get; set; }
        public string Owner { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string TelephoneNumber { get; set; }
        public string ExtraInfo { get; set; }

        public List<OrderSQL> Orders { get; set; } = new();
    }
}
