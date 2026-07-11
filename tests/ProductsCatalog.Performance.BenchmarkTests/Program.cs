using BenchmarkDotNet.Running;
using ProductCatalog.Performance.BenchmarkTests.AmountValidationPolicy.Domain;
using ProductCatalog.Performance.BenchmarkTests.Categories.Domain;
using ProductCatalog.Performance.BenchmarkTests.Categories.Infrastructure;
using ProductCatalog.Performance.BenchmarkTests.CommonDescription.Domain;
using ProductCatalog.Performance.BenchmarkTests.Currencies.Application;
using ProductCatalog.Performance.BenchmarkTests.Currencies.Domain;
using ProductCatalog.Performance.BenchmarkTests.Currencies.Infrastructure;
using ProductCatalog.Performance.BenchmarkTests.ElectronicDetails.Domain;
using ProductCatalog.Performance.BenchmarkTests.MobilePhoneFilter.Domain;
using ProductCatalog.Performance.BenchmarkTests.MobilePhones.Application;
using ProductCatalog.Performance.BenchmarkTests.MobilePhones.Domain;
using ProductsCatalog.Performance.BenchmarkTests.Categories.Application;
using ProductsCatalog.Performance.BenchmarkTests.Currencies.Application;

namespace ProductCatalog.Performance.BenchmarkTests
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // ===================================================
            // 1. DOMAIN: MOBILE PHONES / PRODUCTS
            // ===================================================
            BenchmarkRunner.Run<MobilePhonesValidationPolicyBenchmarks>();
            BenchmarkRunner.Run<CommonDescriptionValidationPolicyBenchmarks>();
            BenchmarkRunner.Run<ElectronicDetailsValidationPolicyBenchmarks>();
            BenchmarkRunner.Run<MobilePhoneFilterValidationPolicyBenchmarks>();
            BenchmarkRunner.Run<AmountValidationPolicyBenchmarks>();

            // -- Application (Queries) --
            BenchmarkRunner.Run<GetMobilePhonesQueryApplicationBenchmarks>();
            BenchmarkRunner.Run<GetTopMobilePhonesQueryApplicationBenchmarks>();

            // ===================================================
            // 2. DOMAIN: CATEGORIES
            // ===================================================
            // -- Domain --
            BenchmarkRunner.Run<CategoriesValidationPolicyBenchmarks>();

            // -- Infrastructure --
            BenchmarkRunner.Run<CategoriesRepositoryBenchmarks>();

            // -- Application (Queries) --
            BenchmarkRunner.Run<CategoriesQueryApplicationBenchmarks>();
            BenchmarkRunner.Run<GetCategoryByIdQueryApplicationBenchmarks>();

            // -- Application (Commands) --
            BenchmarkRunner.Run<CategoriesCreateCommandApplicationBenchmarks>();
            BenchmarkRunner.Run<CategoriesUpdateCommandApplicationBenchmarks>();
            BenchmarkRunner.Run<CategoriesDeleteCommandApplicationBenchmarks>();

            // ===================================================
            // 3. DOMAIN: CURRENCIES
            // ===================================================
            // -- Domain --
            BenchmarkRunner.Run<CurrenciesValidationPolicyBenchmarks>();

            // -- Infrastructure --
            BenchmarkRunner.Run<CurrenciesRepositoryBenchmarks>();

            // -- Application (Queries) --
            BenchmarkRunner.Run<CurrenciesQueryApplicationBenchmarks>();

            // -- Application (Commands) --
            BenchmarkRunner.Run<CurrenciesCreateCommandApplicationBenchmarks>();
            BenchmarkRunner.Run<CurrenciesUpdateCommandApplicationBenchmarks>();
            BenchmarkRunner.Run<CurrenciesDeleteCommandApplicationBenchmarks>();

            // Combined currency command flow benchmark (create/update/delete).
            BenchmarkRunner.Run<CurrenciesCommandApplicationBenchmarks>();
        }
    }
}