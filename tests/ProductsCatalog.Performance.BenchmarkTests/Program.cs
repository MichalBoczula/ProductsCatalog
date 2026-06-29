using BenchmarkDotNet.Running;
using ProductCatalog.Performance.BenchmarkTests.ElectronicDetails.Domain;
using ProductCatalog.Performance.BenchmarkTests.CommonDescription.Domain;
using ProductCatalog.Performance.BenchmarkTests.AmountValidationPolicy.Domain;
using ProductCatalog.Performance.BenchmarkTests.Categories.Domain;
using ProductCatalog.Performance.BenchmarkTests.Currencies.Domain;
using ProductCatalog.Performance.BenchmarkTests.MobilePhones.Domain;

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
            
            // ==========================================
            // 5. Currencies Domain
            // ==========================================
            BenchmarkRunner.Run<CurrenciesValidationPolicyBenchmarks>();
          
            // ==========================================
            // 6. Electronic Details / Domain
            // ==========================================
            BenchmarkRunner.Run<ElectronicDetailsValidationPolicyBenchmarks>();
        }
    }
}
