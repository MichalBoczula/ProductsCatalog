using BenchmarkDotNet.Running;
using ProductCatalog.Performance.BenchmarkTests.MobilePhoneFilter.Domain;
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
            BenchmarkRunner.Run<MobilePhoneFilterValidationPolicyBenchmarks>();
        }
    }
}
