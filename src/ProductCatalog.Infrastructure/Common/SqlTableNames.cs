using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.History;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.History;

namespace ProductCatalog.Infrastructure.Common
{
    internal static class SqlTableNames
    {
        public const string Products = "TB_Products";
        public const string Categories = "TB_Categories";
        public const string Currencies = "TB_Currencies";
        public const string ProductsHistory = "TB_Products_History";
        public const string CategoriesHistory = "TB_Categories_History";
        public const string CurrenciesHistory = "TB_Currencies_History";
    }
}