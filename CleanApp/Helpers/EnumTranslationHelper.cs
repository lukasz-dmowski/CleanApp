using CleanApp.Enums;
using CleanApp.Resources;

namespace CleanApp.Helpers
{
    public static class EnumTranslationHelper
    {
        public static string GetText(ItemType type)
        {
            string key = $"ItemType_{type}";

            //translation according to language set on startup in App.xaml.cs
            string translatedText = EnumResources.ResourceManager.GetString(key, EnumResources.Culture);

            if (translatedText == null)
            {
                return type.ToString();
            }

            return translatedText;
        }

        public static string GetText(ItemReadinessStatus status)
        {
            string key = $"ItemReadinessStatus_{status}";

            //translation according to language set on startup in App.xaml.cs
            string translatedText = EnumResources.ResourceManager.GetString(key, EnumResources.Culture);

            if (translatedText == null)
            {
                return status.ToString();
            }

            return translatedText;
        }

        //sorting according to language, but putting "none" option as first in combobox
        public static IEnumerable<ItemType> GetSortedItemTypes()
        {
            var result = Enum.GetValues<ItemType>()
                             .Where(x => x != ItemType.None)
                             .OrderBy(GetText)
                             .Prepend(ItemType.None);
            return result;
        }

        //sorting according to language, but putting "none" option as first in combobox
        public static IEnumerable<ItemReadinessStatus> GetSortedItemStatuses()
        {
            var result = Enum.GetValues<ItemReadinessStatus>()
                             .Where(x => x != ItemReadinessStatus.None)
                             .OrderBy(GetText)
                             .Prepend(ItemReadinessStatus.None);
            return result;
        }
    }
}
