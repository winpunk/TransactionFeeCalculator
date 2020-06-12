namespace Transaction_Fee_calculator
{
    public class DiscountEntry
    {
        public string mechantName { get; set; }
        public decimal discount { get; set; }

        public DiscountEntry(string mechantName, decimal discount)
        {
            this.mechantName = mechantName;
            this.discount = discount;
        }
    }
}