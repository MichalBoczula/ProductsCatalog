namespace ProductCatalog.Domain.Validation.Common
{
    public sealed class ValidationError
    {
        public required string Message { get; init; }
        public required string Name { get; init; }
    }
}
