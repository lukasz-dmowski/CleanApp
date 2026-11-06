using CleanApp.Enums;
using System.Globalization;
using System.Windows.Data;

namespace CleanApp.Converters
{
    public class OrderNotificationToEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is OrderNotificationStatus state)
            {
                return state == OrderNotificationStatus.Notified;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
