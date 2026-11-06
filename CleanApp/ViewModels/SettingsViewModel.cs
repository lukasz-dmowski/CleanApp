using CleanApp.Commands;
using CleanApp.Data;
using CleanApp.Data.Settings;
using CleanApp.Services.Repositories;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;

namespace CleanApp.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        #region Fields

        private readonly AppSettingsService _appSettingsService;
        private readonly AppSettings _appSettings;

        private readonly UserRepository _userRepository;

        #endregion

        #region Events

        //event needed to clear passwordboxes after password is saved
        public event Action? UserDataSaved;

        #endregion

        #region Properties

        private string _username = string.Empty;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
                CommandManager.InvalidateRequerySuggested();
            }
        }


        private string _usernameConfirmation = string.Empty;
        public string UsernameConfirmation
        {
            get => _usernameConfirmation;
            set
            {
                _usernameConfirmation = value;
                OnPropertyChanged(nameof(UsernameConfirmation));
                CommandManager.InvalidateRequerySuggested();
            }
        }


        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                CommandManager.InvalidateRequerySuggested();
            }
        }


        private string _passwordConfirmation = string.Empty;
        public string PasswordConfirmation
        {
            get => _passwordConfirmation;
            set
            {
                _passwordConfirmation = value;
                OnPropertyChanged(nameof(PasswordConfirmation));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private string _backupDirectory = string.Empty;

        public string BackupDirectory
        {
            get => _backupDirectory;
            set
            {
                _backupDirectory = value;
                OnPropertyChanged(nameof(BackupDirectory));
            }
        }

        #endregion

        #region Commands

        public ICommand CopyDirectoryCommand { get; }
        public ICommand BrowseBackupDirectoryCommand { get; }
        public ICommand SaveUserDataCommand { get; }

        #endregion

        #region Constructor

        public SettingsViewModel( UserRepository userRepository, AppSettingsService appSettingsService, AppSettings appSettings)
        {
            _userRepository = userRepository;
            _appSettingsService = appSettingsService;
            _appSettings = appSettings;

            if (string.IsNullOrWhiteSpace(_appSettings.BackupDirectory))
            {
                //if never changed, directory is default local folder
                BackupDirectory = DatabasePathProvider.BackupDirectory;
            }
            else
            {
                //if ever changed, directory is in 'settings' json file in local folder
                BackupDirectory = _appSettings.BackupDirectory;
            }

            CopyDirectoryCommand = new RelayCommand(CopyDirectory, CanCopyDirectory);
            BrowseBackupDirectoryCommand = new RelayCommand(async () => await BrowseBackupDirectoryAsync());
            SaveUserDataCommand = new RelayCommand(async () => await SaveUserDataAsync(), CanSaveUserData);

        }

        #endregion

        #region Backup Methods

        private async Task BrowseBackupDirectoryAsync()
        {
            OpenFolderDialog dialog = new OpenFolderDialog
            {
                Title = "Select backup directory",
                InitialDirectory = BackupDirectory
            };

            if (dialog.ShowDialog() == false)
                return;

            BackupDirectory = dialog.FolderName;

            _appSettings.BackupDirectory = BackupDirectory;
            await _appSettingsService.SaveAsync(_appSettings);
        }

        private void CopyDirectory()
        {
            Clipboard.SetText(BackupDirectory);
        }

        private bool CanCopyDirectory()
        {
            if (string.IsNullOrWhiteSpace(BackupDirectory))
            {
                return false;
            }
            return true;
        }

        #endregion

        #region User Methods

        private async Task SaveUserDataAsync()
        {
            await _userRepository.UpdateStandardUserAsync(Username, Password);

            Username = string.Empty;
            UsernameConfirmation = string.Empty;
            Password = string.Empty;
            PasswordConfirmation = string.Empty;

            //event that clears passwordboxes in xaml.cs 
            UserDataSaved?.Invoke();
        }

        private bool CanSaveUserData()
        {
            if (string.IsNullOrWhiteSpace(Username))
                return false;

            if (Username != UsernameConfirmation)
                return false;

            if (Username.Length < 1)
                return false;

            if (string.IsNullOrEmpty(Password))
                return false;

            if (Password != PasswordConfirmation)
                return false;

            if (Password.Length < 1)
                return false;


            return true;
        }

        #endregion
    }
}
