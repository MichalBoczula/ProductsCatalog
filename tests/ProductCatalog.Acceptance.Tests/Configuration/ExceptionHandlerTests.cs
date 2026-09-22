using Microsoft.AspNetCore.Http;
using ProductCatalog.Api.Configuration;
using ProductCatalog.Domain.Validation.Common;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Configuration
{
    public sealed class ExceptionHandlerTests
    {
        [Fact]
        public async Task TryHandleAsync_WhenValidationExceptionIsThrown_ReturnsBadRequestProblemDetails()
        {
            var validationResult = new ValidationResult();
            validationResult.AddValidationError(new ValidationError
            {
                Message = "Amount must be greater than zero.",
                Entity = "Amount",
                Name = "AmountGreaterThanZeroValidationRule"
            });

            var context = new DefaultHttpContext
            {
                TraceIdentifier = "validation-trace-id"
            };
            context.Request.Path = "/mobile-phones";
            context.Response.Body = new MemoryStream();

            var exception = new ValidationException(validationResult);
            var exceptionHandler = new ExceptionHandler();

            var handled = await exceptionHandler.TryHandleAsync(
                context,
                exception,
                CancellationToken.None);

            handled.ShouldBeTrue();
            context.Response.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
            context.Response.Body.Position = 0;
            using var document = await JsonDocument.ParseAsync(context.Response.Body);
            var problem = document.RootElement;

            problem.GetProperty("status").GetInt32().ShouldBe((int)HttpStatusCode.BadRequest);
            problem.GetProperty("title").GetString().ShouldBe("Validation failed");
            problem.GetProperty("detail").GetString().ShouldBe("One or more validation errors occurred.");
            problem.GetProperty("instance").GetString().ShouldBe("/mobile-phones");
            problem.GetProperty("traceId").GetString().ShouldBe(context.TraceIdentifier);

            var errors = problem.GetProperty("errors");
            errors.GetArrayLength().ShouldBe(1);
            var error = errors[0];
            error.GetProperty("message").GetString().ShouldBe("Amount must be greater than zero.");
            error.GetProperty("entity").GetString().ShouldBe("Amount");
            error.GetProperty("name").GetString().ShouldBe("AmountGreaterThanZeroValidationRule");
        }
    }
}
