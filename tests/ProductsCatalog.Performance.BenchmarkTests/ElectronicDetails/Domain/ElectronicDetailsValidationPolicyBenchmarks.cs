using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Concrete.Policies;

namespace ProductCatalog.Performance.BenchmarkTests.ElectronicDetails.Domain
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class ElectronicDetailsValidationPolicyBenchmarks
    {
        private IServiceProvider _serviceProvider = null!;
        private IServiceScope _serviceScope = null!;
        private IValidationPolicy<ProductCatalog.Domain.AggregatesModel.Common.ValueObjects.ElectronicDetails> _policy = null!;

        private ProductCatalog.Domain.AggregatesModel.Common.ValueObjects.ElectronicDetails _validEntity;
        private ProductCatalog.Domain.AggregatesModel.Common.ValueObjects.ElectronicDetails _invalidSingleEntity;
        private ProductCatalog.Domain.AggregatesModel.Common.ValueObjects.ElectronicDetails _allInvalidEntity;

        [GlobalSetup]
        public void Setup()
        {
            var services = new ServiceCollection();

            services.AddScoped<IValidationPolicy<ProductCatalog.Domain.AggregatesModel.Common.ValueObjects.ElectronicDetails>, ElectronicDetailsValidationPolicy>();

            _serviceProvider = services.BuildServiceProvider();
            _serviceScope = _serviceProvider.CreateScope();

            _policy = _serviceScope.ServiceProvider.GetRequiredService<IValidationPolicy<ProductCatalog.Domain.AggregatesModel.Common.ValueObjects.ElectronicDetails>>();

            _validEntity = ElectronicDetailsValidationDataFactory.CreateValid();
            _invalidSingleEntity = ElectronicDetailsValidationDataFactory.CreateInvalidSingle();
            _allInvalidEntity = ElectronicDetailsValidationDataFactory.CreateAllInvalid();
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
