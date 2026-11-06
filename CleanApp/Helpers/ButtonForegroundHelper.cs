using System.Windows;
using System.Windows.Media;

namespace CleanApp.Helpers
{
    public static class ButtonForegroundHelper
    {
        public static readonly DependencyProperty ForegroundBrushProperty =
            DependencyProperty.RegisterAttached(
                "ForegroundBrush",
                typeof(Brush),
                typeof(ButtonForegroundHelper),
                new PropertyMetadata(Brushes.Black));

        public static void SetForegroundBrush(UIElement element, Brush value)
        {
            element.SetValue(ForegroundBrushProperty, value);
        }

        public static Brush GetForegroundBrush(UIElement element)
        {
            return (Brush)element.GetValue(ForegroundBrushProperty);
        }
    }
}
