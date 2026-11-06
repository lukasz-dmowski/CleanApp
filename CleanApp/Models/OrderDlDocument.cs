using CleanApp.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

public class OrderDlDocument : IDocument
{
    private readonly Enterprise _enterprise;
    private readonly Order _order;

    public OrderDlDocument(Enterprise enterprise, Order order)
    {
        _enterprise = enterprise;
        _order = order;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(99, 210, Unit.Millimetre);
            page.Margin(10, Unit.Millimetre);

            page.DefaultTextStyle(x => x.FontSize(12));

            page.Content().Column(column =>
            {
                column.Spacing(4);

                column.Item().LineHorizontal(2);
                column.Item().PaddingBottom(3);

                column.Item()
                    .AlignCenter()
                    .Text(_enterprise.Name)
                    .FontSize(18)
                    .Bold();

                column.Item().PaddingTop(3);
                column.Item().LineHorizontal(1);
                column.Item().PaddingBottom(3);

                column.Item().Row(row =>
                {
                    row.ConstantItem(70).Text("Adress:");

                    row.RelativeItem()
                        .PaddingLeft(10)
                        .Text(_enterprise.Street + " " + _enterprise.BuildingNumber);

                });
                column.Item().Row(row =>
                {
                    row.ConstantItem(70).Text("");

                    row.RelativeItem()
                        .PaddingLeft(10)
                        .Text(_enterprise.City + " " + _enterprise.PostalCode);
                });
                column.Item().Row(row =>
                {
                    row.ConstantItem(70).Text("Telephone:");

                    row.RelativeItem()
                        .PaddingLeft(10)
                        .Text(_enterprise.TelephoneNumber);
                });
                column.Item().Row(row =>
                {
                    row.ConstantItem(70).Text("");

                    row.RelativeItem()
                        .PaddingLeft(10)
                        .Text(_enterprise.Owner);

                });
                column.Item().Row(row =>
                {
                    row.ConstantItem(70).Text("");

                    row.RelativeItem()
                        .PaddingLeft(10)
                        .Text(_enterprise.ExtraInfo);

                });

                column.Item().PaddingTop(3);
                column.Item().LineHorizontal(2);
                column.Item().PaddingBottom(5);

                column.Item().Row(row =>
                {
                    row.ConstantItem(70).Text("Order ID:");

                    row.RelativeItem()
                        .PaddingLeft(10)
                        .Text(_order.OrderNumber.ToString());
                });
                column.Item().Row(row =>
                {
                    row.ConstantItem(70).Text("Client name:");

                    row.RelativeItem()
                        .PaddingLeft(10)
                        .Text(_order.ClientName);
                });
                column.Item().Row(row =>
                {
                    row.ConstantItem(70).Text("Telephone:");

                    row.RelativeItem()
                        .PaddingLeft(10)
                        .Text(_order.TelephoneNumber);
                });
                column.Item().Row(row =>
                {
                    row.ConstantItem(70).Text("Items count:");

                    row.RelativeItem()
                        .PaddingLeft(10)
                        .Text(_order.ItemsCount.ToString());
                });
                column.Item().Row(row =>
                {
                    row.ConstantItem(70).Text("Start date:");

                    row.RelativeItem()
                        .PaddingLeft(10)
                        .Text($"{_order.StartDate:dd.MM.yyyy}");
                });
                column.Item().Row(row =>
                {
                    row.ConstantItem(70).Text("Due date:");

                    row.RelativeItem()
                        .PaddingLeft(10)
                        .Text($"{_order.DueDate:dd.MM.yyyy}");
                });

                column.Item().PaddingTop(5);
                column.Item().LineHorizontal(1).LineDashPattern([3, 3]);
                column.Item().PaddingBottom(3);

                column.Item()
                    .AlignCenter()
                    .Text("Items")
                    .Bold();

                column.Item().PaddingTop(3);
                column.Item().LineHorizontal(1).LineDashPattern([3, 3]);
                column.Item().PaddingBottom(3);

                foreach (var item in _order.Items)
                {
                    column.Item().Row(row =>
                    {
                        row.ConstantItem(20)
                            .AlignLeft()
                            .Text(item.ItemNumber.ToString());

                        row.RelativeItem()
                            .Text(item.Type.ToString());

                        row.ConstantItem(50)
                            .AlignRight()
                            .Text($"{item.Price}");
                    });
                }

                column.Item().PaddingTop(3);
                column.Item().LineHorizontal(1);
                column.Item().PaddingBottom(3);

                column.Item().Row(row =>
                {
                    row.RelativeItem()
                        .Text("Total:")
                        .Bold();

                    row.ConstantItem(50)
                        .AlignRight()
                        .Text(_order.Price.ToString())
                        .Bold();
                });                    
            });
        });
    }
}