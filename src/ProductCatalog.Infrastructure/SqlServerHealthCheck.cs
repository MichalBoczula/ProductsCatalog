using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ProductCatalog.Infrastructure.Common;
using ProductCatalog.Infrastructure.Contexts.Commands;

namespace ProductCatalog.Infrastructure;

internal sealed class SqlServerHealthCheck(ProductsContext context) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext healthCheckContext,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await context.Database.ExecuteSqlRawAsync($"""
                SELECT TOP (0) Id FROM [dbo].[{SqlTableNames.MobilePhones}];
                SELECT TOP (0) Id FROM [dbo].[{SqlTableNames.MobilePhonesHistory}];
                """, cancellationToken);

            return HealthCheckResult.Healthy("SQL Server catalog is ready.");
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy("SQL Server catalog is not ready.", exception);
        }
    }
}
