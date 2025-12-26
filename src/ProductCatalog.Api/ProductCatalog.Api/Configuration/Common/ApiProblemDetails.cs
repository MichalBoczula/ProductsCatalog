using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Api.Configuration.Common
{
    public sealed class ApiProblemDetails : ProblemDetails
    {
        public IEnumerable<ValidationError> Errors { get; init; } = Enumerable.Empty<ValidationError>();
        public string TraceId { get; init; } = string.Empty;
    }
}
