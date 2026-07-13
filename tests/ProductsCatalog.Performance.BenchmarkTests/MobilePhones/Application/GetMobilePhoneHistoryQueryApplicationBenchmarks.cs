using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneHistory;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Performance.BenchmarkTests.MobilePhones.Application.Common;

namespace ProductCatalog.Performance.BenchmarkTests.MobilePhones.Application
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class GetMobilePhoneHistoryQueryApplicationBenchmarks
    {
        private IServiceProvider _serviceProvider = null!;
        private GetMobilePhoneHistoryQueryHandler _handler = null!;
        private GetMobilePhoneHistoryQuery _query = null!;

        [GlobalSetup]
        public void Setup()
        {
            MappingConfig.RegisterMappings();

            var services = new ServiceCollection();

            var mobilePhonesRepoMock = new Mock<IMobilePhonesQueriesRepository>();
            mobilePhonesRepoMock
                .Setup(x => x.GetHistoryOfChanges(
                    It.IsAny<Guid>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => MobilePhonesApplicationBenchmarkDataFactory.CreateHistoryReadModels());

            services.AddSingleton<GetMobilePhoneHistoryQueryFlowDescribtor>();
            services.AddSingleton(mobilePhonesRepoMock.Object);
            services.AddScoped<GetMobilePhoneHistoryQueryHandler>();

            _serviceProvider = services.BuildServiceProvider();
            var scope = _serviceProvider.CreateScope();

            _handler = scope.ServiceProvider.GetRequiredService<GetMobilePhoneHistoryQueryHandler>();
            _query = MobilePhonesApplicationBenchmarkDataFactory.CreateHistoryQuery();
        }

        [Benchmark(Baseline = true)]
        public async Task GetMobilePhoneHistory_Flow()
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
