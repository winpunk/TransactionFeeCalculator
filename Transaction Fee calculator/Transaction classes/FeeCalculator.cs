using System;
using System.Collections.Generic;

namespace Transaction_Fee_calculator
{

    public class FeeCalculator : IFeeCalculator
    {
        private List<DiscountEntry> _merchantDiscountsList = new List<DiscountEntry>() 
        { 
            new DiscountEntry("TELIA", 0.9m),
            new DiscountEntry("CIRCLE_K", 0.8m)
        };

        private const decimal _basicFeeRate = 0.01m;

        private const decimal _monthlyInvoiceFee = 29m;

        private List<string> _monthlyFeeIssuedTo = new List<string>();

        private DateTime _lastTransactionDate = new DateTime();

        public decimal AddBasicFee(ITransactionData transaction)
        {
            return transaction.Amount * _basicFeeRate;
        }

        public decimal AddDiscount(ITransactionData transaction)
        {            
            foreach(var discountEntry in _merchantDiscountsList)
            {
                if(discountEntry.mechantName.Equals(transaction.MerchantName))
                {
                    return transaction.Fee * discountEntry.discount;
                }
            }

            return transaction.Fee;
        }
               
        public decimal AddMonthlyFee(ITransactionData transaction)
        {
            if ((transaction.Date.Month > _lastTransactionDate.Month) || (transaction.Date.Year > _lastTransactionDate.Year)) 
                _monthlyFeeIssuedTo.Clear();

            if (!_monthlyFeeIssuedTo.Contains(transaction.MerchantName) && transaction.Fee > 0)
            {
                transaction.Fee = transaction.Fee + _monthlyInvoiceFee;
                _monthlyFeeIssuedTo.Add(transaction.MerchantName);
            }

            _lastTransactionDate = transaction.Date;

            return transaction.Fee;
        }
    }
}