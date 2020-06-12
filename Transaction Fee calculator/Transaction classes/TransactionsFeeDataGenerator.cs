using System;
using System.IO;

namespace Transaction_Fee_calculator
{
    public class TransactionsFeeDataGenerator
    {
        private ITransactionData _transactionData;
        private IDataGenerator _dataGenerator;
        private IFeeCalculator _feeCalculator;
        private IOutputWritter _consoleWritter;

        public TransactionsFeeDataGenerator() : this (new TransactionData(), new DataGenerator(), new FeeCalculator(), new ConsoleWritter())
        {

        }
        public TransactionsFeeDataGenerator(ITransactionData transactionData, IDataGenerator dataGenerator, IFeeCalculator feeCalculator, IOutputWritter outputWritter)
        {
            _transactionData = transactionData;
            _dataGenerator = dataGenerator;
            _feeCalculator = feeCalculator;
            _consoleWritter = outputWritter;

        }
        public void OutputFeeData(string fileInputPath)
        {
            string fileLine = "";

            try
            {
                using (var file = new StreamReader(fileInputPath))
                {
                    while ((fileLine = file.ReadLine()) != null)
                    {
                        if (!_dataGenerator.GenerateDataFromLine(fileLine, ref _transactionData))
                            continue;

                        _transactionData.Fee = _feeCalculator.AddBasicFee(_transactionData);

                        _transactionData.Fee = _feeCalculator.AddDiscount(_transactionData);

                        _transactionData.Fee = _feeCalculator.AddMonthlyFee(_transactionData);

                        _consoleWritter.Write(_transactionData);
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