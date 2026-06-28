using BenchmarkDotNet.Running;
using ProductCatalog.Performance.BenchmarkTests.AmountValidationPolicy.Domain;
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
            // 2. Amount Validation Policy / Domain
            // ==========================================
            BenchmarkRunner.Run<AmountValidationPolicyBenchmarks>();
        }
    }
}
