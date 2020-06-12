using System;

namespace Transaction_Fee_calculator
{
    public class ConsoleWritter : IOutputWritter
    {
        public void Write(ITransactionData transactionData)
        {
            Console.WriteLine("{0:yyyy-MM-dd} {1} {2:0.00}", transactionData.Date, transactionData.MerchantName, transactionData.Fee);
        }
    }
}