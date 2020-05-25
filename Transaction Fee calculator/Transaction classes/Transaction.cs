using System;

namespace Transaction_Fee_calculator
{
    //-- For storage transaction data. --//
    public class Transaction
    {
        public DateTime Date { get; set; }
        public string MerchantName { get; set; }
        public double Amount { get; set; }
        public double Fee { get; set; }
    }
}