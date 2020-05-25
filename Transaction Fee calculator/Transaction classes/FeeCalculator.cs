using System;
using System.Collections.Generic;

namespace Transaction_Fee_calculator
{
    //-- For calculating all transactional fees necessary. --//
    public class FeeCalculator
    {
        // All fee rates in one place for easy management.
        private const double _basicFeeRate = 0.01;

        private const double _discountTelia = 0.9;
        private const double _discountCirkleK = 0.8;

        private const double _monthlyInvoiceFee = 29;

        // List to store all merchants which were billed with monthly fee at current month.
        private List<string> _monthlyFeeIssuedTo = new List<string>();

        // Last TransactionDate for checking if it is a new month in current line.
        private DateTime _lastTransactionDate = new DateTime();

        public double CalculateBasicFee(Transaction transaction)
        {
            return transaction.Amount * _basicFeeRate;
        }

        // Add discount for different merchants. Default case - no discount.
        public double AddDiscount(Transaction transaction)
        {
            switch (transaction.MerchantName)
            {
                case "TELIA":
                    return transaction.Fee * _discountTelia;

                case "CIRCLE_K":
                    return transaction.Fee * _discountCirkleK;

                default:
                    return transaction.Fee;
            }
        }

        // Add monthly fee if it is new month or year and the fee is not 0.
        public double AddMonthlyFee(Transaction transaction)
        {
            // Check transaction date and clear _monthlyFeeIssuedTo list if new month or year.
            if ((transaction.Date.Month > _lastTransactionDate.Month) || (transaction.Date.Year > _lastTransactionDate.Year)) _monthlyFeeIssuedTo.Clear();

            // Check the _monthlyFeeIssuedTo list if merchant was not issued with a fee and if transaction fee > 0.
            if (!_monthlyFeeIssuedTo.Contains(transaction.MerchantName) && transaction.Fee > 0)
            {
                // Add monthly fee and add merchant to _monthlyFeeIssuedTo list.
                transaction.Fee = transaction.Fee + _monthlyInvoiceFee;
                _monthlyFeeIssuedTo.Add(transaction.MerchantName);
            }

            // Update _lastTransactionDate
            _lastTransactionDate = transaction.Date;

            return transaction.Fee;
        }
    }
}