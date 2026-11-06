using CleanApp.Data;
using CleanApp.Data.Settings;
using CleanApp.Interfaces;
using CleanApp.Models;
using CleanApp.Resources;
using CleanApp.Services;
using CleanApp.Services.Repositories;
using CleanApp.Stores;
using CleanApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using System.Globalization;
using System.Windows;

namespace CleanApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Enterprise _enterprise;
        private AppDbContext _db;

        private AppSettingsService _appSettingsService;
        private AppSettings _appSettings;
        private DatabaseBackupService _backupService;
        private DialogService _dialogService;
        private PdfService _pdfService;

        protected override async void OnStartup(StartupEventArgs e)
        {
            //type "en" or "pl" in brackets to run the app in appropriate language
            Language.Culture = new CultureInfo("en");
            EnumResources.Culture = new CultureInfo("en");

            base.OnStartup(e);

            DatabasePathProvider.EnsureDirectoryExists();

            //data should be put in local app folder
            string connectionString = $"Data Source={DatabasePathProvider.DatabasePath}";
            var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connectionString).Options;

            _db = new AppDbContext(options);

            //creating app folders in local app data directory 
            _appSettingsService = new AppSettingsService();

            //getting the path of settings json file ( for backup reasons )
            _appSettings = await _appSettingsService.LoadAsync();

            //providing the path to database
            //pointing where the backup files should be stored
            _backupService = new DatabaseBackupService(DatabasePathProvider.DatabasePath, _appSettings);

            //creating enterprise, users, orders, items if they dont exist in db
            DbInitializer.Initialize(_db);

            //repository for actions when app is starting
            EnterpriseRepository enterpriseRepository = new EnterpriseRepository(_db);

            //loading the whole enterprise from db
            _enterprise = await enterpriseRepository.LoadAsync();

            if (_enterprise == null)
            {
                throw new InvalidOperationException("failed to load enterprise");
            }

            //repositories for actions when app is running
            //1 enterprise holds N orders; there is only 1 enterprise in db, queried by its id
            OrderRepository orderRepository = new OrderRepository(_db, _enterprise.Id);
            UserRepository userRepository = new UserRepository(_db);

            //pdf creator
            QuestPDF.Settings.License = LicenseType.Community;

            //dialog windows for info and confirmation
            _dialogService = new DialogService();

            //printing order info 
            _pdfService = new PdfService();

            //users & passwords
            IAuthenticationService authenticationService = new SQLiteAuthenticationService(_db);

            NavigationStore navigationStore =  new NavigationStore();

            //passing globally used dependecies to factory, where viewmodels (-> views) are created 
            ViewModelFactory viewModelFactory = new ViewModelFactory(authenticationService, orderRepository, enterpriseRepository, userRepository, _appSettingsService, _appSettings, _dialogService, _pdfService);

            //setting the startup viewmodel (-> view) for main window
            navigationStore.CurrentViewModel = viewModelFactory.CreateLogin(navigationStore, viewModelFactory, _enterprise);

            MainWindow mainWindow = new MainWindow
            {
                DataContext = new MainViewModel(navigationStore)
            };

            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                //creating backup file on app exit
                _backupService.CreateBackup();
            }
            finally
            {
                _db?.Dispose();

                base.OnExit(e);
            }
        }
    }

}
