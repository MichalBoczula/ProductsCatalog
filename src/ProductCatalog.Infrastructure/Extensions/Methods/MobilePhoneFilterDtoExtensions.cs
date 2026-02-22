using Dapper;
using ProductCatalog.Domain.Common.Filters;
using System.Text;

namespace ProductCatalog.Infrastructure.Extensions.Methods
{
    internal static class MobilePhoneFilterDtoExtensions
    {
        internal static DynamicParameters FilterQueryBuilder(this MobilePhoneReadFilterDto mobilePhoneFilter, StringBuilder sb)
        {
            var @params = new DynamicParameters();

            if (mobilePhoneFilter.MinimalPrice is not null)
            {
                sb.Append(" AND PriceAmount >= @MinimalPrice");
                @params.Add("MinimalPrice", mobilePhoneFilter.MinimalPrice);
            }

            if (mobilePhoneFilter.MaximalPrice is not null)
            {
                sb.Append(" AND PriceAmount <= @MaximalPrice");
                @params.Add("MaximalPrice", mobilePhoneFilter.MaximalPrice);
            }

            if (!string.IsNullOrWhiteSpace(mobilePhoneFilter.BrandName))
            {
                sb.Append(" AND Brand = @BrandName");
                @params.Add("BrandName", mobilePhoneFilter.BrandName);
            }

            return @params;
        }
    }
}
