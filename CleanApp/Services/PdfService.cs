using CleanApp.Models;
using Microsoft.Win32;
using QuestPDF.Fluent;

namespace CleanApp.Services
{
    public class PdfService
    {
        public void SaveOrderPdf(Enterprise enterprise, Order order)
        {
            if (order == null)
            {
                return;
            }

            var dialog = new SaveFileDialog
            {
                Filter = "PDF (*.pdf)|*.pdf",  
                DefaultExt = ".pdf", 
                AddExtension = true,  
                FileName = $"{order.OrderNumber}_{order.ClientName}.pdf"
            };

            if (dialog.ShowDialog() == false)
            {
                return;
            }

            var document = new OrderDlDocument(enterprise, order);

            document.GeneratePdf(dialog.FileName);
        }
    }
}
