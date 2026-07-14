using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetFilteredMobilePhones;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Common.Filters;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Performance.BenchmarkTests.MobilePhones.Application.Common;

namespace ProductCatalog.Performance.BenchmarkTests.MobilePhones.Application
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class GetFilteredMobilePhonesQueryApplicationBenchmarks
    {
        private IServiceProvider _serviceProvider = null!;
        private GetFilteredMobilePhonesQueryHandler _handler = null!;
        private GetFilteredMobilePhonesQuery _query = null!;

        [GlobalSetup]
        public void Setup()
        {
            MappingConfig.RegisterMappings();

            var services = new ServiceCollection();

            var mobilePhonesRepoMock = new Mock<IMobilePhonesQueriesRepository>();
            mobilePhonesRepoMock
                .Setup(x => x.GetFilteredPhones(It.IsAny<MobilePhoneReadFilterDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => MobilePhonesApplicationBenchmarkDataFactory.CreateReadModels());

            services.AddSingleton<GetFilteredMobilePhonesQueryFlowDescribtor>();
            services.AddSingleton<IValidationPolicy<MobilePhoneFilterDto>, global::ProductCatalog.Domain.Validation.Concrete.Policies.MobilePhoneFilterValidationPolicy>();
            services.AddSingleton(mobilePhonesRepoMock.Object);
            services.AddScoped<GetFilteredMobilePhonesQueryHandler>();

            _serviceProvider = services.BuildServiceProvider();
            var scope = _serviceProvider.CreateScope();

            _handler = scope.ServiceProvider.GetRequiredService<GetFilteredMobilePhonesQueryHandler>();
            _query = MobilePhonesApplicationBenchmarkDataFactory.CreateFilteredQuery();
        }

        [Benchmark(Baseline = true)]
        public async Task GetFilteredMobilePhones_Flow()
        {
            await _handler.Handle(_query, CancellationToken.None);
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
