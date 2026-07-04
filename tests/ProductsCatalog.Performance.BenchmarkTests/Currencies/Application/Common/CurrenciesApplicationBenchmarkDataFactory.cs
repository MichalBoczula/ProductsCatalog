using ProductCatalog.Application.Features.Currencies.Queries.GetCurrencies;
using ProductCatalog.Domain.ReadModels;

namespace ProductsCatalog.Performance.BenchmarkTests.Currencies.Application.Common
{
    internal static class CurrenciesApplicationBenchmarkDataFactory
    {
        public static GetCurrenciesQuery CreateQuery()
        {
            return new GetCurrenciesQuery();
        }

        public static IReadOnlyList<CurrencyReadModel> CreateReadModels()
        {
            return new List<CurrencyReadModel>
            {
                new()
                {
                    Id = Guid.Parse("e73b3ef4-ec2c-4262-81ef-0ac21fbc1ec3"),
                    Code = "PLN",
                    Description = "Polish Złoty",
                    IsActive = true
                },
                new()
                {
                    Id = Guid.Parse("1a017544-890c-4219-891f-cd5549473d4e"),
                    Code = "USD",
                    Description = "US Dollar",
                    IsActive = true
                },
                new()
                {
                    Id = Guid.Parse("12da255e-6408-4b28-a5b1-84758f889348"),
                    Code = "EUR",
                    Description = "Euro",
                    IsActive = true
                }
            };
        }
    }
}
