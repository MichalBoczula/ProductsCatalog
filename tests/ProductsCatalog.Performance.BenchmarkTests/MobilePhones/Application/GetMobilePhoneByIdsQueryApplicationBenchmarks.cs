using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneByIds;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Performance.BenchmarkTests.MobilePhones.Application.Common;

namespace ProductCatalog.Performance.BenchmarkTests.MobilePhones.Application
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class GetMobilePhoneByIdsQueryApplicationBenchmarks
    {
        private IServiceProvider _serviceProvider = null!;
        private GetMobilePhoneByIdsQueryHandler _handler = null!;
        private GetMobilePhoneByIdsQuery _query = null!;

        [GlobalSetup]
        public void Setup()
        {
            MappingConfig.RegisterMappings();

            var mobilePhones = MobilePhonesApplicationBenchmarkDataFactory.CreateReadModels();
            var mobilePhoneIds = mobilePhones.Select(mobilePhone => mobilePhone.Id).ToArray();
            var services = new ServiceCollection();

            var mobilePhonesRepoMock = new Mock<IMobilePhonesQueriesRepository>();
            mobilePhonesRepoMock
                .Setup(x => x.GetByIds(It.IsAny<IReadOnlyCollection<Guid>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mobilePhones);

            services.AddSingleton<GetMobilePhoneByIdsQueryFlowDescribtor>();
            services.AddSingleton(mobilePhonesRepoMock.Object);
            services.AddScoped<GetMobilePhoneByIdsQueryHandler>();

            _serviceProvider = services.BuildServiceProvider();
            var scope = _serviceProvider.CreateScope();

            _handler = scope.ServiceProvider.GetRequiredService<GetMobilePhoneByIdsQueryHandler>();
            _query = MobilePhonesApplicationBenchmarkDataFactory.CreateByIdsQuery(mobilePhoneIds);
        }

        [Benchmark(Baseline = true)]
        public async Task GetMobilePhoneByIds_Flow()
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
