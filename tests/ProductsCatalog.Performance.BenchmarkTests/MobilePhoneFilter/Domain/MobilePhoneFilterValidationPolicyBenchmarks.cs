using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Domain.Common.Filters;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Concrete.Policies;

namespace ProductCatalog.Performance.BenchmarkTests.MobilePhoneFilter.Domain
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class MobilePhoneFilterValidationPolicyBenchmarks
    {
        private IServiceProvider _serviceProvider = null!;
        private IServiceScope _serviceScope = null!;
        private IValidationPolicy<MobilePhoneFilterDto> _policy = null!;

        private MobilePhoneFilterDto _validFilter = null!;
        private MobilePhoneFilterDto _invalidSingleFilter = null!;
        private MobilePhoneFilterDto _allInvalidFilter = null!;

        [GlobalSetup]
        public void Setup()
        {
            var services = new ServiceCollection();

            services.AddScoped<IValidationPolicy<MobilePhoneFilterDto>, MobilePhoneFilterValidationPolicy>();

            _serviceProvider = services.BuildServiceProvider();
            _serviceScope = _serviceProvider.CreateScope();

            _policy = _serviceScope.ServiceProvider.GetRequiredService<IValidationPolicy<MobilePhoneFilterDto>>();

            _validFilter = MobilePhoneFilterValidationDataFactory.CreateValid();
            _invalidSingleFilter = MobilePhoneFilterValidationDataFactory.CreateInvalidSingle();
            _allInvalidFilter = MobilePhoneFilterValidationDataFactory.CreateAllInvalid();
        }

        [Benchmark(Baseline = true)]
        public async Task Validate_Success_HappyPath()
        {
            await _policy.Validate(_validFilter);
        }

        [Benchmark]
        public async Task Validate_Failure_SingleError()
        {
            await _policy.Validate(_invalidSingleFilter);
        }

        [Benchmark]
        public async Task Validate_Failure_MultipleErrors()
        {
            await _policy.Validate(_allInvalidFilter);
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _serviceScope.Dispose();
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
