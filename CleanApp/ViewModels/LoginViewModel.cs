using CleanApp.Commands;
using CleanApp.Interfaces;
using CleanApp.Models;
using CleanApp.Services;
using CleanApp.Stores;
using System.Windows.Input;

namespace CleanApp.ViewModels
{

    public class LoginViewModel : ViewModelBase, IWindowStateEvents
    {
        #region Fields

        private readonly NavigationStore _mainNavigationStore;
        private readonly ViewModelFactory _viewModelFactory;
        private readonly Enterprise _enterprise;
        private readonly IAuthenticationService _authenticationService;

        private readonly NavigationService _toShellNavigationService;

        #endregion 

        #region Window Events

        public event Action RequestMinimize;
        public event Action RequestMaximize;
        public event Action RequestDrag;
        public event Action RequestClose;

        #endregion 

        #region Properties

        public string EnterpriseName => _enterprise.Name;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private bool _isAuthenticationInvalid;
        public bool IsAuthenticationInvalid
        {
            get => _isAuthenticationInvalid;
            set
            {
                _isAuthenticationInvalid = value;
                OnPropertyChanged(nameof(IsAuthenticationInvalid));
            }
        }

        #endregion 

        #region Commands

        public ICommand LoginCommand { get; }
        public ICommand MinimizeWindowCommand { get; }
        public ICommand MaximizeWindowCommand { get; }
        public ICommand DragWindowCommand { get; }
        public ICommand ExitCommand { get; }

        #endregion 

        #region Constructor

        public LoginViewModel(NavigationStore mainNavigationStore, ViewModelFactory viewModelFactory, Enterprise enterprise, IAuthenticationService authenticationService)
        {
            _mainNavigationStore = mainNavigationStore;
            _viewModelFactory = viewModelFactory;
            _enterprise = enterprise;
            _authenticationService = authenticationService;

            _toShellNavigationService = new NavigationService(mainNavigationStore, () => viewModelFactory.CreateShell(_mainNavigationStore, _viewModelFactory, _enterprise));

            LoginCommand = new RelayCommand(Login, CanLogin);

            DragWindowCommand = new RelayCommand(DragWindow);
            MinimizeWindowCommand = new RelayCommand(MinimizeWindow);
            MaximizeWindowCommand = new RelayCommand(MaximizeWindow);
            ExitCommand = new RelayCommand(Exit);
        }

        #endregion 

        #region Methods

        private void Login()
        {
            if(_authenticationService.Authenticate(Username, Password) == false)
            {
                IsAuthenticationInvalid = true;
                return;
            }
            IsAuthenticationInvalid = false;
            _toShellNavigationService.Navigate();
        }

        private bool CanLogin()
        {
            if (string.IsNullOrEmpty(Username) ||
                string.IsNullOrEmpty(Password))
            {
                return false;
            }
            return true;
        }

        #endregion 

        #region Window Action Methods

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
        private void Exit()
        {
            RequestClose?.Invoke();
        }

        #endregion
    }
}
