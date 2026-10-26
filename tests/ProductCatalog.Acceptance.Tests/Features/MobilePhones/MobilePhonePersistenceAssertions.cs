using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Infrastructure.Contexts.Commands;
using Reqnroll;
using Shouldly;
using System.Globalization;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.MobilePhones;

internal static class MobilePhonePersistenceAssertions
{
    public static async Task AssertSaved(ScenarioApiContext api, Guid id, Table table, MobilePhoneDetailsDto? original = null)
    {
        var expected = table.Rows.ToDictionary(row => row["Field"], row => row["Value"]);
        using var scope = api.Factory!.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ProductsContext>();
        var current = await db.MobilePhones.AsNoTracking().Where(phone => phone.Id == id).ToListAsync();
        var history = await db.MobilePhonesHistories.AsNoTracking().Where(entry => entry.MobilePhoneId == id)
            .OrderBy(entry => entry.ChangedAt).ThenBy(entry => entry.Id).ToListAsync();

        // Attach the actual SQL state before assertions, including evidence when an assertion fails.
        AllureJson.AttachObject("Saved catalogue state and history", new
        {
            current = current.Select(phone => new
            {
                phone.Id,
                phone.IsActive,
                phone.ChangedAt,
                phone.CommonDescription.Name,
                phone.CommonDescription.MainPhoto,
                phone.Price
            }),
            history = history.Select(entry => new
            {
                entry.Id,
                entry.MobilePhoneId,
                entry.Operation,
                entry.IsActive,
                entry.ChangedAt,
                entry.Name,
                entry.MainPhoto,
                entry.PriceAmount,
                entry.PriceCurrency
            })
        }, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        current.Count.ShouldBe(int.Parse(expected["CurrentRows"], CultureInfo.InvariantCulture));
        var saved = current.Single();
        saved.CommonDescription.Name.ShouldBe(expected["Name"]);
        saved.CommonDescription.MainPhoto.ShouldBe(expected["MainPhoto"]);
        saved.IsActive.ShouldBe(bool.Parse(expected["IsActive"]));
        saved.ChangedAt.ShouldNotBe(default);
        var operations = expected["HistoryOperations"].Split(',', StringSplitOptions.TrimEntries)
            .Select(value => Enum.Parse<Operation>(value)).ToArray();
        history.Select(entry => entry.Operation).ShouldBe(operations);
        history.Select(entry => entry.Id).Distinct().Count().ShouldBe(history.Count);
        var latest = history.Last();
        latest.Name.ShouldBe(expected["Name"]);
        latest.MainPhoto.ShouldBe(expected["MainPhoto"]);
        latest.IsActive.ShouldBe(bool.Parse(expected["IsActive"]));
        latest.PriceAmount.ShouldBe(saved.Price.Amount);
        latest.PriceCurrency.ShouldBe(saved.Price.Currency);
        latest.ChangedAt.ShouldBe(saved.ChangedAt);
        if (original is not null)
        {
            var inserted = history.First();
            inserted.Name.ShouldBe(original.CommonDescription.Name);
            inserted.MainPhoto.ShouldBe(original.CommonDescription.MainPhoto);
            inserted.PriceAmount.ShouldBe(original.Price.Amount);
            inserted.PriceCurrency.ShouldBe(original.Price.Currency);
            inserted.IsActive.ShouldBeTrue();
            latest.ChangedAt.ShouldBeGreaterThan(inserted.ChangedAt);
        }
    }
}
