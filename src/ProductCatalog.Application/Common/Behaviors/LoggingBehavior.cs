using MediatR;
using Microsoft.Extensions.Logging;
using ProductCatalog.Domain.Validation.Common;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace ProductCatalog.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>
        (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            WriteIndented = false
        };

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

            var requestBody = SerializePayload(request);
            logger.LogInformation(
                "Handling request {RequestName} with payload: {RequestBody}",
                requestName,
                requestBody);

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var response = await next();
                stopwatch.Stop();

                var responseBody = SerializePayload(response);
                logger.LogInformation(
                    "Handled request {RequestName} in {ElapsedMilliseconds} ms with response: {ResponseBody}",
                    requestName,
                    stopwatch.ElapsedMilliseconds,
                    responseBody);

                return response;
            }
            catch (Exception exception)
            {
                stopwatch.Stop();

                var exceptionDetails = BuildExceptionDetails(exception);

                logger.LogError(
                    exception,
                    "Error handling request {RequestName} after {ElapsedMilliseconds} ms with payload: {RequestBody}. ExceptionType: {ExceptionType}. Details: {ExceptionDetails}",
                    requestName,
                    stopwatch.ElapsedMilliseconds,
                    requestBody,
                    exception.GetType().Name,
                    exceptionDetails);

                throw;
            }
        }

        private static string SerializePayload(object? payload)
        {
            try
            {
                return JsonSerializer.Serialize(payload, SerializerOptions);
            }
            catch (Exception)
            {
                return "<unserializable payload>";
            }
        }

        private static string BuildExceptionDetails(Exception exception)
        {
            switch (exception)
            {
                case ValidationException validationException:
                    return FormatValidationException(validationException);
                case ResourceNotFoundException resourceNotFoundException:
                    return FormatResourceNotFoundException(resourceNotFoundException);
                default:
                    return exception.Message;
            }
        }

        private static string FormatValidationException(ValidationException validationException)
        {
            var errors = validationException.ValidationResult.GetValidatonErrors();

            var builder = new StringBuilder();
            builder.Append("Validation errors: ");
            for (var index = 0; index < errors.Count; index++)
            {
                var error = errors[index];
                builder.Append($"[{index + 1}] Entity='{error.Entity}', Name='{error.Name}', Message='{error.Message}'");
                if (index < errors.Count - 1)
                {
                    builder.Append("; ");
                }
            }

            return builder.ToString();
        }

        private static string FormatResourceNotFoundException(ResourceNotFoundException exception)
        {
            return $"Resource not found. Action='{exception.ActionName}', ResourceType='{exception.ResourceType}', ResourceId='{exception.ResourceId}'.";
        }
    }
}
