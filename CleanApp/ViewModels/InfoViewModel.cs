using CleanApp.Commands;
using CleanApp.Models;
using CleanApp.Services.Repositories;
using System.Windows.Input;

namespace CleanApp.ViewModels
{
    public class InfoViewModel : ViewModelBase
    {
        #region Fields

        private readonly Enterprise _enterprise;
        private Enterprise _backupEnterprise;
        private readonly EnterpriseRepository _enterpriseRepository;

        #endregion 

        #region Properties

        public string Version { get; set; }

        public string Name { get; set; }
        public string Owner { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string TelephoneNumber { get; set; }
        public string ExtraInfo { get; set; }

        #endregion 

        #region Commands

        public ICommand UndoChangesCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand SaveChangesCommand { get; }

        #endregion 

        #region Constructor

        public InfoViewModel(Enterprise enterprise, EnterpriseRepository enterpriseRepository)
        {
            _enterprise = enterprise;
            _enterpriseRepository = enterpriseRepository;

            //displayed version text (not included in .resx files!)
            Version = "App v0.0.9 (Showcase)";

            Name = enterprise.Name;
            Owner = enterprise.Owner;
            Street = enterprise.Street;
            BuildingNumber = enterprise.BuildingNumber;
            City = enterprise.City;
            PostalCode = enterprise.PostalCode;
            TelephoneNumber = enterprise.TelephoneNumber;
            ExtraInfo = enterprise.ExtraInfo;

            _backupEnterprise = enterprise.Clone();

            UndoChangesCommand = new RelayCommand(UndoChanges, CanUndoChanges);
            ClearCommand = new RelayCommand(Clear, CanClear);
            SaveChangesCommand = new RelayCommand(async () => await SaveChanges());
        }

        #endregion 

        #region Enterprise Methods

        private void UndoChanges()
        {
            Name = _enterprise.Name;
            Owner = _enterprise.Owner;
            Street = _enterprise.Street;
            BuildingNumber = _enterprise.BuildingNumber;
            City = _enterprise.City;
            PostalCode = _enterprise.PostalCode;
            TelephoneNumber = _enterprise.TelephoneNumber;
            ExtraInfo = _enterprise.ExtraInfo;

            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Owner));
            OnPropertyChanged(nameof(Street));
            OnPropertyChanged(nameof(BuildingNumber));
            OnPropertyChanged(nameof(City));
            OnPropertyChanged(nameof(PostalCode));
            OnPropertyChanged(nameof(TelephoneNumber));
            OnPropertyChanged(nameof(ExtraInfo));
        }
        private bool CanUndoChanges()
        {
            if (Name != _backupEnterprise.Name ||
                Owner != _backupEnterprise.Owner ||
                Street != _backupEnterprise.Street ||
                BuildingNumber != _backupEnterprise.BuildingNumber ||
                City != _backupEnterprise.City ||
                PostalCode != _backupEnterprise.PostalCode ||
                TelephoneNumber != _backupEnterprise.TelephoneNumber ||
                ExtraInfo != _backupEnterprise.ExtraInfo)
            {
                return true;
            }
            return false;
        }
        private void Clear()
        {
            Name = string.Empty;
            Owner = string.Empty;
            Street = string.Empty;
            BuildingNumber = string.Empty;
            City = string.Empty;
            PostalCode = string.Empty;
            TelephoneNumber = string.Empty;
            ExtraInfo = string.Empty;

            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Owner));
            OnPropertyChanged(nameof(Street));
            OnPropertyChanged(nameof(BuildingNumber));
            OnPropertyChanged(nameof(City));
            OnPropertyChanged(nameof(PostalCode));
            OnPropertyChanged(nameof(TelephoneNumber));
            OnPropertyChanged(nameof(ExtraInfo));

            CommandManager.InvalidateRequerySuggested();
        }
        private bool CanClear()
        {
            if (!string.IsNullOrEmpty(Name) ||
                !string.IsNullOrEmpty(Owner) ||
                !string.IsNullOrEmpty(Street) ||
                !string.IsNullOrEmpty(BuildingNumber) ||
                !string.IsNullOrEmpty(City) ||
                !string.IsNullOrEmpty(PostalCode) ||
                !string.IsNullOrEmpty(TelephoneNumber) ||
                !string.IsNullOrEmpty(ExtraInfo))
            {
                return true;
            }
            return false;
        }

        private async Task SaveChanges()
        {
            _enterprise.Name = Name;
            _enterprise.Owner = Owner;
            _enterprise.Street = Street;
            _enterprise.BuildingNumber = BuildingNumber;
            _enterprise.City = City;
            _enterprise.PostalCode = PostalCode;
            _enterprise.TelephoneNumber = TelephoneNumber;
            _enterprise.ExtraInfo = ExtraInfo;

            await _enterpriseRepository.UpdateAsync(_enterprise);

            _backupEnterprise = _enterprise.Clone();

            CommandManager.InvalidateRequerySuggested();
        }

        #endregion
    }
}
