namespace ProductCatalog.Domain.AggregatesModel.CategoryAggregate
{
    public sealed class Category
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }

        private Category() { }

        public Category(string code, string name)
        {
            Id = Guid.NewGuid();
            Code = code;
            Name = name;
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void AssigneNewCategoryInformation(Category incoming)
        {
            Code = incoming.Code;
            Name = incoming.Name;
        }
    }
}
