using CleanApp.Commands;
using CleanApp.Enums;
using CleanApp.Helpers;
using CleanApp.Models;
using CleanApp.Services;
using CleanApp.Services.Repositories;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace CleanApp.ViewModels
{
    public class ViewOrdersViewModel : ViewModelBase
    {
        #region Fields

        private readonly Enterprise _enterprise;
        private readonly OrderRepository _orderRepository;
        private readonly PaginationService<Order> _paginationService = new();
        private readonly DialogService _dialogService;
        private readonly PdfService _pdfService;

        #endregion

        #region Collections

        public ObservableCollection<Order> Orders => _enterprise.OrderBook.Orders;
        public ObservableCollection<Order> FilteredOrders { get; } = new();

        private List<FilterFlag> _allFilterFlags;
        public IEnumerable<FilterFlag> AllFilters => _allFilterFlags;

        public IEnumerable<ItemType> ItemTypes { get; }
        public IEnumerable<ItemReadinessStatus> ItemReadinessStatuses { get; }

        #endregion

        #region Properties

        private Order _selectedOrder;
        public Order SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;

                if (value == null)
                {
                    EditedOrder = null;
                    _backupOrder = null;
                }
                else
                {
                    //separate copies are required for editing and undo functionality
                    EditedOrder = value.Clone();
                    _backupOrder = value.Clone();

                    EditedOrder.UpdateStatus();
                }
                OnPropertyChanged(nameof(SelectedOrder));
            }
        }

        private Order _editedOrder;
        public Order EditedOrder
        {
            get => _editedOrder;
            set
            {
                _editedOrder = value;
                OnPropertyChanged(nameof(EditedOrder));
            }
        }

        private Order _backupOrder;

        public Item NewItem { get; set; }

        private string _newItemPrice;

        public string NewItemPrice
        {
            get => _newItemPrice;
            set
            {
                _newItemPrice = value;
                OnPropertyChanged(nameof(NewItemPrice));

                if (int.TryParse(value, out int parsed))
                {
                    NewItem.Price = parsed;
                }

                OnPropertyChanged(nameof(NewItem));
            }
        }

        #endregion

        #region Filtering

        //readiness
        public FilterFlag PendingFilter { get; } = new();
        public FilterFlag PartialFilter { get; } = new();
        public FilterFlag CompletedFilter { get; } = new();

        //schedule
        public FilterFlag OverdueFilter { get; } = new();
        public FilterFlag TodayFilter { get; } = new();
        public FilterFlag UpcomingFilter { get; } = new();

        //notification
        public FilterFlag NotifiedFilter { get; } = new();
        public FilterFlag NotNotifiedFilter { get; } = new();

        //collection
        public FilterFlag CollectedFilter { get; } = new();
        public FilterFlag NotCollectedFilter { get; } = new();

        private string _searchPhrase;
        public string SearchPhrase
        {
            get => _searchPhrase;
            set
            {
                _searchPhrase = value;
                OnPropertyChanged(nameof(SearchPhrase));

                _paginationService.ResetPage();
                UpdateOrdersFilter();
            }
        }

        #endregion

        #region Pagination

        public int CurrentPage => _paginationService.CurrentPage;
        public int TotalPages => _paginationService.TotalPages;

        #endregion

        #region Commands

        public ICommand ResetFilterCommand { get; }
        public ICommand FirstPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand LastPageCommand { get; }

        public ICommand UpdateOrderCommand { get; }
        public ICommand DeleteOrderCommand { get; }
        public ICommand UndoChangesCommand { get; }
        public ICommand PrintOrderCommand { get; }

        public ICommand RemoveItemCommand { get; }
        public ICommand AddItemCommand { get; }
        
        public ICommand TestCommand { get; }

        #endregion

        #region Constructor

        public ViewOrdersViewModel(Enterprise enterprise, OrderRepository orderRepository, DialogService dialogService, PdfService pdfService)
        {
            _enterprise = enterprise;
            //SQLite repository
            _orderRepository = orderRepository;
            _dialogService = dialogService;
            _pdfService = pdfService;

            NewItem = new Item()
            {
                Type = ItemType.None,
                ReadinessStatus = ItemReadinessStatus.None
            };

            //collection of all flags to manage order filter
            _allFilterFlags = new()
            {
                PendingFilter,
                PartialFilter,
                CompletedFilter,

                OverdueFilter,
                TodayFilter,
                UpcomingFilter,

                NotifiedFilter,
                NotNotifiedFilter,

                CollectedFilter,
                NotCollectedFilter,
            };

            //item types displayed in combobox are sorted in alphabetical order, no matter the language version
            ItemTypes = EnumTranslationHelper.GetSortedItemTypes();
            //item readiness statuses displayed in combobox are sorted in alphabetical order, no matter the language version
            ItemReadinessStatuses = EnumTranslationHelper.GetSortedItemStatuses();

            ResetFilterCommand = new RelayCommand(ResetFilter);
            FirstPageCommand = new RelayCommand(GoToFirstPage, () => CurrentPage > 1);
            PreviousPageCommand = new RelayCommand(GoToPreviousPage, () => CurrentPage > 1);
            NextPageCommand = new RelayCommand(GoToNextPage, () => CurrentPage < TotalPages);
            LastPageCommand = new RelayCommand(GoToLastPage, () => CurrentPage < TotalPages);

            UpdateOrderCommand = new RelayCommand(async () => await UpdateOrderAsync(), CanUpdateOrder);
            DeleteOrderCommand = new RelayCommand(async () => await DeleteOrderAsync(), () => SelectedOrder != null);
            UndoChangesCommand = new RelayCommand(UndoChanges, CanUndoChanges);
            PrintOrderCommand = new RelayCommand(SaveOrderPdf, () => EditedOrder != null);

            RemoveItemCommand = new RelayCommandWithParam(RemoveItem);
            AddItemCommand = new RelayCommand(AddItem, CanAddItem);            

            ConnectFilterHandlers();
            UpdateOrdersFilter();
        }

        #endregion

        #region Filtering Methods

        private void ConnectFilterHandlers()
        {
            foreach (var filter in _allFilterFlags)
            {
                filter.PropertyChanged += FilterChanged;
            }
        }

        private void FilterChanged(object sender, PropertyChangedEventArgs e)
        {
            _paginationService.ResetPage();
            UpdateDependentFilters();
            UpdateOrdersFilter();
        }

        //there is a filter tree
        //method enables or diasbles filter checkboxes according to this tree
        //ex. schedule checkboxes are disabled if Pending & Partial checkboxes are disabled
        //ex. notification checkboxes are disabled if Completed checkbox is disabled
        //ex. collection checkboxes are disabled if Notified checkbox is disabled

        /// <summary>
        /// Enables or disables dependent filter groups
        /// Example:
        /// Pending/Partial -> Schedule
        /// Completed -> Notification
        /// Notified -> Collection
        /// </summary>
        private void UpdateDependentFilters()
        {
            bool enableSchedule = PendingFilter.IsChecked == true ||
                                  PartialFilter.IsChecked == true;

            OverdueFilter.IsEnabled = enableSchedule;
            TodayFilter.IsEnabled = enableSchedule;
            UpcomingFilter.IsEnabled = enableSchedule;

            
            if (enableSchedule == false)
            {
                OverdueFilter.IsChecked = false;
                TodayFilter.IsChecked = false;
                UpcomingFilter.IsChecked = false;
            }

            bool enableNotification = CompletedFilter.IsChecked == true;

            NotifiedFilter.IsEnabled = enableNotification;
            NotNotifiedFilter.IsEnabled = enableNotification;

            if (enableNotification == false)
            {
                NotifiedFilter.IsChecked = false;
                NotNotifiedFilter.IsChecked = false;
            }

            bool enableCollection = NotifiedFilter.IsChecked == true;

            CollectedFilter.IsEnabled = enableCollection;
            NotCollectedFilter.IsEnabled = enableCollection;

            if (enableCollection == false)
            {
                CollectedFilter.IsChecked = false;
                NotCollectedFilter.IsChecked = false;
            }
        }

        private void UpdateOrdersFilter()
        {
            var filtered = Orders.AsEnumerable();

            var readiness = new List<OrderReadinessStatus>();
            var schedule = new List<OrderScheduleStatus>();
            var notification = new List<OrderNotificationStatus>();
            var collection = new List<OrderCollectionStatus>();

            //readiness status
            if (PendingFilter.IsChecked) readiness.Add(OrderReadinessStatus.Pending);
            if (PartialFilter.IsChecked) readiness.Add(OrderReadinessStatus.Partial);
            if (CompletedFilter.IsChecked) readiness.Add(OrderReadinessStatus.Completed);

            filtered = filtered.Where(o => readiness.Contains(o.ReadinessStatus));

            //schedule status
            if (OverdueFilter.IsChecked) schedule.Add(OrderScheduleStatus.Overdue);
            if (TodayFilter.IsChecked) schedule.Add(OrderScheduleStatus.Today);
            if (UpcomingFilter.IsChecked) schedule.Add(OrderScheduleStatus.Upcoming);
            if (CompletedFilter.IsChecked)
            {
                //completed orders' schedule status is None, so this has to be added
                schedule.Add(OrderScheduleStatus.None);

                //pending and partial orders' notifiaction and collection statuses are None so these have to be added
                notification.Add(OrderNotificationStatus.None);
                collection.Add(OrderCollectionStatus.None);
            }

            filtered = filtered.Where(o => schedule.Contains(o.ScheduleStatus));

            //notifiaction status
            if (NotifiedFilter.IsChecked) notification.Add(OrderNotificationStatus.Notified);
            if (NotNotifiedFilter.IsChecked) notification.Add(OrderNotificationStatus.NotNotified);
            if (PendingFilter.IsChecked == true || 
                PartialFilter.IsChecked == true)
            {
                //pending and partial orders' notifiaction status is None so this has to be added
                notification.Add(OrderNotificationStatus.None);
            }

            filtered = filtered.Where(o => notification.Contains(o.NotificationStatus));

            //cellection status
            if (CollectedFilter.IsChecked) collection.Add(OrderCollectionStatus.Collected);
            if (NotCollectedFilter.IsChecked) collection.Add(OrderCollectionStatus.NotCollected);
            if (PendingFilter.IsChecked == true || 
                PartialFilter.IsChecked == true)
            {
                //pending and partial orders' collection status is None so this has to be added
                collection.Add(OrderCollectionStatus.None);
            }

            filtered = filtered.Where(o => collection.Contains(o.CollectionStatus));

            //serach phrase
            if (string.IsNullOrWhiteSpace(SearchPhrase) == false)
            {
                filtered = filtered
                    .Where(o => o.ClientName.Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            _paginationService.SetSource(filtered);

            FilteredOrders.Clear();

            foreach (var order in _paginationService.GetPage())
            {
                FilteredOrders.Add(order);
            }

            OnPropertyChanged(nameof(CurrentPage));
            OnPropertyChanged(nameof(TotalPages));
        }

        private void ResetFilter()
        {
            foreach (var filter in _allFilterFlags)
            {
                filter.IsChecked = true;
                filter.IsEnabled = true;
            }

            SearchPhrase = string.Empty;
            UpdateOrdersFilter();
        }

        #endregion

        #region Pagination Methods

        private void GoToFirstPage()
        {
            _paginationService.FirstPage();
            UpdateOrdersFilter();
        }

        private void GoToPreviousPage()
        {
            _paginationService.PrevPage();
            UpdateOrdersFilter();
        }

        private void GoToNextPage()
        {
            _paginationService.NextPage();
            UpdateOrdersFilter();
        }

        private void GoToLastPage()
        {
            _paginationService.LastPage();
            UpdateOrdersFilter();
        }

        #endregion

        #region Order Methods

        private async Task UpdateOrderAsync()
        {
            if (EditedOrder == null || 
                SelectedOrder == null)
            {
                return;
            }

            EditedOrder.UpdateStatus();

            await _orderRepository.UpdateAsync(EditedOrder);
            _enterprise.OrderBook.UpdateOrder(EditedOrder);

            UpdateOrdersFilter();
            OnPropertyChanged(nameof(SelectedOrder));
        }

        private bool CanUpdateOrder()
        {
            if (EditedOrder == null ||
                string.IsNullOrWhiteSpace(EditedOrder.ClientName) ||
                string.IsNullOrWhiteSpace(EditedOrder.TelephoneNumber) ||
                EditedOrder.StartDate == null ||
                EditedOrder.DueDate == null)
                return false;

            //every item must be fully configured before order can be saved
            if (EditedOrder.Items.Any(i => i.Price == null) ||
                EditedOrder.Items.Any(i => i.Type == ItemType.None) ||
                EditedOrder.Items.Any(i => i.ReadinessStatus == ItemReadinessStatus.None))
                return false;

            if (SelectedOrder.IsEqualTo(EditedOrder))
                return false;

            return true;
        }

        private async Task DeleteOrderAsync()
        {
            if (SelectedOrder == null)
                return;

            //confirmation window
            bool deleteOrder = await _dialogService.AskDeleteOrder();

            if (deleteOrder == false)
                return;

            //first - deleting order from database
            await _orderRepository.DeleteAsync(SelectedOrder);
            //second - deleting order from ui
            _enterprise.OrderBook.DeleteOrder(SelectedOrder);

            SelectedOrder = null;
            UpdateOrdersFilter();
        }
        private void UndoChanges()
        {
            // restore original state before editing
            EditedOrder = _backupOrder.Clone();

            EditedOrder.UpdateStatus();

            OnPropertyChanged(nameof(EditedOrder));
        }

        private bool CanUndoChanges()
        {
            if (SelectedOrder == null || 
                EditedOrder == null || 
                _backupOrder == null)
            {
                return false;
            }

            return !SelectedOrder.IsEqualTo(EditedOrder);
        }


        private void SaveOrderPdf()
        {
            _pdfService.SaveOrderPdf(_enterprise, EditedOrder);
        }

        #endregion

        #region Item Methods

        private void RemoveItem(object parameter)
        {
            if (parameter is Item item)
            {
                EditedOrder.Items.Remove(item);
                EditedOrder.UpdateStatus();
                OnPropertyChanged(nameof(EditedOrder));
            }
        }

        private void AddItem()
        {
            var itemToAdd = new Item()
            {
                ItemNumber = EditedOrder.GetNextItemNumber(),
                Price = NewItem.Price,
                Type = NewItem.Type,
                ReadinessStatus = NewItem.ReadinessStatus,
            };

            EditedOrder.Items.Add(itemToAdd);
            EditedOrder.UpdateStatus();

            NewItemPrice = string.Empty;
            NewItem = Item.Reset();

            OnPropertyChanged(nameof(NewItem));
            CommandManager.InvalidateRequerySuggested();
        }

        private bool CanAddItem()
        {
            if (EditedOrder == null)
                return false;

            if (NewItem.Price == 0 ||
                NewItem.Price == null ||
                NewItem.Type == ItemType.None ||
                NewItem.ReadinessStatus == ItemReadinessStatus.None)
                return false;

            return true;
        }

        #endregion
    }
}
