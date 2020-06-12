using System;

namespace Transaction_Fee_calculator
{
    internal class MainProgram
    {
        private static void Main(string[] args)
        {            
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "transactions.txt";

            var feeDataGenerator = new TransactionsFeeDataGenerator();

            feeDataGenerator.OutputFeeData(filePath);

            Console.ReadKey();
        }
    }
}