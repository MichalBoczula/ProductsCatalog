using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductCatalog.Application.Features.MobilePhones.Commands.DeleteMobilePhone;
using ProductCatalog.Application.Mapping;
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
    public class DeleteMobilePhoneCommandApplicationBenchmarks
    {
        private IServiceProvider _serviceProvider = null!;
        private IServiceScope _serviceScope = null!;

        private DeleteMobilePhoneCommandHandler _deleteHandler = null!;
        private DeleteMobilePhoneCommand _deleteCommand = null!;

        [GlobalSetup]
        public void Setup()
        {
            MappingConfig.RegisterMappings();

            var targetMobilePhoneId = Guid.Parse("26400545-81c4-4e50-95c7-c723006a83dd");
            var services = new ServiceCollection();

            var commandsRepoMock = new Mock<IMobilePhonesCommandsRepository>();
            var validationPolicyMock = new Mock<IValidationPolicy<MobilePhone>>();

            validationPolicyMock
                .Setup(x => x.Validate(It.IsAny<MobilePhone>()))
                .ReturnsAsync(new ValidationResult());

            commandsRepoMock
                .Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => MobilePhonesApplicationBenchmarkDataFactory.CreateDomainPhone(targetMobilePhoneId));

            commandsRepoMock
                .Setup(x => x.SaveChanges(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            services.AddSingleton<DeleteMobilePhoneCommandFlowDescribtor>();
            services.AddSingleton(commandsRepoMock.Object);
            services.AddSingleton(validationPolicyMock.Object);
            services.AddScoped<DeleteMobilePhoneCommandHandler>();

            _serviceProvider = services.BuildServiceProvider();
            _serviceScope = _serviceProvider.CreateScope();

            _deleteHandler = _serviceScope.ServiceProvider.GetRequiredService<DeleteMobilePhoneCommandHandler>();
            _deleteCommand = MobilePhonesApplicationBenchmarkDataFactory.CreateDeleteCommand(targetMobilePhoneId);
        }

        [Benchmark(Baseline = true)]
        public async Task DeleteMobilePhone_Flow()
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
