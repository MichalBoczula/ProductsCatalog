using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductCatalog.Application.Features.Categories.Commands.DeleteCategory;
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
    public class CategoriesDeleteCommandApplicationBenchmarks
    {
        private IServiceProvider _serviceProvider = null!;
        private IServiceScope _serviceScope = null!;

        private DeleteCategoryCommandHandler _deleteHandler = null!;
        private DeleteCategoryCommand _deleteCommand = null!;

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
                .Setup(x => x.GetCategoryById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => CategoriesApplicationBenchmarkDataFactory.CreateDomainCategory());

            commandsRepoMock
                .Setup(x => x.SaveChanges(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            services.AddSingleton<DeleteCategoryCommandFlowDescribtor>();
            services.AddSingleton(commandsRepoMock.Object);
            services.AddSingleton(validationPolicyMock.Object);
            services.AddScoped<DeleteCategoryCommandHandler>();

            _serviceProvider = services.BuildServiceProvider();
            _serviceScope = _serviceProvider.CreateScope();

            _deleteHandler = _serviceScope.ServiceProvider.GetRequiredService<DeleteCategoryCommandHandler>();

            var targetCategoryId = Guid.NewGuid();
            _deleteCommand = CategoriesApplicationBenchmarkDataFactory.CreateDeleteCommand(targetCategoryId);
        }

        [Benchmark(Baseline = true)]
        public async Task DeleteCategory_Flow()
        {
            await _deleteHandler.Handle(_deleteCommand, CancellationToken.None);
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
