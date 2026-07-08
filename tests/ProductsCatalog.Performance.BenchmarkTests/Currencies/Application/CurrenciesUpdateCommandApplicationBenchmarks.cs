using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductCatalog.Application.Features.Currencies.Commands.UpdateCurrency;
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
    public class CurrenciesUpdateCommandApplicationBenchmarks
    {
        private IServiceProvider _serviceProvider = null!;
        private IServiceScope _serviceScope = null!;

        private UpdateCurrencyCommandHandler _updateHandler = null!;
        private UpdateCurrencyCommand _updateCommand = null!;

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

            services.AddSingleton<UpdateCurrencyCommandFlowDescribtor>();
            services.AddSingleton(commandsRepoMock.Object);
            services.AddSingleton(validationPolicyMock.Object);
            services.AddScoped<UpdateCurrencyCommandHandler>();

            _serviceProvider = services.BuildServiceProvider();
            _serviceScope = _serviceProvider.CreateScope();

            _updateHandler = _serviceScope.ServiceProvider.GetRequiredService<UpdateCurrencyCommandHandler>();

            var targetCurrencyId = Guid.NewGuid();
            _updateCommand = CurrenciesApplicationBenchmarkDataFactory.CreateUpdateCommand(targetCurrencyId);
        }

        [Benchmark(Baseline = true)]
        public async Task UpdateCurrency_Flow()
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
