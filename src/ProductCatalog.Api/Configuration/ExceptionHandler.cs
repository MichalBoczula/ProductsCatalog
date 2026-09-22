using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Api.Configuration.Extensions;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Api.Configuration
{
    public sealed class ExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext context,
            Exception exception,
            CancellationToken cancellationToken)
        {
            await (exception switch
            {
                ValidationException validationException =>
                    ValidationExceptionHandlerExtension.HandleValidationException(
                        context, validationException, cancellationToken),

                ResourceNotFoundException notFoundException =>
                    NotFoundExceptionHandlerExtension.HandleNotFoundException(
                        context, notFoundException, cancellationToken),

                BadHttpRequestException badHttpRequestException when badHttpRequestException.InnerException is JsonException =>
                    JsonDeserializationExceptionHandlerExtension.HandleJsonDeserializationException(
                        context, cancellationToken),

                JsonException jsonException =>
                    JsonDeserializationExceptionHandlerExtension.HandleJsonDeserializationException(
                        context, cancellationToken),

                BadHttpRequestException badHttpRequestException =>
                    ApiProblemResponse.WriteAsync(
                        context,
                        badHttpRequestException.StatusCode == StatusCodes.Status415UnsupportedMediaType
                            ? StatusCodes.Status415UnsupportedMediaType
                            : StatusCodes.Status400BadRequest,
                        badHttpRequestException.StatusCode == StatusCodes.Status415UnsupportedMediaType
                            ? "unsupported_media_type" : "invalid_request",
                        "Invalid request.",
                        "The request could not be processed.",
                        cancellationToken),

                _ => DefaultExceptionHandlerExtension.HandleDefaultException(context, exception, _logger, cancellationToken)
            });

            return true;
        }
    }
}
