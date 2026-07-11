using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetTopMobilePhones;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Performance.BenchmarkTests.MobilePhones.Application.Common;

namespace ProductCatalog.Performance.BenchmarkTests.MobilePhones.Application
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class GetTopMobilePhonesQueryApplicationBenchmarks
    {
        private IServiceProvider _serviceProvider = null!;
        private GetTopMobilePhonesQueryHandler _handler = null!;
        private GetTopMobilePhonesQuery _query = null!;

        [GlobalSetup]
        public void Setup()
        {
            MappingConfig.RegisterMappings();

            var services = new ServiceCollection();

            var mobilePhonesRepoMock = new Mock<IMobilePhonesQueriesRepository>();
            mobilePhonesRepoMock
                .Setup(x => x.GetTop(It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => MobilePhonesApplicationBenchmarkDataFactory.CreateTopReadModels());

            services.AddSingleton<GetTopMobilePhonesQueryFlowDescribtor>();
            services.AddSingleton(mobilePhonesRepoMock.Object);
            services.AddScoped<GetTopMobilePhonesQueryHandler>();

            _serviceProvider = services.BuildServiceProvider();
            var scope = _serviceProvider.CreateScope();

            _handler = scope.ServiceProvider.GetRequiredService<GetTopMobilePhonesQueryHandler>();
            _query = MobilePhonesApplicationBenchmarkDataFactory.CreateTopQuery();
        }

        [Benchmark(Baseline = true)]
        public async Task GetTopMobilePhones_Flow()
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
