using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Concrete.Policies;

namespace ProductCatalog.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomain(
            this IServiceCollection services)
        {
            services.AddScoped<IValidationPolicy<Category>, CategoriesValidationPolicy>();
            services.AddScoped<IValidationPolicy<Currency>, CurrenciesValidationPolicy>();
            services.AddScoped<IValidationPolicy<MobilePhone>, MobilePhonesValidationPolicy>();
            services.AddScoped<IValidationPolicy<ElectronicDetails>, ElectronicDetailsValidationPolicy>();
            services.AddScoped<IValidationPolicyDescriptorProvider, CategoriesValidationPolicy>();
            services.AddScoped<IValidationPolicyDescriptorProvider, CurrenciesValidationPolicy>();
            services.AddScoped<IValidationPolicyDescriptorProvider, MobilePhonesValidationPolicy>();
            services.AddScoped<IValidationPolicyDescriptorProvider, ElectronicDetailsValidationPolicy>();
            return services;
        }
    }
}
