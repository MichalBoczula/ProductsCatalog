namespace ProductCatalog.Domain.AggregatesModel.ProductAggregate.ValueObjects
{
    public sealed class Money
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        private Money() { }

        public Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }
    }
}
