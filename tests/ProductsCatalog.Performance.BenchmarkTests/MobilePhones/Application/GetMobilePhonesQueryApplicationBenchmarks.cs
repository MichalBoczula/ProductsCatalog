using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhones;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Concrete.Policies;
using ProductCatalog.Performance.BenchmarkTests.MobilePhones.Application.Common;

namespace ProductCatalog.Performance.BenchmarkTests.MobilePhones.Application
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class GetMobilePhonesQueryApplicationBenchmarks
    {
        private IServiceProvider _serviceProvider = null!;
        private GetMobilePhonesQueryHandler _handler = null!;
        private GetMobilePhonesQuery _query = null!;

        [GlobalSetup]
        public void Setup()
        {
            MappingConfig.RegisterMappings();

            var services = new ServiceCollection();

            var mobilePhonesRepoMock = new Mock<IMobilePhonesQueriesRepository>();
            mobilePhonesRepoMock
                .Setup(x => x.GetPhones(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => MobilePhonesApplicationBenchmarkDataFactory.CreateReadModels());

            services.AddSingleton<GetMobilePhonesQueryFlowDescribtor>();
            services.AddSingleton<IValidationPolicy<int>, AmountValidationPolicy>();
            services.AddSingleton(mobilePhonesRepoMock.Object);
            services.AddScoped<GetMobilePhonesQueryHandler>();

            _serviceProvider = services.BuildServiceProvider();
            var scope = _serviceProvider.CreateScope();

            _handler = scope.ServiceProvider.GetRequiredService<GetMobilePhonesQueryHandler>();
            _query = MobilePhonesApplicationBenchmarkDataFactory.CreateQuery();
        }

        [Benchmark(Baseline = true)]
        public async Task GetMobilePhones_Flow()
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
