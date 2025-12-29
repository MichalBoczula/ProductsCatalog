using MediatR;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace ProductCatalog.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>
        (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            using var scope = logger.BeginScope(
                new Dictionary<string, object?>
                {
                    ["RequestName"] = requestName,
                    ["TraceId"] = Activity.Current?.TraceId.ToString()
                });

            logger.LogInformation("Handling request {RequestName}", requestName);

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var response = await next();
                stopwatch.Stop();

                logger.LogInformation(
                    "Handled request {RequestName} in {ElapsedMilliseconds} ms",
                    requestName,
                    stopwatch.ElapsedMilliseconds);

                return response;
            }
            catch (Exception exception)
            {
                stopwatch.Stop();

                logger.LogError(
                    exception,
                    "Error handling request {RequestName} after {ElapsedMilliseconds} ms",
                    requestName,
                    stopwatch.ElapsedMilliseconds);

                throw;
            }
        }
    }
}
