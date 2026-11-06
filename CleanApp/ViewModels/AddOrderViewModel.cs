using CleanApp.Commands;
using CleanApp.Enums;
using CleanApp.Helpers;
using CleanApp.Models;
using CleanApp.Services;
using CleanApp.Services.Repositories;
using System.Windows.Input;

namespace CleanApp.ViewModels
{
    public class AddOrderViewModel : ViewModelBase
    {
        #region Fields

        private readonly Enterprise _enterprise;
        private readonly OrderRepository _orderRepository;
        private readonly DialogService _dialogService;
        private readonly PdfService _pdfService;

        #endregion

        #region Properties

        private Order _newOrder;
        public Order NewOrder
        {
            get => _newOrder;
            set
            {
                _newOrder = value;
                OnPropertyChanged(nameof(NewOrder));
            }
        }

        private Item _newItem;
        public Item NewItem
        {
            get => _newItem;
            set
            {
                _newItem = value;
                OnPropertyChanged(nameof(NewItem));
            }
        }

        public IEnumerable<ItemType> ItemTypes { get; }

        #endregion

        #region Commands

        public ICommand ClearCommand { get; }
        public ICommand AddOrderCommand { get; }
        public ICommand RemoveItemCommand { get; }
        public ICommand AddItemCommand { get; }

        #endregion

        #region Constructor

        public AddOrderViewModel(Enterprise enterprise, OrderRepository orderRepository, DialogService dialogService, PdfService pdfService)
        {
            _enterprise = enterprise;
            _orderRepository = orderRepository;
            _dialogService = dialogService;
            _pdfService = pdfService;

            //item types displayed in combobox are sorted in alphabetical order, no matter the language version
            ItemTypes = EnumTranslationHelper.GetSortedItemTypes();

            //assignig last order's number +1
            NewOrder = Order.CreateNew(_enterprise.OrderBook.GetNextOrderNumber());
            //new item ReadinessStatus is always Pending in this vm
            NewItem = Item.CreateNewPending(NewOrder.GetNextItemNumber());

            ClearCommand = new RelayCommand(Clear, CanClear);
            AddOrderCommand = new RelayCommand(async () => await AddOrderAsync(), CanAddOrder);
            RemoveItemCommand = new RelayCommandWithParam(RemoveItem);
            AddItemCommand = new RelayCommand(AddItem, CanAddItem);
        }

        #endregion

        #region Order Methods

        private void Clear()
        {
            NewOrder = Order.CreateNew(_enterprise.OrderBook.GetNextOrderNumber());
        }
        private bool CanClear()
        {
            if (!string.IsNullOrWhiteSpace(NewOrder.ClientName) ||
                !string.IsNullOrWhiteSpace(NewOrder.TelephoneNumber) ||
                NewOrder.Items?.Count > 0)
            {
                return true;
            }
            return false;
        }

        private async Task AddOrderAsync()
        {
            //save to SQLite datebase
            await _orderRepository.AddAsync(NewOrder);

            //save (update) orders in UI
            _enterprise.OrderBook.AddOrder(NewOrder);

            if (await _dialogService.AskSavePdf() == true)
            {
                _pdfService.SaveOrderPdf(_enterprise, NewOrder);
            }

            //assignig last order's number +1
            NewOrder = Order.CreateNew(_enterprise.OrderBook.GetNextOrderNumber());
            //new item ReadinessStatus is always Pending in this vm
            NewItem = Item.CreateNewPending(NewOrder.GetNextItemNumber());
        }

        private bool CanAddOrder()
        {
            if (NewOrder == null ||
                string.IsNullOrWhiteSpace(NewOrder.ClientName) ||
                string.IsNullOrWhiteSpace(NewOrder.TelephoneNumber) ||
                NewOrder.StartDate == null ||
                NewOrder.DueDate == null)
            {
                return false;
            }
            if (NewOrder.Items.Count == 0)
            {
                return false;
            }
            if (NewOrder.Items.Any(i => i.Price == null) ||
                NewOrder.Items.Any(i => i.Type == ItemType.None))
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Item Methods

        private void AddItem()
        {
            NewOrder.Items.Add(NewItem.Clone());
            //new item ReadinessStatus is always Pending in this vm
            NewItem = Item.CreateNewPending(NewOrder.GetNextItemNumber());
        }

        private bool CanAddItem()
        {
            if (NewOrder == null)
            {
                return false;
            }
            if (NewItem.Price == null ||
                NewItem.Price == 0 ||
                NewItem.Type == ItemType.None)
            {
                return false;
            }
            return true;
        }

        private void RemoveItem(object parameter)
        {
            if (parameter is Item item)
            {
                NewOrder.Items.Remove(item);
            }
        }

        #endregion
    }
}
