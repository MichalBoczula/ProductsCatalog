using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using ProductCatalog.Api.Configuration.Extensions;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Api.Configuration
{
    public sealed class ExceptionHandler : IExceptionHandler
    {
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
                        context, badHttpRequestException, cancellationToken),

                JsonException jsonException =>
                    JsonDeserializationExceptionHandlerExtension.HandleJsonDeserializationException(
                        context, jsonException, cancellationToken),

                _ => DefaultExceptionHandlerExtension.HandleDefaultException(context, cancellationToken)
            });

            return true;
        }
    }
}
