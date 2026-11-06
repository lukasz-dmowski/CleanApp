using CleanApp.Enums;
using System.Collections.ObjectModel;

namespace CleanApp.Models
{
    public static class OrderBookGenerator
    {
        private static readonly Random _random = new Random();

        private static string[] _names = new string[]
        {
            "Smith","Johnson","Williams","Brown","Jones","Garcia","Miller","Davis","Rodriguez","Martinez",
            "Hernandez","Lopez","Gonzalez","Wilson","Anderson","Thomas","Taylor","Moore","Jackson","Martin",
            "Lee","Perez","Thompson","White","Harris","Sanchez","Clark","Ramirez","Lewis","Robinson",
            "Walker","Young","Allen","King","Wright","Scott","Torres","Nguyen","Hill","Flores",
            "Green","Adams","Nelson","Baker","Hall","Rivera","Campbell","Mitchell","Carter","Roberts",
            "Gomez","Phillips","Evans","Turner","Diaz","Parker","Cruz","Edwards","Collins","Reyes",
            "Stewart","Morris","Morales","Murphy","Cook","Rogers","Gutierrez","Ortiz","Morgan","Cooper",
            "Peterson","Bailey","Reed","Kelly","Howard","Ramos","Kim","Cox","Ward","Richardson",
            "Watson","Brooks","Chavez","Wood","James","Bennett","Gray","Mendoza","Ruiz","Hughes",
            "Price","Alvarez","Castillo","Sanders","Patel","Myers","Long","Ross","Foster","Jimenez",
            "Powell","Jenkins","Perry","Russell","Sullivan","Bell","Coleman","Butler","Henderson","Barnes",
            "Gonzales","Fisher","Vasquez","Simmons","Romero","Jordan","Patterson","Alexander","Hamilton","Graham",
            "Reynolds","Griffin","Wallace","Moreno","West","Cole","Hayes","Bryant","Herrera","Gibson",
            "Ellis","Tran","Medina","Aguilar","Stevens","Murray","Ford","Castro","Marshall","Owens",
            "Harrison","Fernandez","McDonald","Woods","Washington","Kennedy","Wells","Vargas","Henry","Chen",
            "Freeman","Webb","Tucker","Guzman","Burns","Crawford","Olson","Simpson","Porter","Hunter",
            "Gordon","Mendez","Silva","Shaw","Snyder","Mason","Dixon","Munoz","Hunt","Hicks",
            "Holmes","Palmer","Wagner","Black","Robertson","Boyd","Rose","Stone","Salazar","Fox",
            "Warren","Mills","Meyer","Rice","Schmidt","Garza","Daniels","Ferguson","Nichols","Stephens",
            "Soto","Weaver","Ryan","Gardner","Payne","Grant","Dunn","Kelley","Spencer","Hawkins",
            "Arnold","Pierce","Vega","Hansen","Peters","Santos","Hart","Bradley","Knight","Elliott",
            "Cunningham","Duncan","Armstrong","Hudson","Carroll","Lane","Riley","Andrews","Alvarado","Ray",
            "Delgado","Berry","Perkins","Hoffman","Johnston","Matthews","Pena","Richards","Contreras","Willis"
        };
        private static char[] _digits = new char[]
        {
            '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };

        public static void GenerateOrders(OrderBook orderBook)
        {
            for (int i = 1; i <= 100; i++)
            {
                var order = new Order
                {
                    OrderNumber = OrderNumberGenerator.GetNumber(),
                    ClientName = _names[_random.Next(_names.Length)],
                    TelephoneNumber = GenerateTelephoneNumber(),
                    StartDate = GenerateStartDate(),
                    Items = GenerateItems(),
                };

                order.ReadinessStatus = EvaluateOrderReadinessStatus(order);
                order.DueDate = GenerateDueDate(order);
                order.NotificationStatus = GenerateOrderNotification(order);
                order.CollectionStatus = GenerateOrderCollectionStatus(order);
                order.EndDate = GenerateEndDate(order);

                orderBook.Orders.Add(order);
            }
        }

        private static string GenerateTelephoneNumber()
        {
            char[] telephoneNumber = new char[9];

            for (int i = 0; i < 9; i++)
            {
                telephoneNumber[i] = _digits[_random.Next(_digits.Length)];
            }
            return new string(telephoneNumber);
        }
        private static DateTime GenerateStartDate()
        {
            int daysBack = _random.Next(0, 31);
            return DateTime.Today.AddDays(-daysBack);
        }
        private static DateTime? GenerateDueDate(Order order)
        {
            return order.StartDate?.AddDays(14);
        }
        private static DateTime? GenerateEndDate(Order order)
        {
            int daysBack = _random.Next(0, 31);

            if (order.CollectionStatus == OrderCollectionStatus.Collected)
            {
                DateTime endDate = DateTime.Today.AddDays(-daysBack);

                

                if (endDate < order.StartDate) endDate = DateTime.Today;

                return endDate;
            }
            else
            {
                return null;                
            }
        }
        private static ObservableCollection<Item> GenerateItems()
        {
            var items = new ObservableCollection<Item>();

            int itemsCount = _random.Next(1, 5);
            for (int i = 1; i <= itemsCount; i++)
            {
                items.Add(new Item
                {
                    ItemNumber = i,
                    Price = _random.Next(2,16) * 5,
                    Type = GenerateItemType(),
                    ReadinessStatus = GenerateItemStatus(),
                });
            }
            return items;
        }

        private static ItemType GenerateItemType()
        {
            List<ItemType> types;
            types = Enum.GetValues(typeof(ItemType)).Cast<ItemType>().ToList();
            types.Remove(ItemType.None);

            return types[_random.Next(types.Count)];
        }
        private static ItemReadinessStatus GenerateItemStatus()
        {
            List<ItemReadinessStatus> statuses;
            statuses = Enum.GetValues(typeof(ItemReadinessStatus)).Cast<ItemReadinessStatus>().ToList();
            statuses.Remove(ItemReadinessStatus.None);

            return statuses[_random.Next(statuses.Count)];
        }

        private static OrderReadinessStatus EvaluateOrderReadinessStatus(Order order)
        {
            bool areAllItemsPending = order.Items.All(o => o.ReadinessStatus == ItemReadinessStatus.Pending);
            bool areAllItemsCompleted = order.Items.All(o => o.ReadinessStatus == ItemReadinessStatus.Completed);

            if (areAllItemsPending == true)
            {
                return OrderReadinessStatus.Pending;
            }
            else if (areAllItemsCompleted == true)
            {
                return OrderReadinessStatus.Completed;
            }
            else
            {
                return OrderReadinessStatus.Partial;
            }
        }

        private static OrderNotificationStatus GenerateOrderNotification(Order order)
        {
            if (order.ReadinessStatus == OrderReadinessStatus.Completed)
            {
                int number = _random.Next(0, 2);

                if (number == 0)
                {
                    return OrderNotificationStatus.Notified;
                }
                else
                {
                    return OrderNotificationStatus.NotNotified;
                }
            }
            else
            {
                return OrderNotificationStatus.None;
            }
        }

        private static OrderCollectionStatus GenerateOrderCollectionStatus(Order order)
        {
            List<OrderCollectionStatus> statuses;
            statuses = Enum.GetValues(typeof(OrderCollectionStatus)).Cast<OrderCollectionStatus>().ToList();
            statuses.Remove(OrderCollectionStatus.None);

            if (order.NotificationStatus == OrderNotificationStatus.Notified)
            {
                return statuses[_random.Next(statuses.Count)];
            }
            else
            {
                return OrderCollectionStatus.None;
            }


        }
    }
}
