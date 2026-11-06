using CleanApp.Dialogs;
using MaterialDesignThemes.Wpf;

namespace CleanApp.Services
{
    public class DialogService
    {

        public async Task<bool> AskSavePdf()
        {
            var result = await DialogHost.Show(new SavePdfDialog(), "ShellDialog");

            if (result == null)
            {
                return false;
            }

            string? response = result.ToString();

            if (bool.TryParse(response, out bool savePdf) == false)
            {
                return false;
            }

            return savePdf;
        }

        public async Task<bool> AskDeleteOrder()
        {
            var result = await DialogHost.Show(new DeleteOrderDialog(), "ShellDialog");

            if (result == null)
            {
                return false;
            }

            string? response = result.ToString();

            if (bool.TryParse(response, out bool deleteOrder) == false)
            {
                return false;
            }

            return deleteOrder;
        }
        
    }
}
