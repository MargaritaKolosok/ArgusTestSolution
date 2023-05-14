namespace ArgusTestSolution.BussinessObjects
{
    public class CheckoutBill
    {
        private decimal _charge = 10;

        public CheckoutBill()
        {
        }

        public CheckoutBill(decimal finalSum, decimal charge)
        {
            FinalSum = finalSum;
            Charge = charge;
        }

        public decimal FinalSum { get; set; }

        public decimal Charge
        {
            get { return _charge; }

            set
            {
                if (value < 0 || value > 100)
                {
                    throw new ArgumentOutOfRangeException("Discount must be between 0 and 100.");
                }
                _charge = value;
            }
        }
        
        public decimal CalculateFinalBill()
        {
            return FinalSum + Math.Round((FinalSum * _charge)/100, 2);
        }


    }
}
