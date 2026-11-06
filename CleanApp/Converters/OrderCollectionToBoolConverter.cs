using CleanApp.Enums;
using System.Globalization;
using System.Windows.Data;

namespace CleanApp.Converters
{
    public class OrderCollectionToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not OrderCollectionStatus state)
                return false;

            return state == OrderCollectionStatus.Collected;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked)
            {
                if (isChecked == true)
                {
                    return OrderCollectionStatus.Collected;
                }
                else
                {
                    return OrderCollectionStatus.NotCollected;
                }
            }
            return OrderCollectionStatus.NotCollected;
        }
    }
}
