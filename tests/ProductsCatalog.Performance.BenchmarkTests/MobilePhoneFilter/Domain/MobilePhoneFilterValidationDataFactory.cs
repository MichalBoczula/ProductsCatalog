using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.Common.Filters;

namespace ProductCatalog.Performance.BenchmarkTests.MobilePhoneFilter.Domain
{
    internal static class MobilePhoneFilterValidationDataFactory
    {
        private const int Seed = 20260625;
        private static readonly Random RandomGenerator = new(Seed);
        private static readonly MobilePhonesBrand[] ValidBrands = Enum.GetValues<MobilePhonesBrand>();

        public static MobilePhoneFilterDto CreateValid()
        {
            return new MobilePhoneFilterDto
            {
                Brand = ValidBrands[RandomGenerator.Next(ValidBrands.Length)],
                MinimalPrice = 0m,
                MaximalPrice = 999.99m
            };
        }

        public static MobilePhoneFilterDto CreateInvalidSingle()
        {
            return new MobilePhoneFilterDto
            {
                Brand = ValidBrands[RandomGenerator.Next(ValidBrands.Length)],
                MinimalPrice = 999.99m,
                MaximalPrice = 999.99m
            };
        }

        public static MobilePhoneFilterDto CreateAllInvalid()
        {
            return new MobilePhoneFilterDto
            {
                Brand = (MobilePhonesBrand)RandomGenerator.Next(100, 200),
                MinimalPrice = -999.99m,
                MaximalPrice = -1m
            };
        }
    }
}
