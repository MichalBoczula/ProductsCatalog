using Microsoft.AspNetCore.Mvc;

namespace ProductCatalog.Api.Configuration.Common
{
    public sealed class NotFoundProblemDetails : ProblemDetails
    {
        public string TraceId { get; init; } = string.Empty;
    }
}
