using ProductCatalog.Application.Features.Common;

namespace ProductCatalog.Application.Features.Products.Commands.UpdateProduct
{
    public sealed record UpdateProductExternalDto(string Name, string Description, UpdateMoneyExternalDto Price, Guid CategoryId);
}
