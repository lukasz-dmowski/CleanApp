using CleanApp.Commands;
using CleanApp.Enums;
using CleanApp.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CleanApp.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        #region Fields

        private readonly Enterprise _enterprise;

        //action to open vms with a predefined filter
        private readonly Action<OrderFilter, string> _navigateWithFilter;
        private  ObservableCollection<Order> _orders => _enterprise.OrderBook.Orders;

        #endregion

        #region Properties

        private string _searchPhrase;
        public string SearchPhrase
        {
            get => _searchPhrase;
            set
            {
                _searchPhrase = value;
                OnPropertyChanged(nameof(SearchPhrase));
            }
        }

        public IEnumerable<ISeries> ScheduleOrdersSeries { get; }
        public IEnumerable<ISeries> ReadinessOrdersSeries { get; }
        public IEnumerable<ISeries> NotificationOrdersSeries { get; }
        public IEnumerable<ISeries> CollectionOrdersSeries { get; }

        #endregion

        #region Dashboard Statistics

        //properties for pie charts and numbers in dashboard
        public int UncompletedOverdueOrdersCount => _orders.Where(o => o.ScheduleStatus == OrderScheduleStatus.Overdue).Count();
        public int UncompletedTodayOrdersCount => _orders.Where(o => o.ScheduleStatus == OrderScheduleStatus.Today).Count();
        public int UncompletedUpcomingOrdersCount => _orders.Where(o => o.ScheduleStatus == OrderScheduleStatus.Upcoming).Count();

        public int PendingOrdersCount => _orders.Where(o => o.ReadinessStatus == OrderReadinessStatus.Pending).Count();
        public int PartialOrdersCount => _orders.Where(o => o.ReadinessStatus == OrderReadinessStatus.Partial).Count();
        public int CompletedOrdersCount => _orders.Where(o => o.ReadinessStatus == OrderReadinessStatus.Completed &&
                                                              o.NotificationStatus == OrderNotificationStatus.NotNotified).Count();

        public int NotifiedOrdersCount =>
                   _orders.Where(o => o.NotificationStatus == OrderNotificationStatus.Notified &&
                                      o.CollectionStatus == OrderCollectionStatus.NotCollected).Count();
        public int NotNotifiedOrdersCount =>
                   _orders.Where(o => o.NotificationStatus == OrderNotificationStatus.NotNotified).Count();

        public int CollectedOrdersCount =>
                   _orders.Where(o => o.CollectionStatus == OrderCollectionStatus.Collected).Count();
        public int NotCollectedOrdersCount =>
                   _orders.Where(o => o.CollectionStatus == OrderCollectionStatus.NotCollected).Count();

        #endregion

        #region Commands

        public ICommand ShowSearchPhraseOrdersCommand { get; set; }
        public ICommand ShowUncompletedOverdueOrdersCommand { get; set; }
        public ICommand ShowUncompletedTodayOrdersCommand { get; set; }
        public ICommand ShowUncompletedUpcomingOrdersCommand { get; set; }

        public ICommand ShowPendingOrdersCommand { get; set; }
        public ICommand ShowPartialOrdersCommand { get; set; }
        public ICommand ShowCompletedOrdersCommand { get; set; }

        public ICommand ShowNotifiedOrdersCommand { get; set; }
        public ICommand ShowNotNotifiedOrdersCommand { get; set; }

        public ICommand ShowCollectedOrdersCommand { get; set; }
        public ICommand ShowNotCollectedOrdersCommand { get; set; }

        #endregion

        #region Constructor

        public DashboardViewModel(Enterprise enterprise, Action<OrderFilter, string> navigateWithFilter)
        {
            _enterprise = enterprise;
            _navigateWithFilter = navigateWithFilter;

            ShowSearchPhraseOrdersCommand = new RelayCommand(ShowSearchPhraseOrders);

            ShowUncompletedOverdueOrdersCommand = new RelayCommand(ShowOverdueOrders);
            ShowUncompletedTodayOrdersCommand = new RelayCommand(ShowTodayOrders);
            ShowUncompletedUpcomingOrdersCommand = new RelayCommand(ShowDueOrders);

            ShowPendingOrdersCommand = new RelayCommand(ShowPendingOrders);
            ShowPartialOrdersCommand = new RelayCommand(ShowPartialOrders);
            ShowCompletedOrdersCommand = new RelayCommand(ShowCompletedOrders);

            ShowNotifiedOrdersCommand = new RelayCommand(ShowNotifiedOrders);
            ShowNotNotifiedOrdersCommand = new RelayCommand(ShowNotNotifiedOrders);

            ShowCollectedOrdersCommand = new RelayCommand(ShowCollectedOrders);
            ShowNotCollectedOrdersCommand = new RelayCommand(ShowNotCollectedOrders);


            ScheduleOrdersSeries = new ISeries[]
            {
                new PieSeries<double>
                {
                    Name = "overdue",
                    Values = new double[] { UncompletedOverdueOrdersCount },
                    InnerRadius = 40,
                    Pushout = 0,
                    HoverPushout = 0,
                    IsHoverable = false,
                    Fill = new SolidColorPaint(SKColor.Parse("#ffae53"))
                },
                new PieSeries<double>
                {
                    Name = "today",
                    Values = new double[] { UncompletedTodayOrdersCount },
                    InnerRadius = 40,
                    Pushout = 0,
                    HoverPushout = 0,
                    IsHoverable = false,
                    Fill = new SolidColorPaint(SKColor.Parse("#15b3fd"))
                },
                new PieSeries<double>
                {
                    Name = "upcoming",
                    Values = new double[] { UncompletedUpcomingOrdersCount },
                    InnerRadius = 40,
                    Pushout = 0,
                    HoverPushout = 0,
                    IsHoverable = false,
                    Fill = new SolidColorPaint(SKColor.Parse("#36deaf"))
                }
            };

            ReadinessOrdersSeries = new ISeries[]
            {
                new PieSeries<double>
                {
                    Name = "pending",
                    Values = new double[] { PendingOrdersCount },
                    InnerRadius = 40,
                    Pushout = 0,
                    HoverPushout = 0,
                    IsHoverable = false,
                    Fill = new SolidColorPaint(SKColor.Parse("#ffae53"))
                },
                new PieSeries<double>
                {
                    Name = "partial",
                    Values = new double[] { PartialOrdersCount },
                    InnerRadius = 40,
                    Pushout = 0,
                    HoverPushout = 0,
                    IsHoverable = false,
                    Fill = new SolidColorPaint(SKColor.Parse("#15b3fd"))
                },
                new PieSeries<double>
                {
                    Name = "completed",
                    Values = new double[] { CompletedOrdersCount },
                    InnerRadius = 40,
                    Pushout = 0,
                    HoverPushout = 0,
                    IsHoverable = false,
                    Fill = new SolidColorPaint(SKColor.Parse("#36deaf"))
                }
            };

            NotificationOrdersSeries = new ISeries[]
            {
                new PieSeries<double>
                {
                    Name = "notified",
                    Values = new double[] { NotifiedOrdersCount },
                    InnerRadius = 40,
                    Pushout = 0,
                    HoverPushout = 0,
                    IsHoverable = false,
                    Fill = new SolidColorPaint(SKColor.Parse("#36deaf"))
                },
                new PieSeries<double>
                {
                    Name = "not notified",
                    Values = new double[] { NotNotifiedOrdersCount },
                    InnerRadius = 40,
                    Pushout = 0,
                    HoverPushout = 0,
                    IsHoverable = false,
                    Fill = new SolidColorPaint(SKColor.Parse("#ffae53"))
                },
            };

            CollectionOrdersSeries = new ISeries[]
            {
                new PieSeries<double>
                {
                    Name = "collected",
                    Values = new double[] { CollectedOrdersCount },
                    InnerRadius = 40,
                    Pushout = 0,
                    HoverPushout = 0,
                    IsHoverable = false,
                    Fill = new SolidColorPaint(SKColor.Parse("#36deaf"))
                },
                new PieSeries<double>
                {
                    Name = "not collected",
                    Values = new double[] { NotCollectedOrdersCount },
                    InnerRadius = 40,
                    Pushout = 0,
                    HoverPushout = 0,
                    IsHoverable = false,
                    Fill = new SolidColorPaint(SKColor.Parse("#ffae53"))
                },
            };
        }

        #endregion

        #region Navigation Methods

        private void ShowSearchPhraseOrders() => _navigateWithFilter(OrderFilter.SearchPhrase, SearchPhrase);

        private void ShowOverdueOrders() => _navigateWithFilter(OrderFilter.Overdue, SearchPhrase);
        private void ShowDueOrders() => _navigateWithFilter(OrderFilter.Upcoming, SearchPhrase);
        private void ShowTodayOrders() => _navigateWithFilter(OrderFilter.Today, SearchPhrase);

        private void ShowPendingOrders() => _navigateWithFilter(OrderFilter.Pending, SearchPhrase);
        private void ShowPartialOrders() => _navigateWithFilter(OrderFilter.Partial, SearchPhrase);
        private void ShowCompletedOrders() => _navigateWithFilter(OrderFilter.Completed, SearchPhrase);

        private void ShowNotifiedOrders() => _navigateWithFilter(OrderFilter.Notified, SearchPhrase);
        private void ShowNotNotifiedOrders() => _navigateWithFilter(OrderFilter.NotNotified, SearchPhrase);

        private void ShowCollectedOrders() => _navigateWithFilter(OrderFilter.Collected, SearchPhrase);
        private void ShowNotCollectedOrders() => _navigateWithFilter(OrderFilter.NotCollected, SearchPhrase);

        #endregion
    }
}
