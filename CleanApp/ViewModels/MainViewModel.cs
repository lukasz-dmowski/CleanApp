using CleanApp.Stores;

namespace CleanApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        public ViewModelBase MainCurrentViewModel => _navigationStore.CurrentViewModel;
        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += () => OnPropertyChanged(nameof(MainCurrentViewModel));
        }
    }
}
