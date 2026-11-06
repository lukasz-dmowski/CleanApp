namespace CleanApp.ViewModels
{
    public class OrderFilterOption<T> : ViewModelBase
    {
        private bool _isChecked = true;
        private bool _isEnabled = true;
        public T Value { get; }
        public string Name { get; }

        public OrderFilterOption(T value, string name)
        {
            Value = value;
            Name = name;
        }

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked == value)
                    return;

                _isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled == value)
                    return;

                _isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }


    }
}
