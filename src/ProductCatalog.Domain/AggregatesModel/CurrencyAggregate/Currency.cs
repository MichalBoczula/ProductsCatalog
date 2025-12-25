using ProductCatalog.Domain.AggregatesModel.Common;

namespace ProductCatalog.Domain.AggregatesModel.CurrencyAggregate
{
    public sealed class Currency : AggregateRoot
    {
        public string Code { get; private set; }
        public string Description { get; private set; }

        private Currency() { }

        public Currency(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public void AssigneNewCurrencyInformation(Currency incoming)
        {
            Code = incoming.Code;
            Description = incoming.Description;
            this.SetChangeDate();
        }
    }
}
