using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductCatalog.Application.Features.Categories.Queries.GetCategories;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductsCatalog.Performance.BenchmarkTests.Categories.Application.Common;

namespace ProductsCatalog.Performance.BenchmarkTests.Categories.Application
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class CategoriesQueryApplicationBenchmarks
    {
        private IServiceProvider _serviceProvider = null!;
        private GetCategoriesQueryHandler _handler = null!;
        private GetCategoriesQuery _query = null!;

        [GlobalSetup]
        public void Setup()
        {
            var services = new ServiceCollection();

            var categoriesRepoMock = new Mock<ICategoriesQueriesRepository>();
            categoriesRepoMock
                .Setup(x => x.GetCategories(It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => CategoriesApplicationBenchmarkDataFactory.CreateReadModels());

            services.AddSingleton<GetCategoriesQueryFlowDescribtor>();
            services.AddSingleton(categoriesRepoMock.Object);
            services.AddScoped<GetCategoriesQueryHandler>();

            _serviceProvider = services.BuildServiceProvider();
            var scope = _serviceProvider.CreateScope();

            _handler = scope.ServiceProvider.GetRequiredService<GetCategoriesQueryHandler>();
            _query = CategoriesApplicationBenchmarkDataFactory.CreateQuery();
        }

        [Benchmark(Baseline = true)]
        public async Task GetCategories_Flow()
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
