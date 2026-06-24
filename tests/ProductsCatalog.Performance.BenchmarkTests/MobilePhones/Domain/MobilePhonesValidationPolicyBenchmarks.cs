using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.ReadModels;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Concrete.Policies;

namespace ProductCatalog.Performance.BenchmarkTests.MobilePhones.Domain
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class MobilePhonesValidationPolicyBenchmarks
    {
        private IServiceProvider _serviceProvider = null!;
        private IServiceScope _serviceScope = null!;
        private IValidationPolicy<MobilePhone> _policy = null!;

        private MobilePhone _validEntity = null!;
        private MobilePhone _invalidSingleEntity = null!;
        private MobilePhone _allInvalidEntity = null!;

        [GlobalSetup]
        public void Setup()
        {
            var services = new ServiceCollection();

            var categoriesRepoMock = new Mock<ICategoriesQueriesRepository>();
            var currenciesRepoMock = new Mock<ICurrenciesQueriesRepository>();

            // Poprawione tworzenie obiektu CategoryReadModel z wymaganymi (required) polami
            var validCategoryReadModel = new CategoryReadModel
            {
                Id = MobilePhonesValidationDataFactory.ValidCategoryId,
                Code = "MOBILE",
                Name = "Mobile",
                IsActive = true
            };

            categoriesRepoMock
                .Setup(x => x.GetById(MobilePhonesValidationDataFactory.ValidCategoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validCategoryReadModel);

            categoriesRepoMock
                .Setup(x => x.GetById(MobilePhonesValidationDataFactory.InvalidCategoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CategoryReadModel?)null);

            services.AddSingleton(categoriesRepoMock.Object);
            services.AddSingleton(currenciesRepoMock.Object);
            services.AddScoped<IValidationPolicy<MobilePhone>, MobilePhonesValidationPolicy>();

            _serviceProvider = services.BuildServiceProvider();
            _serviceScope = _serviceProvider.CreateScope();

            _policy = _serviceScope.ServiceProvider.GetRequiredService<IValidationPolicy<MobilePhone>>();

            _validEntity = MobilePhonesValidationDataFactory.CreateValid();
            _invalidSingleEntity = MobilePhonesValidationDataFactory.CreateInvalidSingle();
            _allInvalidEntity = MobilePhonesValidationDataFactory.CreateAllInvalid();
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