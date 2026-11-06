using CleanApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CleanApp.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();

            DataContextChanged += SettingsView_DataContextChanged;
        }
        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is SettingsViewModel vm)
            {
                vm.Password = ((PasswordBox)sender).Password;
            }
        }

        private void PasswordConfirmation_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is SettingsViewModel vm)
            {
                vm.PasswordConfirmation = ((PasswordBox)sender).Password;
            }
        }

        private void SettingsView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //unbinding from old version of vm
            if (e.OldValue is SettingsViewModel oldViewModel)
                oldViewModel.UserDataSaved -= ClearPasswords;

            //binding with new version of vm
            if (e.NewValue is SettingsViewModel newViewModel)
                newViewModel.UserDataSaved += ClearPasswords;
        }

        private void ClearPasswords()
        {
            Password.Clear();
            PasswordConfirmation.Clear();
        }
    }
}
