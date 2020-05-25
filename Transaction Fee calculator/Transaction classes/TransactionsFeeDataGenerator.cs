using System;
using System.IO;

namespace Transaction_Fee_calculator
{
    //-- For generating transactions fees data and outputting to the console window. --//
    public class TransactionsFeeDataGenerator
    {
        public void GenerateData(string filePath)
        {
            string fileLine = "";

            var transaction = new Transaction();
            var dataRetrieval = new DataRetrieval();
            var feeCalculator = new FeeCalculator();

            try
            {
                using (var file = new StreamReader(filePath))
                {
                    while ((fileLine = file.ReadLine()) != null)
                    {
                        // Check if we can retrieve transaction data from a file line and retrieve it.
                        if (!dataRetrieval.RetrieveDataFromLine(fileLine, ref transaction))
                            continue;

                        // -- MOBILEPAY-2. Calculate the basic fee for all the clients.
                        transaction.Fee = feeCalculator.CalculateBasicFee(transaction);

                        // -- MOBILEPAY-3 and MOBILEPAY-4. Add discount ("TELIA" 10% off. "CIRKLE_K" 20% off. Others - no discount).
                        transaction.Fee = feeCalculator.AddDiscount(transaction);

                        // -- MOBILEPAY-5. Invoice fee - 29 DKK every month.
                        transaction.Fee = feeCalculator.AddMonthlyFee(transaction);

                        // Round the fee to 2 decimal places (bankers rounding).
                        transaction.Fee = Math.Round(transaction.Fee, 2);
                        Console.WriteLine("{0:yyyy-MM-dd} {1} {2:0.00}", transaction.Date, transaction.MerchantName, transaction.Fee);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("-- ERROR: " + ex.Message);
            }
        }
    }
}