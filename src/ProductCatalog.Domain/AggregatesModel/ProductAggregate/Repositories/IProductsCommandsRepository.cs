using ProductCatalog.Domain.AggregatesModel.ProductAggregate.History;

namespace ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories
{
    public interface IProductsCommandsRepository
    {
        void Add(Product product);
        void Update(Product product);
        Task<Product?> GetProductById(Guid productId, CancellationToken cancellationToken);
        void WriteHistory(ProductsHistory entity);
        Task SaveChanges(CancellationToken cancellationToken);
    }
}
