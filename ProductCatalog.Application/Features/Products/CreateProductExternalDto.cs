namespace ProductCatalog.Application.Features.Products
{
    public sealed record CreateProductExternalDto(string Name, string Description, CreateMoneyExternalDto Price, Guid CategoryId);
}