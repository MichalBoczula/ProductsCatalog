using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Infrastructure.Contexts.Commands;
using ProductCatalog.Infrastructure.Repositories.MobilePhones;
using ProductCatalog.Performance.BenchmarkTests.MobilePhones.Infrastructure.Common;
using Testcontainers.MsSql;

namespace ProductCatalog.Performance.BenchmarkTests.MobilePhones.Infrastructure
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class MobilePhonesRepositoryBenchmarks
    {
        private MsSqlContainer _msSqlContainer = null!;
        private ProductsContext _context = null!;
        private MobilePhonesCommandsRepository _commandsRepository = null!;
        private MobilePhonesQueriesRepository _queriesRepository = null!;

        private readonly List<Guid> _existingIds = new();
        private int _readCounter;
        private int _updateCounter;

        [GlobalSetup]
        public async Task Setup()
        {
            _msSqlContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
                .Build();

            await _msSqlContainer.StartAsync();
            var connectionString = _msSqlContainer.GetConnectionString();

            var optionsBuilder = new DbContextOptionsBuilder<ProductsContext>();
            optionsBuilder.UseSqlServer(connectionString);

            _context = new ProductsContext(optionsBuilder.Options);
            await _context.Database.EnsureCreatedAsync();

            _commandsRepository = new MobilePhonesCommandsRepository(_context);

            var memorySettings = new Dictionary<string, string?>
            {
                {"ConnectionStrings:DefaultConnection", connectionString}
            };
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(memorySettings)
                .Build();

            _queriesRepository = new MobilePhonesQueriesRepository(configuration);

            await SeedAdditionalBenchmarkDataAsync();
        }

        private async Task SeedAdditionalBenchmarkDataAsync()
        {
            _existingIds.Clear();
            _readCounter = 0;
            _updateCounter = 0;

            for (int i = 0; i < 100; i++)
            {
                var id = Guid.NewGuid();
                var phone = MobilePhonesInfrastructureDataFactory.Create(id);

                _commandsRepository.Add(phone);
                _existingIds.Add(id);
            }
            await _commandsRepository.SaveChanges(CancellationToken.None);
        }

        [Benchmark]
        public async Task<MobilePhoneReadModel?> Dapper_GetById()
        {
            var id = _existingIds[_readCounter % _existingIds.Count];
            _readCounter++;
            return await _queriesRepository.GetById(id, CancellationToken.None);
        }

        [Benchmark]
        public async Task<IReadOnlyList<MobilePhoneReadModel>> Dapper_GetPhonesTopAmount()
        {
            return await _queriesRepository.GetPhones(10, CancellationToken.None);
        }

        [Benchmark]
        public async Task EFCore_AddMobilePhone()
        {
            var phone = MobilePhonesInfrastructureDataFactory.Create();
            _commandsRepository.Add(phone);
            await _commandsRepository.SaveChanges(CancellationToken.None);
        }

        [Benchmark]
        public async Task EFCore_UpdateMobilePhone()
        {
            var id = _existingIds[_updateCounter % _existingIds.Count];
            _updateCounter++;

            var phone = await _commandsRepository.GetById(id, CancellationToken.None);
            if (phone is null) return;

            var incomingData = MobilePhonesInfrastructureDataFactory.Create(id);
            phone.AssigneNewMobilePhoneInformation(incomingData);

            _commandsRepository.Update(phone);
            await _commandsRepository.SaveChanges(CancellationToken.None);
        }

        [GlobalCleanup]
        public async Task Cleanup()
        {
            if (_context is not null)
            {
                await _context.DisposeAsync();
            }
            if (_msSqlContainer is not null)
            {
                await _msSqlContainer.DisposeAsync();
            }
        }
    }
}