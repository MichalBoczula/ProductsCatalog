namespace ProductCatalog.Domain.AggregatesModel.CategoryAggregate
{
    public sealed class Category
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }

        public Category() { }

        public Category(string code, string name)
        {
            Id = Guid.NewGuid();
            Code = code;
            Name = name;
        }
    }
}
