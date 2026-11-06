using MaterialDesignThemes.Wpf;
using System.Windows;

namespace CleanApp.Helpers
{
    public static class ButtonIconHelper
    {
        #region Icon Kind

        public static readonly DependencyProperty IconKindProperty =
                               DependencyProperty.RegisterAttached
                              ("IconKind", typeof(PackIconKind), typeof(ButtonIconHelper), new PropertyMetadata(PackIconKind.None));

        public static void SetIconKind(UIElement element, PackIconKind value)
        {
            element.SetValue(IconKindProperty, value);
        }
        public static PackIconKind GetIconKind(UIElement element)
        {
            return (PackIconKind)element.GetValue(IconKindProperty);
        }

        #endregion

    }
}
