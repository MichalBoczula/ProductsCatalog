namespace ProductCatalog.Performance.BenchmarkTests.AmountValidationPolicy.Domain
{
    internal static class AmountValidationDataFactory
    {
        private const int Seed = 587480;
        private static readonly Random Random = new(Seed);

        public static int CreateValid()
        {
            return Random.Next(1, int.MaxValue);
        }

        public static int CreateInvalidSingle()
        {
            return 0;
        }

        public static int CreateAllInvalid()
        {
            return Random.Next(int.MinValue, 0);
        }
    }
}
