using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Concrete.Policies;

namespace ProductCatalog.Performance.BenchmarkTests.Currencies.Domain
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class CurrenciesValidationPolicyBenchmarks
    {
        private IServiceProvider _serviceProvider = null!;
        private IServiceScope _serviceScope = null!;
        private IValidationPolicy<Currency> _policy = null!;

        private Currency _validEntity = null!;
        private Currency _invalidSingleEntity = null!;
        private Currency _allInvalidEntity = null!;

        [GlobalSetup]
        public void Setup()
        {
            var services = new ServiceCollection();

            services.AddScoped<IValidationPolicy<Currency>, CurrenciesValidationPolicy>();

            _serviceProvider = services.BuildServiceProvider();
            _serviceScope = _serviceProvider.CreateScope();

            _policy = _serviceScope.ServiceProvider.GetRequiredService<IValidationPolicy<Currency>>();

            _validEntity = CurrenciesValidationDataFactory.CreateValid();
            _invalidSingleEntity = CurrenciesValidationDataFactory.CreateInvalidSingle();
            _allInvalidEntity = CurrenciesValidationDataFactory.CreateAllInvalid();
        }

        [Benchmark(Baseline = true)]
        public async Task Validate_Success_HappyPath()
        {
            await _policy.Validate(_validEntity);
        }

        [Benchmark]
        public async Task Validate_Failure_SingleError()
        {
            await _policy.Validate(_invalidSingleEntity);
        }

        [Benchmark]
        public async Task Validate_Failure_MultipleErrors()
        {
            await _policy.Validate(_allInvalidEntity);
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
