using System;

namespace Transaction_Fee_calculator
{
    internal class MainProgram
    {
        private static void Main(string[] args)
        {
            // Contains file path of input file with transactions. "transactions.txt" should be located where executable is.
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "transactions.txt";

            var feeDataGenerator = new TransactionsFeeDataGenerator();

            // GenerateData generates all transaction fee data according to the rules and outputs it to the console window.
            feeDataGenerator.GenerateData(filePath);

            Console.ReadKey();
        }
    }
}