using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace ProductsCatalog.Infrastructure.UnitTests.Integration.Configuration
{
    internal sealed class CustomTestConfiguration : IConfiguration
    {
        private readonly string _connectionString;

        public CustomTestConfiguration(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string? this[string key]
        {
            get => _connectionString;
            set { }
        }

        public IConfigurationSection GetSection(string key)
        {
            return new CustomTestConfigurationSection(_connectionString);
        }

        public IEnumerable<IConfigurationSection> GetChildren() => Array.Empty<IConfigurationSection>();
        public IChangeToken GetReloadToken() => throw new NotImplementedException();
    }

    internal sealed class CustomTestConfigurationSection : IConfigurationSection
    {
        public string? Value { get => _connectionString; set { } }
        public string Key => "ProductCatalogDb";
        public string Path => "ConnectionStrings:ProductCatalogDb";

        private readonly string _connectionString;

        public CustomTestConfigurationSection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string? this[string key]
        {
            get => _connectionString;
            set { }
        }

        public IConfigurationSection GetSection(string key) => this;
        public IEnumerable<IConfigurationSection> GetChildren() => Array.Empty<IConfigurationSection>();
        public IChangeToken GetReloadToken() => throw new NotImplementedException();
    }
}