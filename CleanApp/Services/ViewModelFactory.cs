using CleanApp.Data.Settings;
using CleanApp.Enums;
using CleanApp.Interfaces;
using CleanApp.Models;
using CleanApp.Services.Repositories;
using CleanApp.Stores;
using CleanApp.ViewModels;

namespace CleanApp.Services
{

    public class ViewModelFactory
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly EnterpriseRepository _enterpriseRepository;
        private readonly OrderRepository _orderRepository;
        private readonly UserRepository _userRepository;
        private readonly AppSettingsService _appSettingsService;
        private readonly AppSettings _appSettings;
        private readonly DialogService _dialogService;
        private readonly PdfService _pdfService;


        public ViewModelFactory(IAuthenticationService authenticationService, 
                                OrderRepository orderRepository,
                                EnterpriseRepository enterpriseRepository,
                                UserRepository userRepository,
                                AppSettingsService appSettingsService,
                                AppSettings appSettings,
                                DialogService dialogService,
                                PdfService pdfService)
        {
            _authenticationService = authenticationService;
            _orderRepository = orderRepository;
            _enterpriseRepository = enterpriseRepository;
            _userRepository = userRepository;
            _appSettingsService = appSettingsService;
            _appSettings = appSettings;
            _dialogService = dialogService;
            _pdfService = pdfService;
        }
        public LoginViewModel CreateLogin(NavigationStore mainNavigationStore, ViewModelFactory viewModelFactory, Enterprise enterprise)
        {
            return new LoginViewModel(mainNavigationStore, this, enterprise, _authenticationService);
        }

        public ShellViewModel CreateShell(NavigationStore mainNavigationStore, ViewModelFactory viewModelFactory, Enterprise enterprise)
        {
            return new ShellViewModel(mainNavigationStore, this, enterprise);
        }

        public InfoViewModel CreateManageInfo(Enterprise enterprise)
        {
            return new InfoViewModel(enterprise, _enterpriseRepository);
        }
        public SettingsViewModel CreateSettings()
        {
            return new SettingsViewModel(_userRepository, _appSettingsService, _appSettings);
        }

        public DashboardViewModel CreateDashboard(Enterprise enterprise, Action<OrderFilter, string> navigateUsingFilter)
        {
            return new DashboardViewModel(enterprise, navigateUsingFilter);
        }

        public AddOrderViewModel CreateAddOrder(Enterprise enterprise)
        {
            return new AddOrderViewModel(enterprise, _orderRepository, _dialogService, _pdfService);
        }
        public ViewOrdersViewModel CreateViewOrders(Enterprise enterprise)
        {
            return new ViewOrdersViewModel(enterprise, _orderRepository, _dialogService, _pdfService);
        }
    }
}
