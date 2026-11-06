using CleanApp.Enums;
using CleanApp.Helpers;
using System.Globalization;
using System.Windows.Data;

namespace CleanApp.Converters
{
    public class ItemStatusEnumToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not ItemReadinessStatus status)
            {
                return string.Empty;
            }

            return EnumTranslationHelper.GetText(status);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
