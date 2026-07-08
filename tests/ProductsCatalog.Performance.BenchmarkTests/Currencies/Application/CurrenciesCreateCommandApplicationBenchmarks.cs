using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductCatalog.Application.Features.Currencies.Commands.CreateCurrency;
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
    public class CurrenciesCreateCommandApplicationBenchmarks
    {
        private IServiceProvider _serviceProvider = null!;
        private IServiceScope _serviceScope = null!;

        private CreateCurrencyCommandHandler _createHandler = null!;
        private CreateCurrencyCommand _createCommand = null!;

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
                .Setup(x => x.SaveChanges(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            services.AddSingleton<CreateCurrencyCommandFlowDescribtor>();
            services.AddSingleton(commandsRepoMock.Object);
            services.AddSingleton(validationPolicyMock.Object);
            services.AddScoped<CreateCurrencyCommandHandler>();

            _serviceProvider = services.BuildServiceProvider();
            _serviceScope = _serviceProvider.CreateScope();

            _createHandler = _serviceScope.ServiceProvider.GetRequiredService<CreateCurrencyCommandHandler>();
            _createCommand = CurrenciesApplicationBenchmarkDataFactory.CreateCreateCommand();
        }

        [Benchmark(Baseline = true)]
        public async Task CreateCurrency_Flow()
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
