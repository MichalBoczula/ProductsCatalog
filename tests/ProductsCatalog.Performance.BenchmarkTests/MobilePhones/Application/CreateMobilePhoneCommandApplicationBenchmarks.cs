using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Performance.BenchmarkTests.MobilePhones.Application.Common;

namespace ProductCatalog.Performance.BenchmarkTests.MobilePhones.Application
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class CreateMobilePhoneCommandApplicationBenchmarks
    {
        private IServiceProvider _serviceProvider = null!;
        private IServiceScope _serviceScope = null!;

        private CreateMobilePhoneCommandHandler _createHandler = null!;
        private CreateMobilePhoneCommand _createCommand = null!;

        [GlobalSetup]
        public void Setup()
        {
            MappingConfig.RegisterMappings();

            var services = new ServiceCollection();

            var commandsRepoMock = new Mock<IMobilePhonesCommandsRepository>();
            var validationPolicyMock = new Mock<IValidationPolicy<MobilePhone>>();

            validationPolicyMock
                .Setup(x => x.Validate(It.IsAny<MobilePhone>()))
                .ReturnsAsync(new ValidationResult());

            commandsRepoMock
                .Setup(x => x.Add(It.IsAny<MobilePhone>()));

            commandsRepoMock
                .Setup(x => x.WriteHistory(It.IsAny<MobilePhonesHistory>()));

            commandsRepoMock
                .Setup(x => x.SaveChanges(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            services.AddSingleton<CreateMobilePhoneCommandFlowDescribtor>();
            services.AddSingleton(commandsRepoMock.Object);
            services.AddSingleton(validationPolicyMock.Object);
            services.AddScoped<CreateMobilePhoneCommandHandler>();

            _serviceProvider = services.BuildServiceProvider();
            _serviceScope = _serviceProvider.CreateScope();

            _createHandler = _serviceScope.ServiceProvider.GetRequiredService<CreateMobilePhoneCommandHandler>();
            _createCommand = MobilePhonesApplicationBenchmarkDataFactory.CreateCreateCommand();
        }

        [Benchmark(Baseline = true)]
        public async Task CreateMobilePhone_Flow()
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
