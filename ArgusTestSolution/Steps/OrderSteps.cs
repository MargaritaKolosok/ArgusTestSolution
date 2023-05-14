using ArgusTestSolution.BussinessObjects;
using ArgusTestSolution.Consts;
using TechTalk.SpecFlow;

namespace ArgusTestSolution.Steps
{
    [Binding]
    public class OrderSteps
    {
        private ScenarioContext _scenarioContext;


        private OrderItem _starter;
        private OrderItem _main;
        private OrderItem _drink;

        private TimeSpan _discountFinalTime;        
        private decimal _discountPercent;

        private decimal _serviceCharge = 0;

        private decimal _OrderSum;
        private decimal _FinalPrice;

        private Order _Order = new Order();
        private CheckoutBill CheckOutBill = new CheckoutBill();

        public OrderSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"the following set costs for the restaurant:")]
        public void GivenTheFollowingSetCostsForTheRestaurant(Table table)
        {
            foreach (var row in table.Rows)
            {
                var item = row["Item"];
                var cost = decimal.Parse(row["Cost"]
                    .Replace("£", string.Empty)
                    .Replace("$", string.Empty));

                switch (item.ToLower())
                {
                    case "starter":
                        _starter = new OrderItem(ItemType.Starter, cost);
                        _scenarioContext["starter"] = _starter;
                        break;
                    case "main":
                        _main = new OrderItem(ItemType.Main, cost);
                        _scenarioContext["main"] = _main;
                        break;
                    case "drink":
                        _drink = new OrderItem(ItemType.Drink, cost);
                        _scenarioContext["drink"] = _drink;
                        break;
                }
            }
        }

        [Given(@"drinks have a (.+)% discount when ordered before (\d+):(\d+)")]
        public void GivenDrinksHaveADiscountWhenOrderedBefore(int discountPercentage, int hour, int minute)
        {
            _discountPercent = discountPercentage;
            _discountFinalTime = new TimeSpan(hour, minute, 0);

            _scenarioContext["discount"] = discountPercentage;
            _scenarioContext["discountStopTime"] = _discountFinalTime;
           
            Console.WriteLine(  );
        }

        [Given(@"service charge on food is (.*)%")]
        public void GivenServiceChargeOnFoodIs(decimal charge)
        {
            _serviceCharge = charge;
            _scenarioContext["charge"] = charge;
        }

        private void SetDrinkDiscount(TimeSpan orderTime, TimeSpan discountFinalTime)
        {
            _drink.Discount = (orderTime < discountFinalTime) ? _drink.Discount = _discountPercent : 0;
            _scenarioContext["drink"] = _drink;
        }

        [When(@"A group orders (.*) starters, (.*) mains and (.*) drinks")]
        public void WhenAGroupOrdersStartersMainsAndDrinks(int starters, int mains, int drinks)
        {
            var order = _starter.Price * starters + _main.Price * mains + _drink.Price * drinks;

            Console.WriteLine($"Order price: {order}");
            _OrderSum = order;

            _scenarioContext["orderSum"] = _OrderSum;
        }

        public void AddItemsToOrder(OrderItem item, int quantity)
        {
            _Order.AddItemToOrder(item, quantity);
        }


        [When(@"A group orders (.*) starters, (.*) mains and (.*) drinks at (\d+):(\d+)")]
        public void WhenAGroupOrdersStartersMainsAndDrinksAtTime(int starters, int mains, int drinks, int hour, int minute)
        {
            var orderTime = new TimeSpan(hour, minute, 0);
            _scenarioContext["orderTime"] = orderTime;

            SetDrinkDiscount(orderTime, _discountFinalTime);

            AddItemsToOrder(_starter, starters);
            AddItemsToOrder(_main, mains);
            AddItemsToOrder(_drink, drinks);
            
            Console.WriteLine($"Order price: {_Order.TotalOrderSum}");
            _OrderSum = _Order.TotalOrderSum;

            _scenarioContext["orderSum"] = _OrderSum;          
        }

        [When(@"(.*) starter, (.*) main and (.*) drink are removed from order")]
        public void WhenARemoveFromOrderStartersMainsAndDrinks(int starters, int mains, int drinks)
        {
            _Order.RemoveFromOrder(ItemType.Starter, starters);
            _Order.RemoveFromOrder(ItemType.Main, mains);
            _Order.RemoveFromOrder(ItemType.Drink, drinks);
        }

        [Then(@"The order is sent to the endpoint the '([^']*)' is calculated correctly in the bill")]
        public void ThenTheOrderIsSentToTheEndpointTheIsCalculatedCorrectlyInTheBill(decimal total)
        {
            var charge = _scenarioContext.Get<decimal>("charge");
            var checkoutBill = new CheckoutBill(_Order.TotalOrderSum, charge);            
            
            var finalPrice = checkoutBill.CalculateFinalBill();
            Assert.That(finalPrice, Is.EqualTo(total), "Sum is not correct.");
        }

    }
}
