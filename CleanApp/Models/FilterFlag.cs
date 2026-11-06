namespace CleanApp.Models
{
    /// <summary>
    /// Represents a filter checkbox state, used in orders filtering.
    /// </summary>
    public class FilterFlag : ModelBase
    {
        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnPropertyChanged(nameof(IsEnabled));
                }
            }
        }

        public FilterFlag(bool isChecked = true)
        {
            _isChecked = isChecked;
        }
    }
}
