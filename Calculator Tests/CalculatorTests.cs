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
        public void TestDataRetrieval()
        {
            // Test with correct data.
            string fileLine = "2018-09-13  CIRCLE_K 100 ";

            DataRetrieval SUT = new DataRetrieval();
            Transaction transaction = new Transaction();
            bool dataRetrieved = SUT.RetrieveDataFromLine(fileLine, ref transaction);

            Assert.AreEqual(true, dataRetrieved);
            Assert.AreEqual("2018-09-13", transaction.Date.ToString("yyyy-MM-dd"));
            Assert.AreEqual("CIRCLE_K", transaction.MerchantName);
            Assert.AreEqual(100.00, transaction.Amount, 0.001);

            // Test with bad data.
            fileLine = "2018-09-13  CIRCLE_K";
            dataRetrieved = SUT.RetrieveDataFromLine(fileLine, ref transaction);
            Assert.AreEqual(false, dataRetrieved);
        }

        [TestMethod]
        public void TestCalculateBasicFee()
        {
            Random randomNumber = new Random();
            FeeCalculator SUT = new FeeCalculator();
            Transaction transaction = new Transaction();

            for (int i = 0; i < 100; i++)
            {
                transaction.Amount = randomNumber.NextDouble() * 200;
                transaction.Fee = SUT.CalculateBasicFee(transaction);

                Assert.AreEqual(transaction.Amount / 100, transaction.Fee, 0.001);
            }
        }

        [TestMethod]
        public void TestAddDiscount()
        {
            string[] merchants = { "TELIA", "CIRCLE_K", "NETTO", "7-ELEVEN" };

            FeeCalculator SUT = new FeeCalculator();
            Transaction transaction;

            foreach (string merchant in merchants)
            {
                transaction = new Transaction()
                {
                    MerchantName = merchant,
                    Fee = 1.0
                };

                transaction.Fee = SUT.AddDiscount(transaction);

                switch (merchant)
                {
                    case "TELIA":
                        Assert.AreEqual(0.9, transaction.Fee);
                        break;

                    case "CIRCLE_K":
                        Assert.AreEqual(0.8, transaction.Fee);
                        break;

                    default:
                        Assert.AreEqual(1.0, transaction.Fee);
                        break;
                }
            }
        }

        [TestMethod]
        public void TestAddMonthlyFee()
        {
            double fee1 = 0;
            double fee2 = 0;

            // Test same month transaction entries.
            FeeCalculator SUT = new FeeCalculator();
            Transaction transaction = new Transaction()
            {
                Date = new DateTime(2020, 5, 25),
                MerchantName = "TELIA",
                Fee = 1.2
            };

            fee1 = SUT.AddMonthlyFee(transaction);

            transaction.Fee = 1.2;
            fee2 = SUT.AddMonthlyFee(transaction);

            Assert.AreEqual(29.0, (fee1 - fee2), 0.001);


            // Test different month transaction entries.
            SUT = new FeeCalculator();
            transaction = new Transaction()
            {
                Date = new DateTime(2020, 5, 25),
                MerchantName = "TELIA",
                Fee = 1.2
            };
            fee1 = SUT.AddMonthlyFee(transaction);

            transaction.Date = transaction.Date.AddMonths(1);
            transaction.Fee = 1.2;
            fee2 = SUT.AddMonthlyFee(transaction);

            Assert.AreEqual(fee1, fee2, 0.001);
           

            // Test if monthly fee is added if the basic fee was 0 and now 1.2.
            SUT = new FeeCalculator();
            transaction = new Transaction()
            {
                Date = new DateTime(2020, 5, 25),
                MerchantName = "TELIA",
                Fee = 0
            };
            fee1 = SUT.AddMonthlyFee(transaction);
            Assert.AreEqual(0, fee1, 0.001);

            transaction.Fee = 1.2;
            transaction.Date = new DateTime(2020, 5, 26);
            fee2 = SUT.AddMonthlyFee(transaction);
            Assert.AreEqual(30.2, (fee2 - fee1), 0.001);


            // Test if monthly fee is added if couple of months has passed.
            SUT = new FeeCalculator();
            transaction = new Transaction()
            {
                Date = new DateTime(2020, 5, 25),
                MerchantName = "TELIA",
                Fee = 1.2
            };
            fee1 = SUT.AddMonthlyFee(transaction);

            transaction.Date = transaction.Date.AddMonths(2);
            transaction.Fee = 1.2;
            fee2 = SUT.AddMonthlyFee(transaction);

            Assert.AreEqual(0, (fee2 - fee1), 0.001);


            // Test if monthly fee is added if year has passed.
            SUT = new FeeCalculator();
            transaction = new Transaction()
            {
                Date = new DateTime(2020, 5, 25),
                MerchantName = "TELIA",
                Fee = 1.2
            };
            fee1 = SUT.AddMonthlyFee(transaction);

            transaction.Date = transaction.Date.AddYears(1);
            transaction.Fee = 1.2;
            fee2 = SUT.AddMonthlyFee(transaction);

            Assert.AreEqual(0, (fee2 - fee1), 0.001);
        }

        [TestMethod]
        public void TestTransactionFeeDataGeneration()
        {
            string expected = "";

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                var feeGenerator = new TransactionsFeeDataGenerator();
                // "transactions.txt" should be located where executable is.
                feeGenerator.GenerateData(AppDomain.CurrentDomain.BaseDirectory + "\\transactions.txt");

                // "expected output.txt" should be located where executable is.
                using (var reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\expected output.txt"))
                {
                    expected = reader.ReadToEnd();
                }

                Assert.AreEqual(expected, sw.ToString());
            }
        }
    }
}