namespace Transaction_Fee_calculator
{
    public interface IOutputWritter
    {
        void Write(ITransactionData transactionData);
    }
}