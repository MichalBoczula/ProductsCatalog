namespace ProductCatalog.Domain.Validation.Common;

public sealed class ConcurrencyConflictException : Exception
{
    public ConcurrencyConflictException() : base("The mobile phone changed during this request.") { }
}
