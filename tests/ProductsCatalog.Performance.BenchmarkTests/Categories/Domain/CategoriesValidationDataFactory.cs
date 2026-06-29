using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;

namespace ProductCatalog.Performance.BenchmarkTests.Categories.Domain
{
    internal static class CategoriesValidationDataFactory
    {
        public const string ValidCodeSeed = "MOBILE";
        public const string ValidNameSeed = "Mobile phones";
        public const string InvalidCodeSeed = "";
        public const string InvalidNameSeed = "";

        public static Category CreateValid()
        {
            return new Category(ValidCodeSeed, ValidNameSeed);
        }

        public static Category CreateInvalidSingle()
        {
            return new Category(InvalidCodeSeed, ValidNameSeed);
        }

        public static Category CreateAllInvalid()
        {
            return new Category(InvalidCodeSeed, InvalidNameSeed);
        }
    }
}
