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

namespace ProductsCatalog.Performance.BenchmarkTests.Currencies.Application
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class CurrenciesCommandApplicationBenchmarks
    {
        private IServiceProvider _serviceProvider = null!;
        private CreateCurrencyCommandHandler _handler = null!;
        private CreateCurrencyCommand _command = null!;

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
            var scope = _serviceProvider.CreateScope();

            _handler = scope.ServiceProvider.GetRequiredService<CreateCurrencyCommandHandler>();
            _command = CurrenciesApplicationBenchmarkDataFactory.CreateCreateCommand();
        }

        [Benchmark(Baseline = true)]
        public async Task CreateCurrency_Flow()
        {
            await _handler.Handle(_command, CancellationToken.None);
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
