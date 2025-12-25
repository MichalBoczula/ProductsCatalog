namespace ProductCatalog.Domain.AggregatesModel.ProductAggregate.ValueObjects
{
    public record struct Money
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        public Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }
    }
}
