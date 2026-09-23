using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Shouldly;
using System.Diagnostics;
using System.Net;

namespace ProductCatalog.Acceptance.Tests.Configuration;

public sealed class SqlReadinessTests
{
    [Fact]
    public async Task UnreachableSql_DoesNotAffectLivenessAndReadinessIsBounded()
    {
        const string unreachableSql =
            "Server=127.0.0.1,1;Database=ProductsDb;User Id=sa;Password=NotUsed123!;TrustServerCertificate=True;Connect Timeout=30";

        using var factory = new ApplicationFactory(unreachableSql);
        using var client = factory.CreateClient();

        var registration = factory.Services.GetRequiredService<IOptions<HealthCheckServiceOptions>>()
            .Value.Registrations.Single(check => check.Name == "sql-server");
        registration.Timeout.ShouldBe(TimeSpan.FromSeconds(5));

        using var live = await client.GetAsync("/health/live");
        live.StatusCode.ShouldBe(HttpStatusCode.OK);

        var elapsed = Stopwatch.StartNew();
        using var ready = await client.GetAsync("/health/ready");
        elapsed.Stop();

        ready.StatusCode.ShouldBe(HttpStatusCode.ServiceUnavailable);
        elapsed.Elapsed.ShouldBeLessThan(TimeSpan.FromSeconds(8));
    }
}
