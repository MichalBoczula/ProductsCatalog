using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductCatalog.Domain.ReadModels;
using ProductCatalog.Infrastructure.Contexts.Commands;
using ProductCatalog.Infrastructure.Repositories.Currencies;
using ProductCatalog.Performance.BenchmarkTests.Currencies.Infrastructure.Common;
using Testcontainers.MsSql;

namespace ProductCatalog.Performance.BenchmarkTests.Currencies.Infrastructure
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class CurrenciesRepositoryBenchmarks
    {
        private MsSqlContainer _msSqlContainer = null!;
        private ProductsContext _context = null!;
        private CurrenciesCommandsRepository _commandsRepository = null!;
        private CurrenciesQueriesRepository _queriesRepository = null!;

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

            _commandsRepository = new CurrenciesCommandsRepository(_context);

            var memorySettings = new Dictionary<string, string?>
            {
                {"ConnectionStrings:DefaultConnection", connectionString}
            };
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(memorySettings)
                .Build();

            _queriesRepository = new CurrenciesQueriesRepository(configuration);

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
                var currency = CurrenciesInfrastructureDataFactory.Create(id);

                _commandsRepository.Add(currency);
                _existingIds.Add(id);
            }
            await _commandsRepository.SaveChanges(CancellationToken.None);
        }

        [Benchmark]
        public async Task<IReadOnlyList<CurrencyReadModel>> Dapper_GetCurrencies()
        {
            _readCounter++;
            return await _queriesRepository.GetCurrencies(CancellationToken.None);
        }

        [Benchmark]
        public async Task EFCore_AddCurrency()
        {
            var currency = CurrenciesInfrastructureDataFactory.Create();
            _commandsRepository.Add(currency);
            await _commandsRepository.SaveChanges(CancellationToken.None);
        }

        [Benchmark]
        public async Task EFCore_UpdateCurrency()
        {
            var id = _existingIds[_updateCounter % _existingIds.Count];
            _updateCounter++;

            var currency = await _commandsRepository.GetCurrencyById(id, CancellationToken.None);
            if (currency is null) return;

            var incomingData = CurrenciesInfrastructureDataFactory.Create(id);
            currency.AssigneNewCurrencyInformation(incomingData);

            _commandsRepository.Update(currency);
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
