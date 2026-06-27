using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;

namespace ProductCatalog.Performance.BenchmarkTests.Currencies.Domain
{
    internal static class CurrenciesValidationDataFactory
    {
        public const int Seed = 20240625;

        public static Currency CreateValid()
        {
            return new Currency("USD", $"United States dollar benchmark seed {Seed}");
        }

        public static Currency CreateInvalidSingle()
        {
            return new Currency("", $"United States dollar benchmark seed {Seed}");
        }

        public static Currency CreateAllInvalid()
        {
            return new Currency("", "");
        }
    }
}
