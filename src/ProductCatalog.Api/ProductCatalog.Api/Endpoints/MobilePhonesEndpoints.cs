using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;
using ProductCatalog.Application.Features.Products.Commands.CreateProduct;

namespace ProductCatalog.Api.Endpoints
{
    public static class MobilePhonesEndpoints
    {
        public static IEndpointRouteBuilder MapMobilePhonesEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/mobile-phones").WithTags("MobilePhones");

            MapMobilePhonesCommands(group);

            return group;
        }

        private static void MapMobilePhonesCommands(IEndpointRouteBuilder group)
        {
            group.MapPost("", async (CreateMobilePhoneExternalDto mobilePhone, IMediator mediator) =>
            {
                var result = await mediator.Send(new CreateMobilePhoneCommand(mobilePhone));
                return Results.Created($"/mobile-phones/{result.Id}", result);
            })
            .WithName("CreateMobilePhone")
            .WithSummary("Create a new mobile phone")
            .WithDescription("Creates a new mobile phone in the product catalog.")
            .Produces<ProductDto>(StatusCodes.Status201Created)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
        }
    }
}
