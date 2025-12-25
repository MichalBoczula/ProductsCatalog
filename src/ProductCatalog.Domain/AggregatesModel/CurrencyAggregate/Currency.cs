namespace ProductCatalog.Domain.AggregatesModel.CurrencyAggregate
{
    public sealed class Currency
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }

        private Currency() { }

        public Currency(string code, string description)
        {
            Id = Guid.NewGuid();
            Code = code;
            Description = description;
            IsActive = true;
        }

        public void Deactivate() => IsActive = false;

        public void AssigneNewProductInformation(Currency incoming)
        {
            Code = incoming.Code;
            Description = incoming.Description;
        }
    }
}
