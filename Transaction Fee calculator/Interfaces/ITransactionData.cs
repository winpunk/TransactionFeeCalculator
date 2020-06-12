using System;

namespace Transaction_Fee_calculator
{
    public interface ITransactionData
    {
        DateTime Date { get; set; }
        string MerchantName { get; set; }
        decimal Amount { get; set; }
        decimal Fee { get; set; }
    }
}