using CleanApp.Enums;
using System.Globalization;
using System.Windows.Data;

namespace CleanApp.Converters
{
    public class OrderNotificationToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not OrderNotificationStatus state)
            {
                return false;
            }
            return state == OrderNotificationStatus.Notified;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked)
            {
                if (isChecked)
                {
                    return OrderNotificationStatus.Notified;
                }
                else
                {
                    return OrderNotificationStatus.NotNotified;
                }
            }
            return OrderNotificationStatus.NotNotified;
        }
    }
}
