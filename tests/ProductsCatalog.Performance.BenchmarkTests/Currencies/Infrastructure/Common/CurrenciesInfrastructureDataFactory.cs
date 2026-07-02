using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;

namespace ProductCatalog.Performance.BenchmarkTests.Currencies.Infrastructure.Common
{
    internal static class CurrenciesInfrastructureDataFactory
    {
        private static int _seed;

        public static Currency Create(Guid id)
        {
            var seed = Interlocked.Increment(ref _seed);
            var currency = new Currency(CreateCurrencyCode(seed), $"Benchmark currency description {seed}");

            var idProperty = typeof(Currency).GetProperty("Id")
                             ?? typeof(Currency).BaseType?.GetProperty("Id");
            idProperty?.SetValue(currency, id);

            return currency;
        }

        public static Currency Create() => Create(Guid.NewGuid());

        private static string CreateCurrencyCode(int seed)
        {
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var value = seed + 3;

            return string.Create(3, value, static (chars, state) =>
            {
                const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                chars[0] = alphabet[state / (26 * 26) % 26];
                chars[1] = alphabet[state / 26 % 26];
                chars[2] = alphabet[state % 26];
            });
        }
    }
}
