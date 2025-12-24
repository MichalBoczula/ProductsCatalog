namespace ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories
{
    public interface IProductsCommandsRepository
    {
        Task Add(Product product, CancellationToken ct);
        Task Update(Product product, CancellationToken cancellationToken);
        Task<Product?> GetProductById(Guid productId, CancellationToken cancellationToken);
    }
}
