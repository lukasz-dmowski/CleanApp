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
    /// Interaction logic for AddOrderView.xaml
    /// </summary>
    public partial class AddOrderView : UserControl
    {
        public AddOrderView()
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
