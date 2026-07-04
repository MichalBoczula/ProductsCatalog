using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductCatalog.Application.Features.Currencies.Queries.GetCurrencies;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories;
using ProductsCatalog.Performance.BenchmarkTests.Currencies.Application.Common;

namespace ProductsCatalog.Performance.BenchmarkTests.Currencies.Application
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class CurrenciesQueryApplicationBenchmarks
    {
        private IServiceProvider _serviceProvider = null!;
        private GetCurrenciesQueryHandler _handler = null!;
        private GetCurrenciesQuery _query = null!;

        [GlobalSetup]
        public void Setup()
        {
            var services = new ServiceCollection();

            var currenciesRepoMock = new Mock<ICurrenciesQueriesRepository>();
            currenciesRepoMock
                .Setup(x => x.GetCurrencies(It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => CurrenciesApplicationBenchmarkDataFactory.CreateReadModels());

            services.AddSingleton<GetCurrenciesQueryFlowDescribtor>();
            services.AddSingleton(currenciesRepoMock.Object);
            services.AddScoped<GetCurrenciesQueryHandler>();

            _serviceProvider = services.BuildServiceProvider();
            var scope = _serviceProvider.CreateScope();

            _handler = scope.ServiceProvider.GetRequiredService<GetCurrenciesQueryHandler>();
            _query = CurrenciesApplicationBenchmarkDataFactory.CreateQuery();
        }

        [Benchmark(Baseline = true)]
        public async Task GetCurrencies_Flow()
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
