using CleanApp.Enums;
using System.Globalization;
using System.Windows.Data;

namespace CleanApp.Converters
{
    public class OrderCollectionToEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is OrderCollectionStatus state)
            {
                if (state == OrderCollectionStatus.Collected)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
