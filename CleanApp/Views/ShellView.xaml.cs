using CleanApp.Interfaces;
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
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : UserControl
    {
        public ShellView()
        {
            InitializeComponent();
        }

        private void DragButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is ShellViewModel vm && vm.DragWindowCommand != null)
            {
                vm.DragWindowCommand.Execute(null);
            }
        }
    }
}
