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
    /// Interaction logic for ViewOrdersViewModel.xaml
    /// </summary>
    public partial class ViewOrdersView : UserControl
    {
        public ViewOrdersView()
        {
            InitializeComponent();
        }

        private void TelephoneNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string text = e.Text;
            bool areDigitsOnly = text.All(char.IsDigit);

            if (areDigitsOnly)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            }
        }

        private void ItemPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            if (e.Text.All(char.IsDigit) == true)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
