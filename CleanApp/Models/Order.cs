using CleanApp.Enums;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace CleanApp.Models
{
    public class Order : ModelBase
    {

        private int _id;
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        private int _orderNumber;
        public int OrderNumber
        {
            get => _orderNumber;
            set { _orderNumber = value; OnPropertyChanged(nameof(OrderNumber)); }
        }

        private string _clientName;
        public string ClientName
        {
            get => _clientName;
            set { _clientName = value; OnPropertyChanged(nameof(ClientName)); }
        }

        private string _telephoneNumber;
        public string TelephoneNumber
        {
            get => _telephoneNumber;
            set { _telephoneNumber = value; OnPropertyChanged(nameof(TelephoneNumber)); }
        }

        private ObservableCollection<Item> _items = new();
        public ObservableCollection<Item> Items
        {
            get => _items;
            set
            {
                if (_items != null)
                {
                    _items.CollectionChanged -= ItemsChanged;
                    foreach (var item in _items)
                        item.PropertyChanged -= ItemChanged;
                }

                _items = value;

                if (_items != null)
                {
                    _items.CollectionChanged += ItemsChanged;
                    foreach (var item in _items)
                        item.PropertyChanged += ItemChanged;
                }

                OnPropertyChanged(nameof(Items));
                OnPropertyChanged(nameof(ItemsCount));
                OnPropertyChanged(nameof(Price));

                UpdateStatus();
            }
        }

        public int ItemsCount => Items?.Count ?? 0;

        public int? Price => Items?.Sum(i => i.Price) ?? 0;

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get => _startDate;
            set { _startDate = value; OnPropertyChanged(nameof(StartDate)); }
        }

        private DateTime? _dueDate;
        public DateTime? DueDate
        {
            get => _dueDate;
            set
            {
                _dueDate = value;
                OnPropertyChanged(nameof(DueDate));
                UpdateStatus();
            }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get => _endDate;
            set { _endDate = value; OnPropertyChanged(nameof(EndDate)); }
        }

        private OrderScheduleStatus _scheduleStatus;
        public OrderScheduleStatus ScheduleStatus
        {
            get => _scheduleStatus;
            set { _scheduleStatus = value; OnPropertyChanged(nameof(ScheduleStatus)); }
        }

        private OrderReadinessStatus _readinessStatus;
        public OrderReadinessStatus ReadinessStatus
        {
            get => _readinessStatus;
            set
            {
                _readinessStatus = value;
                OnPropertyChanged(nameof(ReadinessStatus));
            }
        }

        private OrderNotificationStatus _notificationStatus;
        public OrderNotificationStatus NotificationStatus
        {
            get => _notificationStatus;
            set 
            { 
                _notificationStatus = value; 
                OnPropertyChanged(nameof(NotificationStatus));

                // jeśli odznaczam "Was client notified"
                if (_notificationStatus != OrderNotificationStatus.Notified)
                {
                    // natychmiast odznacz "Was order collected"
                    CollectionStatus = OrderCollectionStatus.None;

                    // natychmiast wyzeruj EndDate
                    EndDate = null;
                }
                else
                {
                    // jeśli zaznaczam "Was client notified"
                    // a CollectionStatus jest None → ustaw NotCollected
                    if (CollectionStatus == OrderCollectionStatus.None)
                        CollectionStatus = OrderCollectionStatus.NotCollected;
                }
            }
        }

        private OrderCollectionStatus _collectionStatus;
        public OrderCollectionStatus CollectionStatus
        {
            get => _collectionStatus;
            set 
            { 
                _collectionStatus = value; 
                OnPropertyChanged(nameof(CollectionStatus));

                // jeśli zaznaczam "Was order collected"
                if (_collectionStatus == OrderCollectionStatus.Collected)
                {
                    // ustaw EndDate tylko jeśli nie było ustawione ręcznie
                    if (EndDate == null)
                        EndDate = DateTime.Now;
                }
                else
                {
                    // jeśli odznaczam "Was order collected"
                    // EndDate musi być null
                    EndDate = null;
                }
            }
        }

        private void ItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (Item item in e.NewItems)
                    item.PropertyChanged += ItemChanged;

            if (e.OldItems != null)
                foreach (Item item in e.OldItems)
                    item.PropertyChanged -= ItemChanged;

            // changing these prop when new item is added or deleted
            OnPropertyChanged(nameof(ItemsCount));
            OnPropertyChanged(nameof(Price));
        }

        private void ItemChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Item.ReadinessStatus))
            {
                UpdateStatus();
            }

            if (e.PropertyName == nameof(Item.Price))
            {
                OnPropertyChanged(nameof(Price));
            }
        }

        public void UpdateStatus()
        {
            if (Items.Count == 0)
            {
                ReadinessStatus = OrderReadinessStatus.Pending;
                NotificationStatus = OrderNotificationStatus.None;
                CollectionStatus = OrderCollectionStatus.None;
                EndDate = null;
                UpdateScheduleStatus();
                return;
            }

            bool allCompleted = Items.All(i => i.ReadinessStatus == ItemReadinessStatus.Completed);
            bool allPending = Items.All(i => i.ReadinessStatus == ItemReadinessStatus.Pending);

            if (allCompleted)
                ReadinessStatus = OrderReadinessStatus.Completed;
            else if (allPending)
                ReadinessStatus = OrderReadinessStatus.Pending;
            else
                ReadinessStatus = OrderReadinessStatus.Partial;

            if (ReadinessStatus == OrderReadinessStatus.Completed)
            {
                if (NotificationStatus == OrderNotificationStatus.None)
                    NotificationStatus = OrderNotificationStatus.NotNotified;
            }
            else
            {
                NotificationStatus = OrderNotificationStatus.None;
            }
                        
            UpdateScheduleStatus();
        }

        private void UpdateScheduleStatus()
        {
            if (ReadinessStatus == OrderReadinessStatus.Completed)
            {
                ScheduleStatus = OrderScheduleStatus.None;
                return;
            }

            var today = DateTime.Today;

            if (DueDate < today)
                ScheduleStatus = OrderScheduleStatus.Overdue;
            else if (DueDate > today)
                ScheduleStatus = OrderScheduleStatus.Upcoming;
            else
                ScheduleStatus = OrderScheduleStatus.Today;
        }

        public Order Clone()
        {
            return new Order
            {
                Id = this.Id,
                OrderNumber = this.OrderNumber,
                ClientName = this.ClientName,
                TelephoneNumber = this.TelephoneNumber,

                Items = new ObservableCollection<Item>(
                    this.Items.Select(i => i.Clone())
                ),

                StartDate = this.StartDate,
                DueDate = this.DueDate,
                EndDate = this.EndDate,

                ScheduleStatus = this.ScheduleStatus,
                ReadinessStatus = this.ReadinessStatus,
                NotificationStatus = this.NotificationStatus,
                CollectionStatus = this.CollectionStatus
            };
        }

        public bool IsEqualTo(Order other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (other == null)
                return false;

            // ===== IDENTYFIKACJA ORDER =====
            if (Id != other.Id) return false;
            if (OrderNumber != other.OrderNumber) return false;

            // ===== DANE ORDER =====
            if (ClientName != other.ClientName) return false;
            if (TelephoneNumber != other.TelephoneNumber) return false;
            if (StartDate != other.StartDate) return false;
            if (DueDate != other.DueDate) return false;
            if (EndDate != other.EndDate) return false;

            if (ScheduleStatus != other.ScheduleStatus) return false;
            if (ReadinessStatus != other.ReadinessStatus) return false;
            if (NotificationStatus != other.NotificationStatus) return false;
            if (CollectionStatus != other.CollectionStatus) return false;

            // ===== ITEMS =====
            if (Items.Count != other.Items.Count)
                return false;

            for (int i = 0; i < Items.Count; i++)
            {
                Item a = Items[i];
                Item b = other.Items[i];

                // ===== IDENTYFIKACJA ITEM =====
                if (a.Id != b.Id) return false;
                if (a.ItemNumber != b.ItemNumber) return false;

                if (a.Price != b.Price) return false;
                if (a.Type != b.Type) return false;
                if (a.ReadinessStatus != b.ReadinessStatus) return false;
            }

            return true;
        }

        public int GetNextItemNumber()
        {
            if (Items.Count == 0)
            {
                return 1;
            } 

            int nextNumber = Items.Max(i => i.ItemNumber) + 1;
            return nextNumber;
        }

        public static Order CreateNew(int nextOrderNumber)
        {
            Order order = new Order
            {
                //Id is set during adding to database
                OrderNumber = nextOrderNumber,
                ClientName = string.Empty,
                TelephoneNumber = string.Empty,
                StartDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7),
                EndDate = null,
                NotificationStatus = OrderNotificationStatus.None,
                CollectionStatus = OrderCollectionStatus.None,

                Items = new ObservableCollection<Item>()
            };

            return order;
        }


    }
}
