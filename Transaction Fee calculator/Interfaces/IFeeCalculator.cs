namespace Transaction_Fee_calculator
{
    public interface IFeeCalculator
    {
        decimal AddBasicFee(ITransactionData transaction);
        decimal AddDiscount(ITransactionData transaction);
        decimal AddMonthlyFee(ITransactionData transaction);

    }
}