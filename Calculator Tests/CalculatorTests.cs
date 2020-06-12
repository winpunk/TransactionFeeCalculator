using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Transaction_Fee_calculator;

namespace Fee_Calculator_Tests
{
    [TestClass]
    public class FeeCalculatorTests
    {
        [TestMethod]
        public void IsDataGeneratedFromFileLine()
        {
            // Test with correct data.
            string fileLine = "2018-09-13  CIRCLE_K 100 ";

            IDataGenerator SUT = new DataGenerator();
            ITransactionData transaction = new TransactionData();
            bool dataRetrieved = SUT.GenerateDataFromLine(fileLine, ref transaction);

            Assert.AreEqual(true, dataRetrieved);
            Assert.AreEqual("2018-09-13", transaction.Date.ToString("yyyy-MM-dd"));
            Assert.AreEqual("CIRCLE_K", transaction.MerchantName);
            Assert.AreEqual(100.00m, transaction.Amount);

            // Test with bad data.
            fileLine = "2018-09-13  CIRCLE_K";
            dataRetrieved = SUT.GenerateDataFromLine(fileLine, ref transaction);
            Assert.AreEqual(false, dataRetrieved);
        }

        [TestMethod]
        public void IsBasicFeeAdded()
        {
            Random randomNumber = new Random();
            IFeeCalculator SUT = new FeeCalculator();
            ITransactionData transaction = new TransactionData();

            for (int i = 0; i < 100; i++)
            {
                transaction.Amount = (decimal) (randomNumber.NextDouble() * 200);
                transaction.Fee = SUT.AddBasicFee(transaction);

                Assert.AreEqual(transaction.Amount / 100, transaction.Fee);
            }
        }

        [TestMethod]
        public void IsDiscountAdded()
        {
            string[] merchants = { "TELIA", "CIRCLE_K", "NETTO", "7-ELEVEN" };

            FeeCalculator SUT = new FeeCalculator();
            ITransactionData transaction;

            foreach (string merchant in merchants)
            {
                transaction = new TransactionData()
                {
                    MerchantName = merchant,
                    Fee = 1.0m
                };

                transaction.Fee = SUT.AddDiscount(transaction);

                switch (merchant)
                {
                    case "TELIA":
                        Assert.AreEqual(0.9m, transaction.Fee);
                        break;

                    case "CIRCLE_K":
                        Assert.AreEqual(0.8m, transaction.Fee);
                        break;

                    default:
                        Assert.AreEqual(1.0m, transaction.Fee);
                        break;
                }
            }
        }

        [TestMethod]
        public void IsMothlyFeeAdded()
        {
            decimal fee1 = 0m;
            decimal fee2 = 0m;

            
            FeeCalculator SUT = new FeeCalculator();
            ITransactionData transaction = new TransactionData()
            {
                Date = new DateTime(2020, 5, 25),
                MerchantName = "TELIA",
                Fee = 1.2m
            };           
           
            fee1 = SUT.AddMonthlyFee(transaction);

            transaction.Date = transaction.Date.AddMonths(1);
            transaction.Fee = 1.2m;
            fee2 = SUT.AddMonthlyFee(transaction);

            Assert.AreEqual(fee1, fee2);
           

            // Test if monthly fee is added if the basic fee was 0 and now 1.2.
            SUT = new FeeCalculator();
            transaction = new TransactionData()
            {
                Date = new DateTime(2020, 5, 25),
                MerchantName = "TELIA",
                Fee = 0
            };
            fee1 = SUT.AddMonthlyFee(transaction);
            Assert.AreEqual(0m, fee1);

            transaction.Fee = 1.2m;
            transaction.Date = new DateTime(2020, 5, 26);
            fee2 = SUT.AddMonthlyFee(transaction);
            Assert.AreEqual(30.2m, (fee2 - fee1));


            
        }

        [TestMethod]
        public void IsCorrectDataGenerated()
        {
            string expected = "";

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                var feeGenerator = new TransactionsFeeDataGenerator();
                // "transactions.txt" should be located where executable is.
                feeGenerator.OutputFeeData(AppDomain.CurrentDomain.BaseDirectory + "\\transactions.txt");

                // "expected output.txt" should be located where executable is.
                using (var reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\expected_output.txt"))
                {
                    expected = reader.ReadToEnd();
                }

                Assert.AreEqual(expected, sw.ToString());
            }
        }
    }
}