namespace ProductCatalog.Infrastructure.History
{
    internal class CategoriesHistory
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
