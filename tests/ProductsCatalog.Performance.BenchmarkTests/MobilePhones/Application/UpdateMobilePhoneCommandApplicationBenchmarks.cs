using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Performance.BenchmarkTests.MobilePhones.Application.Common;

namespace ProductCatalog.Performance.BenchmarkTests.MobilePhones.Application
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class UpdateMobilePhoneCommandApplicationBenchmarks
    {
        private IServiceProvider _serviceProvider = null!;
        private IServiceScope _serviceScope = null!;

        private UpdateMobilePhoneCommandHandler _updateHandler = null!;
        private UpdateMobilePhoneCommand _updateCommand = null!;

        [GlobalSetup]
        public void Setup()
        {
            var services = new ServiceCollection();

            var commandsRepoMock = new Mock<IMobilePhonesCommandsRepository>();
            var validationPolicyMock = new Mock<IValidationPolicy<MobilePhone>>();

            validationPolicyMock
                .Setup(x => x.Validate(It.IsAny<MobilePhone>()))
                .ReturnsAsync(new ValidationResult());

            commandsRepoMock
                .Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Guid id, CancellationToken _) => MobilePhonesApplicationBenchmarkDataFactory.CreateDomainPhone(id));

            commandsRepoMock
                .Setup(x => x.SaveChanges(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            services.AddSingleton<UpdateMobilePhoneCommandFlowDescribtor>();
            services.AddSingleton(commandsRepoMock.Object);
            services.AddSingleton(validationPolicyMock.Object);
            services.AddScoped<UpdateMobilePhoneCommandHandler>();

            _serviceProvider = services.BuildServiceProvider();
            _serviceScope = _serviceProvider.CreateScope();

            _updateHandler = _serviceScope.ServiceProvider.GetRequiredService<UpdateMobilePhoneCommandHandler>();

            var targetMobilePhoneId = Guid.NewGuid();
            _updateCommand = MobilePhonesApplicationBenchmarkDataFactory.CreateUpdateCommand(targetMobilePhoneId);
        }

        [Benchmark(Baseline = true)]
        public async Task UpdateMobilePhone_Flow()
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
