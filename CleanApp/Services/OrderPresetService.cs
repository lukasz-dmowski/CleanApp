using CleanApp.Enums;
using CleanApp.ViewModels;

namespace CleanApp.Services
{
    public class FilterPresetService
    {
        public void ApplyPreset(ViewOrdersViewModel vm, OrderFilter filter)
        {
            //uncheck all filters
            foreach (var f in vm.AllFilters)
            {
                f.IsChecked = false;
                f.IsEnabled = true;
            }

            switch (filter)
            {
                case OrderFilter.Overdue:
                    vm.PendingFilter.IsChecked = true;
                    vm.PartialFilter.IsChecked = true;

                    vm.OverdueFilter.IsChecked = true;

                    break;

                case OrderFilter.Today:
                    vm.PendingFilter.IsChecked = true;
                    vm.PartialFilter.IsChecked = true;

                    vm.TodayFilter.IsChecked = true;
                    break;

                case OrderFilter.Upcoming:
                    vm.PendingFilter.IsChecked = true;
                    vm.PartialFilter.IsChecked = true;

                    vm.UpcomingFilter.IsChecked = true;
                    break;

                case OrderFilter.Pending:
                    vm.PendingFilter.IsChecked = true;

                    vm.OverdueFilter.IsChecked = true;
                    vm.TodayFilter.IsChecked = true;
                    vm.UpcomingFilter.IsChecked = true;
                    break;

                case OrderFilter.Partial:
                    vm.PartialFilter.IsChecked = true;

                    vm.OverdueFilter.IsChecked = true;
                    vm.TodayFilter.IsChecked = true;
                    vm.UpcomingFilter.IsChecked = true;
                    break;

                case OrderFilter.Completed:
                    vm.CompletedFilter.IsChecked = true;

                    vm.NotNotifiedFilter.IsChecked = true;
                    break;

                case OrderFilter.Notified:
                    vm.CompletedFilter.IsChecked = true;

                    vm.NotifiedFilter.IsChecked = true;

                    vm.NotCollectedFilter.IsChecked = true;
                    break;

                case OrderFilter.NotNotified:
                    vm.CompletedFilter.IsChecked = true;

                    vm.NotNotifiedFilter.IsChecked= true;
                    break;

                case OrderFilter.Collected:
                    vm.CompletedFilter.IsChecked = true;

                    vm.NotifiedFilter.IsChecked = true;

                    vm.CollectedFilter.IsChecked = true;
                    break;

                case OrderFilter.NotCollected:
                    vm.CompletedFilter.IsChecked = true;

                    vm.NotifiedFilter.IsChecked = true;

                    vm.NotCollectedFilter.IsChecked = true;
                    break;

                case OrderFilter.SearchPhrase:

                    foreach (var f in vm.AllFilters)
                    {
                        f.IsChecked = true;
                        f.IsEnabled = true;
                    }
                    break;

            }
        }
    }

}
