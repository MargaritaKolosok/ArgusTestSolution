using ArgusTestSolution.Consts;

namespace ArgusTestSolution.BussinessObjects
{
    public class Order
    {
        public List<OrderItem> OrderedItems { get; set; }

        public decimal TotalOrderSum
        {
            get
            {
                return (OrderedItems != null && OrderedItems.Any())
                    ? OrderedItems.Select(x => x.Price).Sum() : 0;
            }
        }

        public Order()
        {
            OrderedItems = new List<OrderItem>();
        }

        public void AddItemToOrder(OrderItem item, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                OrderedItems.Add(item);
            }
        }

        public void RemoveFromOrder(ItemType itemType, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                var index = OrderedItems.FindLastIndex(x => x.FoodType == itemType);
                OrderedItems.RemoveAt(index);
            }

        }
    }
}
