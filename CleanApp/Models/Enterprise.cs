namespace CleanApp.Models
{
    public class Enterprise
    {
        #region Properties

        public int Id { get; set; }

        public string Name { get; set; }
        public string Owner { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string TelephoneNumber { get; set; }
        public string ExtraInfo { get; set; }

        public OrderBook OrderBook { get; }

        #endregion

        #region Constructors

        public Enterprise(string name, string owner, string street, string buildingNumber, string city, string postalCode, string telephoneNumber, string extraInfo, OrderBook orderBook)
        {
            Name = name;
            Owner = owner;
            Street = street;
            BuildingNumber = buildingNumber;
            City = city;
            PostalCode = postalCode;
            TelephoneNumber = telephoneNumber;
            ExtraInfo = extraInfo;
            OrderBook = orderBook;
        }
        public Enterprise()
        {
            
        }

        #endregion

        #region Methods

        //creates a shallow copy used for editing without modifying original data
        public Enterprise Clone()
        {
            return new Enterprise
            {
                Id = this.Id,

                Name = this.Name,
                Owner = this.Owner,
                Street = this.Street,
                BuildingNumber = this.BuildingNumber,
                City = this.City,
                PostalCode = this.PostalCode,
                TelephoneNumber = this.TelephoneNumber,
                ExtraInfo = this.ExtraInfo
            };
        }
        #endregion
    }
}
