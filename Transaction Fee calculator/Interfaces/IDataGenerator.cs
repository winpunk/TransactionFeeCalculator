namespace Transaction_Fee_calculator
{
    public interface IDataGenerator
    {
        bool GenerateDataFromLine(string fileLine, ref ITransactionData transaction);        
    }
}