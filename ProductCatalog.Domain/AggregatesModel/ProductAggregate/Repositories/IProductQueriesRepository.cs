namespace ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories
{
    public interface IProductQueriesRepository
    {
        Task<Product?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<Product?> GetTopTen (CancellationToken ct);
        Task<Product?> GetByCategoryId (Guid CategoryId, CancellationToken ct);
    }
}
