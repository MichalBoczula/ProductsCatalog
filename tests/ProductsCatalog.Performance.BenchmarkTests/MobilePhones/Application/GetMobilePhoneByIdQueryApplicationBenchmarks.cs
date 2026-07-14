using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneById;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Performance.BenchmarkTests.MobilePhones.Application.Common;

namespace ProductCatalog.Performance.BenchmarkTests.MobilePhones.Application
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class GetMobilePhoneByIdQueryApplicationBenchmarks
    {
        private IServiceProvider _serviceProvider = null!;
        private GetMobilePhoneByIdQueryHandler _handler = null!;
        private GetMobilePhoneByIdQuery _query = null!;

        [GlobalSetup]
        public void Setup()
        {
            MappingConfig.RegisterMappings();

            var targetMobilePhoneId = Guid.Parse("26400545-81c4-4e50-95c7-c723006a83dd");
            var services = new ServiceCollection();

            var mobilePhonesRepoMock = new Mock<IMobilePhonesQueriesRepository>();
            mobilePhonesRepoMock
                .Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => MobilePhonesApplicationBenchmarkDataFactory.CreateReadModel(targetMobilePhoneId));

            services.AddSingleton<GetMobilePhoneByIdQueryFlowDescribtor>();
            services.AddSingleton(mobilePhonesRepoMock.Object);
            services.AddScoped<GetMobilePhoneByIdQueryHandler>();

            _serviceProvider = services.BuildServiceProvider();
            var scope = _serviceProvider.CreateScope();

            _handler = scope.ServiceProvider.GetRequiredService<GetMobilePhoneByIdQueryHandler>();
            _query = MobilePhonesApplicationBenchmarkDataFactory.CreateByIdQuery(targetMobilePhoneId);
        }

        [Benchmark(Baseline = true)]
        public async Task GetMobilePhoneById_Flow()
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
