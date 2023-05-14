using ArgusTestSolution.Consts;
using Newtonsoft.Json.Linq;

namespace ArgusTestSolution.BussinessObjects
{
    public class OrderItem
    {
        private decimal _discount = 0;
        private decimal _price = 0;
        public ItemType FoodType { get; set; }
        public OrderItem(decimal price, decimal discount = 0)
        {
            Price = price;
            Discount = discount;
        }

        public OrderItem(ItemType foodType, decimal price, decimal discount = 0)
        {
            Price = price;
            Discount = discount;
            FoodType = foodType;
        }

        public decimal Price
        {
            get
            {
                if (_discount != 0)
                {
                    var priceWithDiscount = Math.Round(_price - (_price * (_discount / 100)), 2);
                    return priceWithDiscount;
                }
                else
                {
                    return _price;
                }
            }
            set
            {
                _price = value;
            }
        }

        public decimal Discount
        {
            get { return _discount; }

            set
            {
                if (value < 0 || value > 100)
                {
                    throw new ArgumentOutOfRangeException("Discount must be between 0 and 100.");
                }
                _discount = value;
            }
        }
    }
}
