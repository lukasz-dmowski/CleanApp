using CleanApp.Commands;
using CleanApp.Enums;
using CleanApp.Interfaces;
using CleanApp.Models;
using CleanApp.Services;
using CleanApp.Stores;
using System.Windows.Input;

namespace CleanApp.ViewModels
{
    public class ShellViewModel : ViewModelBase, IWindowStateEvents
    {
        #region Fields

        private readonly NavigationStore _mainNavigationStore;
        private readonly NavigationStore _shellNavigationStore;
        private readonly Enterprise _enterprise;

        private readonly ViewModelFactory _viewModelFactory;

        #endregion

        #region Naviagion Services

        private readonly NavigationService _toLoginNavigationService;
        private readonly NavigationService _toManageInfoNavigationService;
        private readonly NavigationService _toSettingsNavigationService;
        private readonly NavigationService _toDashboardNavigationService;
        private readonly NavigationService _toAddOrderNavigationService;
        private readonly NavigationService _toViewOrdersNavigationService;

        #endregion

        #region Window Events

        public event Action RequestMinimize;
        public event Action RequestMaximize;
        public event Action RequestDrag;
        public event Action RequestClose;

        #endregion

        #region Ribbon Buttons Properties

        private bool _isDashboardDisplayed;
        public bool IsDashboardDisplayed
        {
            get
            {
                return _isDashboardDisplayed;
            }
            set
            {
                _isDashboardDisplayed = value;
                if (value == true)
                {
                    IsAddOrderDisplayed = false;
                    IsViewOrdersDisplayed = false;
                    IsInfoDisplayed = false;
                    IsSettingsDisplayed = false;
                }
                OnPropertyChanged(nameof(IsDashboardDisplayed));
            }
        }
        private bool _isAddOrderDisplayed;
        public bool IsAddOrderDisplayed
        {
            get
            {
                return _isAddOrderDisplayed;
            }
            set
            {
                _isAddOrderDisplayed = value;
                if (value == true)
                {
                    IsDashboardDisplayed = false;
                    IsViewOrdersDisplayed = false;
                    IsInfoDisplayed = false;
                    IsSettingsDisplayed = false;
                }
                OnPropertyChanged(nameof(IsAddOrderDisplayed));
            }
        }
        private bool _isViewOrdersDisplayed;
        public bool IsViewOrdersDisplayed
        {
            get
            {
                return _isViewOrdersDisplayed;
            }
            set
            {
                _isViewOrdersDisplayed = value;
                if (value == true)
                {
                    IsDashboardDisplayed = false;
                    IsAddOrderDisplayed = false;
                    IsInfoDisplayed =false;
                    IsSettingsDisplayed = false;
                }
                OnPropertyChanged(nameof(IsViewOrdersDisplayed));
            }
        }

        private bool _isInfoDisplayed;
        public bool IsInfoDisplayed
        {
            get
            {
                return _isInfoDisplayed;
            }
            set
            {
                _isInfoDisplayed = value;
                if (value == true)
                {
                    IsDashboardDisplayed = false;
                    IsAddOrderDisplayed = false;
                    IsViewOrdersDisplayed = false;
                    IsSettingsDisplayed = false;
                }
                OnPropertyChanged(nameof(IsInfoDisplayed));
            }
        }

        private bool _isSettingsDisplayed;
        public bool IsSettingsDisplayed
        {
            get
            {
                return _isSettingsDisplayed;
            }
            set
            {
                _isSettingsDisplayed = value;
                if (value == true)
                {
                    IsDashboardDisplayed = false;
                    IsAddOrderDisplayed = false;
                    IsViewOrdersDisplayed =false;
                    IsInfoDisplayed = false;
                }
                OnPropertyChanged(nameof(IsSettingsDisplayed));
            }
        }

        #endregion

        #region Properties

        public ViewModelBase ShellCurrentViewModel => _shellNavigationStore.CurrentViewModel;

        #endregion

        #region Commands

        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowAddOrderCommand { get; }
        public ICommand ShowViewOrdersCommand { get; }

