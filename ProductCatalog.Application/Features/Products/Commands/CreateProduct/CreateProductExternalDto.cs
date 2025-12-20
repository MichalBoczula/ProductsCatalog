namespace ProductCatalog.Application.Features.Products.Commands.CreateProduct
{
    public sealed record CreateProductExternalDto(string Name, string Description, CreateMoneyExternalDto Price, Guid CategoryId);
}