using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using CommonDescriptionValueObject = ProductCatalog.Domain.AggregatesModel.Common.ValueObjects.CommonDescription;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Concrete.Policies;

namespace ProductCatalog.Performance.BenchmarkTests.CommonDescription.Domain
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class CommonDescriptionValidationPolicyBenchmarks
    {
        private IServiceProvider _serviceProvider = null!;
        private IServiceScope _serviceScope = null!;
        private IValidationPolicy<CommonDescriptionValueObject> _policy = null!;

        private CommonDescriptionValueObject _validEntity;
        private CommonDescriptionValueObject _invalidSingleEntity;
        private CommonDescriptionValueObject _allInvalidEntity;

        [GlobalSetup]
        public void Setup()
        {
            var services = new ServiceCollection();

            services.AddScoped<IValidationPolicy<CommonDescriptionValueObject>, CommonDescriptionValidationPolicy>();

            _serviceProvider = services.BuildServiceProvider();
            _serviceScope = _serviceProvider.CreateScope();

            _policy = _serviceScope.ServiceProvider.GetRequiredService<IValidationPolicy<CommonDescriptionValueObject>>();

            _validEntity = CommonDescriptionValidationDataFactory.CreateValid();
            _invalidSingleEntity = CommonDescriptionValidationDataFactory.CreateInvalidSingle();
            _allInvalidEntity = CommonDescriptionValidationDataFactory.CreateAllInvalid();
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
