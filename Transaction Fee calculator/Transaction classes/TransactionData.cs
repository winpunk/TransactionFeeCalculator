using System;

namespace Transaction_Fee_calculator
{
   
    public class TransactionData : ITransactionData
    {
        public DateTime Date { get; set; }
        public string MerchantName { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
    }
}