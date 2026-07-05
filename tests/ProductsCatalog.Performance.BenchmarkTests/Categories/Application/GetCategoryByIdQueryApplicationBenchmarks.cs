using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductCatalog.Application.Features.Categories.Queries.GetCategoryById;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductsCatalog.Performance.BenchmarkTests.Categories.Application.Common;

namespace ProductsCatalog.Performance.BenchmarkTests.Categories.Application
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class GetCategoryByIdQueryApplicationBenchmarks
    {
        private IServiceProvider _serviceProvider = null!;
        private GetCategoryByIdQueryHandler _handler = null!;
        private GetCategoryByIdQuery _query = null!;

        [GlobalSetup]
        public void Setup()
        {
            var services = new ServiceCollection();

            var categoriesRepoMock = new Mock<ICategoriesQueriesRepository>();
            categoriesRepoMock
                .Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Guid id, CancellationToken _) => CategoriesApplicationBenchmarkDataFactory.CreateReadModel(id));

            services.AddSingleton<GetCategoryByIdQueryFlowDescribtor>();
            services.AddSingleton(categoriesRepoMock.Object);
            services.AddScoped<GetCategoryByIdQueryHandler>();

            _serviceProvider = services.BuildServiceProvider();
            var scope = _serviceProvider.CreateScope();

            _handler = scope.ServiceProvider.GetRequiredService<GetCategoryByIdQueryHandler>();
            _query = CategoriesApplicationBenchmarkDataFactory.CreateGetCategoryByIdQuery();
        }

        [Benchmark(Baseline = true)]
        public async Task GetCategoryById_Flow()
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
