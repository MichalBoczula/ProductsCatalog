using ProductCatalog.Api.Configuration;
using ProductCatalog.Api.Endpoints;
using ProductCatalog.Application;
using ProductCatalog.Domain;
using ProductCatalog.Infrastructure;
using ProductCatalog.Infrastructure.Extensions;
using Serilog;

namespace ProductCatalog.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, services, configuration) => configuration
               .ReadFrom.Configuration(context.Configuration)
               .ReadFrom.Services(services)
               .Enrich.FromLogContext()
               .Enrich.WithEnvironmentName()
               .Enrich.WithMachineName()
               .WriteTo.Console());

            builder.Services.AddAuthorization();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => c.SupportNonNullableReferenceTypes());

            builder.Services.AddDomain();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();

            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(Application.DependencyInjection).Assembly));

            builder.Services.AddHealthChecks();

            builder.Services.AddExceptionHandler<ExceptionHandler>();
            builder.Services.AddProblemDetails();

            var app = builder.Build();

            app.UseExceptionHandler(_ => { });

            app.UseSerilogRequestLogging();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapProductsEndpoints();
            app.MapCategoriesEndpoints();
            app.MapCurrenciesEndpoints();
            app.MapDocumentationsEndpoints();

            app.MapHealthChecks("/health");

            app.ApplyMigrations();

            app.Run();
        }
    }
}
