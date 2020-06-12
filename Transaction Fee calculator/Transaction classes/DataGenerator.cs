using System;

namespace Transaction_Fee_calculator
{
    public class DataGenerator : IDataGenerator
    {
        private DateTime _dateTemp;
        private decimal _amountTemp;

        public bool GenerateDataFromLine(string fileLine, ref ITransactionData transaction)
        {
            string[] lineItems = fileLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (lineItems.Length == 3)
            {
                DateTime.TryParse(lineItems[0], out _dateTemp);
                transaction.Date = _dateTemp;

                transaction.MerchantName = lineItems[1];

                decimal.TryParse(lineItems[2], out _amountTemp);
                transaction.Amount = _amountTemp;

                return true;
            }
            return false;
        }

        
    }
}