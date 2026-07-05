using BenchmarkDotNet.Running;
using ProductCatalog.Performance.BenchmarkTests.AmountValidationPolicy.Domain;
using ProductCatalog.Performance.BenchmarkTests.Categories.Domain;
using ProductCatalog.Performance.BenchmarkTests.Categories.Infrastructure;
using ProductsCatalog.Performance.BenchmarkTests.Categories.Application;
using ProductCatalog.Performance.BenchmarkTests.CommonDescription.Domain;
using ProductCatalog.Performance.BenchmarkTests.Currencies.Application;
using ProductCatalog.Performance.BenchmarkTests.Currencies.Domain;
using ProductCatalog.Performance.BenchmarkTests.Currencies.Infrastructure;
using ProductCatalog.Performance.BenchmarkTests.ElectronicDetails.Domain;
using ProductCatalog.Performance.BenchmarkTests.MobilePhoneFilter.Domain;
using ProductCatalog.Performance.BenchmarkTests.MobilePhones.Domain;
using ProductsCatalog.Performance.BenchmarkTests.Currencies.Application;

namespace ProductsCatalog.Performance.BenchmarkTests
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // ==========================================
            // 1. Mobile Phones / Products Domain
            // ==========================================
            BenchmarkRunner.Run<MobilePhonesValidationPolicyBenchmarks>();
          
            // ==========================================
            // 2. Common Description Domain
            // ==========================================
            BenchmarkRunner.Run<CommonDescriptionValidationPolicyBenchmarks>();
            
            // ==========================================
            // 3. Amount Validation Policy / Domain
            // ==========================================
            BenchmarkRunner.Run<AmountValidationPolicyBenchmarks>();
            
            // ==========================================
            // 4. Categories Domain
            // ==========================================
            BenchmarkRunner.Run<CategoriesValidationPolicyBenchmarks>();
            BenchmarkRunner.Run<CategoriesRepositoryBenchmarks>();
            BenchmarkRunner.Run<CategoriesQueryApplicationBenchmarks>();
            
            // ==========================================
            // 5. Currencies Domain
            // ==========================================
            BenchmarkRunner.Run<CurrenciesValidationPolicyBenchmarks>();
            BenchmarkRunner.Run<CurrenciesRepositoryBenchmarks>();
          
            // ==========================================
            // 6. Electronic Details / Domain
            // ==========================================
            BenchmarkRunner.Run<ElectronicDetailsValidationPolicyBenchmarks>();
          
            // ==========================================
            // 7. Mobile Phones Filter / Domain
            // ==========================================
            BenchmarkRunner.Run<MobilePhoneFilterValidationPolicyBenchmarks>();

            // ==========================================
            // 8. Currencies Application Queries Flow
            // ==========================================
            BenchmarkRunner.Run<CurrenciesQueryApplicationBenchmarks>();
            BenchmarkRunner.Run<CurrenciesCommandApplicationBenchmarks>();
        }
    }
}
