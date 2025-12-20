namespace ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories
{
    public interface IProductCommandsRepository
    {
        Task AddAsync(Product product, CancellationToken ct);
    }
}
