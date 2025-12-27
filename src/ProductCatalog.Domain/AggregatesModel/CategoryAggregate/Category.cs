using ProductCatalog.Domain.AggregatesModel.Common;

namespace ProductCatalog.Domain.AggregatesModel.CategoryAggregate
{
    public sealed class Category : AggregateRoot
    {
        public string Code { get; private set; }
        public string Name { get; private set; }

        private Category() { }

        public Category(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public void AssigneNewCategoryInformation(Category incoming)
        {
            Code = incoming.Code.ToUpper();
            Name = incoming.Name;
            this.SetChangeDate();
        }
    }
}