        public ICommand LogoutCommand { get; }
        public ICommand ShowInfoCommand { get; }
        public ICommand ShowSettingsCommand { get; }

        public ICommand MinimizeWindowCommand { get; }
        public ICommand MaximizeWindowCommand { get; }
        public ICommand DragWindowCommand { get; }
        public ICommand ExitCommand { get; }

        #endregion

        #region Constructor

        public ShellViewModel(NavigationStore mainNavigationStore, ViewModelFactory viewModelFactory, Enterprise enterprise)
        {
            _mainNavigationStore = mainNavigationStore;
            _viewModelFactory = viewModelFactory;
            _enterprise = enterprise;

            _shellNavigationStore = new NavigationStore();

            _toDashboardNavigationService = new NavigationService(_shellNavigationStore, () => viewModelFactory.CreateDashboard(_enterprise, NavigateToOrdersUsingFilter));
            _toAddOrderNavigationService = new NavigationService(_shellNavigationStore, () => viewModelFactory.CreateAddOrder(_enterprise));
            _toViewOrdersNavigationService = new NavigationService(_shellNavigationStore, () => viewModelFactory.CreateViewOrders(_enterprise));

            _toLoginNavigationService = new NavigationService(_mainNavigationStore, () => viewModelFactory.CreateLogin(_mainNavigationStore, _viewModelFactory, _enterprise));
            _toManageInfoNavigationService = new NavigationService(_shellNavigationStore, () => viewModelFactory.CreateManageInfo(_enterprise));
            _toSettingsNavigationService = new NavigationService(_shellNavigationStore, () => viewModelFactory.CreateSettings());

            _toDashboardNavigationService.Navigate();
            IsDashboardDisplayed = true;

            _shellNavigationStore.CurrentViewModelChanged += () => OnPropertyChanged(nameof(ShellCurrentViewModel));

            ShowDashboardCommand = new RelayCommand(ShowDashboard);
            ShowAddOrderCommand = new RelayCommand(ShowAddOrder);
            ShowViewOrdersCommand = new RelayCommand(ShowViewOrders);

            LogoutCommand = new RelayCommand(Logout);
            ShowInfoCommand = new RelayCommand(ShowInfo);
            ShowSettingsCommand = new RelayCommand(ShowSettings);

            MinimizeWindowCommand = new RelayCommand(MinimizeWindow);
            MaximizeWindowCommand = new RelayCommand(MaximizeWindow);
            DragWindowCommand = new RelayCommand(DragWindow);
            ExitCommand = new RelayCommand(Exit);

        }

        #endregion

        #region Navigation Methods

        private void ShowDashboard()
        {
            _toDashboardNavigationService.Navigate();
        }
        private void ShowAddOrder()
        {
            _toAddOrderNavigationService.Navigate();
        }
        private void ShowViewOrders()
        {
            _toViewOrdersNavigationService.Navigate();
        }
        private void Logout()
        {
            _toLoginNavigationService.Navigate();
        }
        private void ShowInfo()
        {
            _toManageInfoNavigationService.Navigate();
        }
        private void ShowSettings()
        {
            _toSettingsNavigationService.Navigate();
        }
        public void NavigateToOrdersUsingFilter(OrderFilter filter, string searchPhrase)
        {
            var vm = _viewModelFactory.CreateViewOrders(_enterprise);

            vm.SearchPhrase = searchPhrase;

            var presetService = new FilterPresetService();
            presetService.ApplyPreset(vm, filter);

            _shellNavigationStore.CurrentViewModel = vm;
            IsViewOrdersDisplayed = true;
        }

        #endregion

        #region Window Actions Methods

        private void MinimizeWindow()
        {
            RequestMinimize?.Invoke();
        }
        private void MaximizeWindow()
        {
            RequestMaximize?.Invoke();
        }
        private void DragWindow()
        {
            RequestDrag?.Invoke();
        }

        #endregion

        #region Session Methods

        private void Exit()
        {
            RequestClose?.Invoke();
        }

        #endregion
    }
}
