namespace ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories
{
    public interface IProductsCommandsRepository
    {
        Task AddAsync(Product product, CancellationToken ct);
        Task UpdateAsync(Product product, CancellationToken cancellationToken);
        Task<Product?> GetProductById(Guid productId, CancellationToken cancellationToken);
    }
}
