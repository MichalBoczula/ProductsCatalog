using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductCatalog.Application.Features.Categories.Commands.UpdateCategory;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;
using ProductsCatalog.Performance.BenchmarkTests.Categories.Application.Common;

namespace ProductsCatalog.Performance.BenchmarkTests.Categories.Application
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class CategoriesUpdateCommandApplicationBenchmarks
    {
        private IServiceProvider _serviceProvider = null!;
        private IServiceScope _serviceScope = null!;

        private UpdateCategoryCommandHandler _updateHandler = null!;
        private UpdateCategoryCommand _updateCommand = null!;

        [GlobalSetup]
        public void Setup()
        {
            var services = new ServiceCollection();

            var commandsRepoMock = new Mock<ICategoriesCommandsRepository>();
            var validationPolicyMock = new Mock<IValidationPolicy<Category>>();

            validationPolicyMock
                .Setup(x => x.Validate(It.IsAny<Category>()))
                .ReturnsAsync(new ValidationResult());

            // Mockujemy bezpieczne załadowanie encji biznesowej z pamięci
            commandsRepoMock
                .Setup(x => x.GetCategoryById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => CategoriesApplicationBenchmarkDataFactory.CreateDomainCategory());

            commandsRepoMock
                .Setup(x => x.SaveChanges(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            services.AddSingleton<UpdateCategoryCommandFlowDescribtor>();
            services.AddSingleton(commandsRepoMock.Object);
            services.AddSingleton(validationPolicyMock.Object);
            services.AddScoped<UpdateCategoryCommandHandler>();

            _serviceProvider = services.BuildServiceProvider();
            _serviceScope = _serviceProvider.CreateScope();

            _updateHandler = _serviceScope.ServiceProvider.GetRequiredService<UpdateCategoryCommandHandler>();

            var targetCategoryId = Guid.NewGuid();
            _updateCommand = CategoriesApplicationBenchmarkDataFactory.CreateUpdateCommand(targetCategoryId);
        }

        [Benchmark(Baseline = true)]
        public async Task UpdateCategory_Flow()
        {
            await _updateHandler.Handle(_updateCommand, CancellationToken.None);
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
