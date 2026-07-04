using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductCatalog.Application.Features.Currencies.Commands.CreateCurrency;
using ProductCatalog.Application.Features.Currencies.Commands.DeleteCurrency; 
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using ProductsCatalog.Performance.BenchmarkTests.Currencies.Application.Common;

namespace ProductCatalog.Performance.BenchmarkTests.Currencies.Application
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class CurrenciesCommandApplicationBenchmarks
    {
        private IServiceProvider _serviceProvider = null!;

        private CreateCurrencyCommandHandler _createHandler = null!;
        private DeleteCurrencyCommandHandler _deleteHandler = null!; 

        private CreateCurrencyCommand _createCommand = null!;
        private DeleteCurrencyCommand _deleteCommand = null!;

        [GlobalSetup]
        public void Setup()
        {
            var services = new ServiceCollection();

            var commandsRepoMock = new Mock<ICurrenciesCommandsRepository>();
            var validationPolicyMock = new Mock<IValidationPolicy<Currency>>();

            validationPolicyMock
                .Setup(x => x.Validate(It.IsAny<Currency>()))
                .ReturnsAsync(new ValidationResult());

            commandsRepoMock
                .Setup(x => x.GetCurrencyById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => CurrenciesApplicationBenchmarkDataFactory.CreateDomainCurrency());

            commandsRepoMock
                .Setup(x => x.SaveChanges(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            services.AddSingleton<CreateCurrencyCommandFlowDescribtor>();
            services.AddSingleton<DeleteCurrencyCommandFlowDescribtor>();

            services.AddSingleton(commandsRepoMock.Object);
            services.AddSingleton(validationPolicyMock.Object);
            services.AddScoped<CreateCurrencyCommandHandler>();
            services.AddScoped<DeleteCurrencyCommandHandler>();

            _serviceProvider = services.BuildServiceProvider();
            var scope = _serviceProvider.CreateScope();

            _createHandler = scope.ServiceProvider.GetRequiredService<CreateCurrencyCommandHandler>();
            _deleteHandler = scope.ServiceProvider.GetRequiredService<DeleteCurrencyCommandHandler>();

            var targetCurrencyId = Guid.NewGuid();
            _createCommand = CurrenciesApplicationBenchmarkDataFactory.CreateCreateCommand();
            _deleteCommand = CurrenciesApplicationBenchmarkDataFactory.CreateDeleteCommand(targetCurrencyId);
        }

        [Benchmark(Baseline = true)]
        public async Task CreateCurrency_Flow()
        {
            await _createHandler.Handle(_createCommand, CancellationToken.None);
        }

        [Benchmark]
        public async Task DeleteCurrency_Flow()
        {
            await _deleteHandler.Handle(_deleteCommand, CancellationToken.None);
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