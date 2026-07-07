using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using ProductsCatalog.Performance.BenchmarkTests.Categories.Application.Common;

namespace ProductsCatalog.Performance.BenchmarkTests.Categories.Application
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class CategoriesCommandApplicationBenchmarks
    {
        private IServiceProvider _serviceProvider = null!;
        private IServiceScope _serviceScope = null!;

        private CreateCategoryCommandHandler _createHandler = null!;
        private CreateCategoryCommand _createCommand = null!;

        [GlobalSetup]
        public void Setup()
        {
            var services = new ServiceCollection();

            var commandsRepoMock = new Mock<ICategoriesCommandsRepository>();
            var validationPolicyMock = new Mock<IValidationPolicy<Category>>();

            validationPolicyMock
                .Setup(x => x.Validate(It.IsAny<Category>()))
                .ReturnsAsync(new ValidationResult());

            commandsRepoMock
                .Setup(x => x.SaveChanges(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            services.AddSingleton<CreateCategoryCommandFlowDescribtor>();
            services.AddSingleton(commandsRepoMock.Object);
            services.AddSingleton(validationPolicyMock.Object);
            services.AddScoped<CreateCategoryCommandHandler>();

            _serviceProvider = services.BuildServiceProvider();
            _serviceScope = _serviceProvider.CreateScope();

            _createHandler = _serviceScope.ServiceProvider.GetRequiredService<CreateCategoryCommandHandler>();
            _createCommand = CategoriesApplicationBenchmarkDataFactory.CreateCreateCommand();
        }

        [Benchmark(Baseline = true)]
        public async Task CreateCategory_Flow()
        {
            await _createHandler.Handle(_createCommand, CancellationToken.None);
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _serviceScope?.Dispose();
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
