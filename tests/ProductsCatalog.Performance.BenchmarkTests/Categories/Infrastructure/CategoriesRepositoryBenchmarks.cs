using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductCatalog.Domain.ReadModels;
using ProductCatalog.Infrastructure.Contexts.Commands;
using ProductCatalog.Infrastructure.Repositories.Categories;
using ProductCatalog.Performance.BenchmarkTests.Categories.Infrastructure.Common;
using Testcontainers.MsSql;

namespace ProductCatalog.Performance.BenchmarkTests.Categories.Infrastructure
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class CategoriesRepositoryBenchmarks
    {
        private MsSqlContainer _msSqlContainer = null!;
        private ProductsContext _context = null!;
        private CategoriesCommandsRepository _commandsRepository = null!;
        private CategoriesQueriesRepository _queriesRepository = null!;

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

            _commandsRepository = new CategoriesCommandsRepository(_context);

            var memorySettings = new Dictionary<string, string?>
            {
                {"ConnectionStrings:DefaultConnection", connectionString}
            };
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(memorySettings)
                .Build();

            _queriesRepository = new CategoriesQueriesRepository(configuration);

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
                var category = CategoriesInfrastructureDataFactory.Create(id);

                _commandsRepository.Add(category);
                _existingIds.Add(id);
            }
            await _commandsRepository.SaveChanges(CancellationToken.None);
        }

        [Benchmark]
        public async Task<CategoryReadModel?> Dapper_GetById()
        {
            var id = _existingIds[_readCounter % _existingIds.Count];
            _readCounter++;
            return await _queriesRepository.GetById(id, CancellationToken.None);
        }

        [Benchmark]
        public async Task<IReadOnlyList<CategoryReadModel>> Dapper_GetCategories()
        {
            return await _queriesRepository.GetCategories(CancellationToken.None);
        }

        [Benchmark]
        public async Task EFCore_AddCategory()
        {
            var category = CategoriesInfrastructureDataFactory.Create();
            _commandsRepository.Add(category);
            await _commandsRepository.SaveChanges(CancellationToken.None);
        }

        [Benchmark]
        public async Task EFCore_UpdateCategory()
        {
            var id = _existingIds[_updateCounter % _existingIds.Count];
            _updateCounter++;

            var category = await _commandsRepository.GetCategoryById(id, CancellationToken.None);
            if (category is null) return;

            var incomingData = CategoriesInfrastructureDataFactory.Create(id);
            category.AssigneNewCategoryInformation(incomingData);

            _commandsRepository.Update(category);
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
