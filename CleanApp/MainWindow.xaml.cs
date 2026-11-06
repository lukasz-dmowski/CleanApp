using CleanApp.Interfaces;
using CleanApp.Models;
using CleanApp.Services;
using CleanApp.Stores;
using CleanApp.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CleanApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel mainViewModel)
            {
                mainViewModel.PropertyChanged += (_, args) =>
                {
                    if (args.PropertyName == nameof(MainViewModel.MainCurrentViewModel))
                    {
                        ConnectWindowStateEvents(mainViewModel.MainCurrentViewModel);
                    }
                };
                ConnectWindowStateEvents(mainViewModel.MainCurrentViewModel);
            }
        }

        private void ConnectWindowStateEvents(ViewModelBase viewModel)
        {
            if (viewModel is IWindowStateEvents vm)
            {
                vm.RequestMinimize += OnRequestMinimize;
                vm.RequestMaximize += OnRequestMaximize;
                vm.RequestDrag += OnRequestDrag;
                vm.RequestClose += OnRequestClose;
            }   
        }
        private void OnRequestMinimize()
        {
            WindowState = WindowState.Minimized;
        }
        private void OnRequestMaximize()
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void OnRequestDrag()
        {
            try
            {
                DragMove();
            }
            catch
            {
  
            }
        }
        private void OnRequestClose()
        {
            Close();
        }

    }
}