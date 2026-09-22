using BenchmarkDotNet.Running;
using ProductCatalog.Performance.BenchmarkTests.AmountValidationPolicy.Domain;
using ProductCatalog.Performance.BenchmarkTests.CommonDescription.Domain;
using ProductCatalog.Performance.BenchmarkTests.ElectronicDetails.Domain;
using ProductCatalog.Performance.BenchmarkTests.MobilePhoneFilter.Domain;
using ProductCatalog.Performance.BenchmarkTests.MobilePhones.Application;
using ProductCatalog.Performance.BenchmarkTests.MobilePhones.Domain;
using ProductCatalog.Performance.BenchmarkTests.MobilePhones.Infrastructure;

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

            // -- Infrastructure --
            BenchmarkRunner.Run<MobilePhonesRepositoryBenchmarks>();

            // -- Application (Queries) --
            BenchmarkRunner.Run<GetMobilePhonesQueryApplicationBenchmarks>();
            BenchmarkRunner.Run<GetFilteredMobilePhonesQueryApplicationBenchmarks>();
            BenchmarkRunner.Run<GetMobilePhoneByIdQueryApplicationBenchmarks>();
            BenchmarkRunner.Run<GetMobilePhoneByIdsQueryApplicationBenchmarks>();
            BenchmarkRunner.Run<GetTopMobilePhonesQueryApplicationBenchmarks>();
            BenchmarkRunner.Run<GetMobilePhoneHistoryQueryApplicationBenchmarks>();

            // -- Application (Commands) --
            BenchmarkRunner.Run<DeleteMobilePhoneCommandApplicationBenchmarks>();
            BenchmarkRunner.Run<UpdateMobilePhoneCommandApplicationBenchmarks>();
            BenchmarkRunner.Run<CreateMobilePhoneCommandApplicationBenchmarks>();

        }
    }
}
