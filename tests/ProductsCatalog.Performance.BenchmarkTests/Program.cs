using BenchmarkDotNet.Running;
using ProductCatalog.Performance.BenchmarkTests.ElectronicDetails.Domain;
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
            // 2. Electronic Details / Domain
            // ==========================================
            BenchmarkRunner.Run<ElectronicDetailsValidationPolicyBenchmarks>();
        }
    }
}
