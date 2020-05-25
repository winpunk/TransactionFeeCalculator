using System;

namespace Transaction_Fee_calculator
{
    //-- For retrieval data in "Date merchantName amount" format. --//
    public class DataRetrieval
    {
        private DateTime _dateTemp;
        private double _amountTemp;

        public bool RetrieveDataFromLine(string fileLine, ref Transaction transaction)
        {
            string[] lineItems = fileLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Check if we have exactly 3 objects in the line. If everything ok, update transaction object.
            if (lineItems.Length == 3)
            {
                DateTime.TryParse(lineItems[0], out _dateTemp);
                transaction.Date = _dateTemp;

                transaction.MerchantName = lineItems[1];

                double.TryParse(lineItems[2], out _amountTemp);
                transaction.Amount = _amountTemp;

                return true;
            }
            return false;
        }
    }
}