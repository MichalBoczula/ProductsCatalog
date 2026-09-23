using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Infrastructure.Contexts.Commands;
using Reqnroll;
using Shouldly;

namespace ProductCatalog.Acceptance.Tests.Features.Common;

[Binding]
public sealed class AcceptanceIsolationSteps(ScenarioApiContext apiContext)
{
    [Given("no Products phone or history has the name {string}")]
    public async Task GivenNoScenarioRecordHasTheName(string name)
    {
        apiContext.Factory.ShouldNotBeNull();
        using var scope = apiContext.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ProductsContext>();

        (await db.MobilePhones.AnyAsync(phone => phone.CommonDescription.Name == name)).ShouldBeFalse();
        (await db.MobilePhonesHistories.AnyAsync(history => history.Name == name)).ShouldBeFalse();
    }

    [Then("this Products scenario has one phone and history named {string}")]
    public async Task ThenTheScenarioHasTheNewRecordAndHistory(string name)
    {
        apiContext.Factory.ShouldNotBeNull();
        using var scope = apiContext.Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ProductsContext>();

        (await db.MobilePhones.CountAsync(phone => phone.CommonDescription.Name == name)).ShouldBe(1);
        (await db.MobilePhonesHistories.CountAsync(history => history.Name == name)).ShouldBe(1);
    }
}
