using CleanApp.Enums;

namespace CleanApp.Models
{
    public class Item : ModelBase
    {

        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private int _itemNumber;
        public int ItemNumber
        {
            get => _itemNumber;
            set
            {
                _itemNumber = value;
                OnPropertyChanged(nameof(ItemNumber));
            }
        }


        private int? _price;
        public int? Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        private ItemType _type;
        public ItemType Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        private ItemReadinessStatus _readinessStatus;
        public ItemReadinessStatus ReadinessStatus
        {
            get => _readinessStatus;
            set
            {
                _readinessStatus = value;
                OnPropertyChanged(nameof(ReadinessStatus));
            }
        }
        public Item Clone()
        {
            return new Item
            {
                Id = this.Id,
                ItemNumber = this.ItemNumber,
                Price = this.Price,
                Type = this.Type,
                ReadinessStatus = this.ReadinessStatus
            };
        }

        public static Item CreateNewPending(int number)
        {
            return new Item
            {
                ItemNumber = number,
                Price = null,
                Type = ItemType.None,
                ReadinessStatus = ItemReadinessStatus.Pending
            };
        }
        public static Item Reset()
        {
            return new Item
            {
                Price = null,
                Type = ItemType.None,
                ReadinessStatus = ItemReadinessStatus.None
            };
        }
    }
}
