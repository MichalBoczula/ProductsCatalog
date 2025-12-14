namespace ProductCatalog.Domain.AggregatesModel.CategoryAggregate
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
