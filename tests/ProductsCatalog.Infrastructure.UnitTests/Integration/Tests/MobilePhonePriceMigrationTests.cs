using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Infrastructure.Contexts.Commands;
using ProductCatalog.Infrastructure.Migrations;
using ProductsCatalog.Infrastructure.UnitTests.Integration.Configuration;
using Shouldly;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace ProductsCatalog.Infrastructure.UnitTests.Integration.Tests
{
    public class MobilePhonePriceMigrationTests : IClassFixture<MsSqlDbTestFixture>
    {
        private const string PreviousMigration = "20260922220000_RemoveCatalogs";
        private const string PriceMigration = "20261101180000_BackfillMobilePhonePrices";
        private static readonly Guid Iphone15Id = Guid.Parse("8d5f9e61-8a71-4d9a-9d86-3f4ef0c5ed4a");
        private static readonly Guid Iphone16Id = Guid.Parse("5b8b2f19-4f6b-4aa7-8a49-1d5f1fd3a7d2");
        private static readonly Guid Iphone17ProId = Guid.Parse("2a3c3fa7-3ffb-4df1-9454-5c243fcbfc7b");
        private static readonly Guid PocoId = Guid.Parse("0f62c3e1-8e3e-4b1f-9d74-3d6e2ff2c6d2");
        private static readonly IReadOnlyDictionary<Guid, decimal> ExpectedPrices = new Dictionary<Guid, decimal>
        {
            [PocoId] = 2499m,
            [Guid.Parse("a243cf6f-dde1-4c05-8a58-2fa7f04ed984")] = 999m,
            [Guid.Parse("5e9d3b34-8005-4b73-9e90-1cc25f8f5c34")] = 1999m,
            [Guid.Parse("07d39209-33b8-4274-b82d-2f0c4f341a9a")] = 2999m,
            [Iphone15Id] = 2799m,
            [Iphone16Id] = 3299m,
            [Guid.Parse("3cf6e155-2a07-40e3-9c40-9be5a4c97d6c")] = 3999m,
            [Iphone17ProId] = 5499m,
            [Guid.Parse("b7a2a7c1-3caa-4de0-8f3b-0b3bdcc55d5f")] = 2499m,
            [Guid.Parse("3c1c2b2e-0a63-4a6d-8f0c-5d3a1e8f9a21")] = 3499m,
            [Guid.Parse("2c1d2f69-3a9f-4c51-8a6b-8f2c5d6b7a10")] = 1199m,
            [Guid.Parse("f2c7d5b1-4a9e-4a1a-8e3c-9b2f1d7c6a55")] = 1599m,
            [Guid.Parse("c9b7d8e1-0f4a-4d3b-9a1c-2e6f7b8c9d10")] = 599m,
            [Guid.Parse("d7b0f3c2-4a6f-4b2d-8c1e-1f2a3b4c5d6e")] = 3999m,
            [Guid.Parse("a5f1c3d7-2b9e-4f6a-8c1d-0e9b7a6c5d4f")] = 1299m
        };

        private readonly MsSqlDbTestFixture _fixture;

        public MobilePhonePriceMigrationTests(MsSqlDbTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task FreshDatabase_ShouldPriceAllSeedsAndPreserveOriginalHistory()
        {
            await using var context = CreateContext(_fixture.ConnectionString);
            var phones = await context.MobilePhones.AsNoTracking().ToListAsync();
            var history = await context.MobilePhonesHistories.AsNoTracking().ToListAsync();

            phones.Select(phone => phone.Id).ShouldBe(ExpectedPrices.Keys, ignoreOrder: true);
            history.Count.ShouldBe(26);
            history.Count(entry => entry.Operation == Operation.Inserted).ShouldBe(15);
            history.Count(entry => entry.Operation == Operation.Updated).ShouldBe(11);

            foreach (var phone in phones)
            {
                phone.Price.Amount.ShouldBe(ExpectedPrices[phone.Id]);
                phone.Price.Currency.ShouldBe("PLN");
                var original = history.Single(entry => entry.MobilePhoneId == phone.Id && entry.Operation == Operation.Inserted);
                var latest = history.Where(entry => entry.MobilePhoneId == phone.Id).OrderByDescending(entry => entry.ChangedAt).First();
                AssertHistoryMatches(phone, latest);
                if (latest.Operation == Operation.Updated)
                {
                    original.PriceAmount.ShouldBe(0m);
                    latest.ChangedAt.ShouldBeGreaterThan(original.ChangedAt);
                }
                else
                {
                    original.PriceAmount.ShouldBe(phone.Price.Amount);
                }
            }
        }

        [Fact]
        public async Task DeploymentScript_ShouldUpgradeExistingDataAndPreserveCustomAndUnrelatedRecords()
        {
            await InLegacyDatabase(async context =>
            {
                var source = await context.MobilePhones.AsNoTracking().SingleAsync(phone => phone.Id == Iphone15Id);
                // Same product name, different ID: never treat an unrelated row as a seed.
                var unrelated = new MobilePhone(source.CommonDescription, source.ElectronicDetails,
                    source.Connectivity, source.SatelliteNavigationSystems, source.Sensors,
                    source.Camera, source.FingerPrint, source.FaceId, source.Price,
                    source.Description2, source.Description3);
                context.MobilePhones.Add(unrelated);
                await context.SaveChangesAsync();
                context.ChangeTracker.Clear();

                var futureTimestamp = new DateTime(2099, 1, 1);
                await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE dbo.TB_MobilePhones SET PriceAmount = {1234.56m} WHERE Id = {Iphone16Id}");
                await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE dbo.TB_MobilePhones SET PriceCurrency = {"EUR"} WHERE Id = {Iphone15Id}");
                await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE dbo.TB_MobilePhones SET PriceAmount = {0m}, ChangedAt = {futureTimestamp} WHERE Id = {PocoId}");
                await context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM dbo.TB_MobilePhones WHERE Id = {Iphone17ProId}");
                var before = await context.MobilePhones.AsNoTracking().ToDictionaryAsync(phone => phone.Id);
                var originalHistory = await context.MobilePhonesHistories.AsNoTracking().OrderBy(entry => entry.Id).ToListAsync();

                await ApplyDeploymentScript(context);

                var after = await context.MobilePhones.AsNoTracking().ToDictionaryAsync(phone => phone.Id);
                after.Keys.ShouldBe(before.Keys, ignoreOrder: true);
                after.ShouldNotContainKey(Iphone17ProId);
                var history = await context.MobilePhonesHistories.AsNoTracking().ToListAsync();
                var originalIds = originalHistory.Select(entry => entry.Id).ToHashSet();
                history.Where(entry => originalIds.Contains(entry.Id)).OrderBy(entry => entry.Id).ShouldBe(originalHistory);
                var corrections = history.Where(entry => !originalIds.Contains(entry.Id)).ToList();
                corrections.Count.ShouldBe(9);

                foreach (var (id, oldPhone) in before)
                {
                    var phone = after[id];
                    if (corrections.Any(entry => entry.MobilePhoneId == id))
                    {
                        oldPhone.Price.Amount.ShouldBe(0m);
                        phone.Price.Amount.ShouldBe(ExpectedPrices[id]);
                        phone.Price.Currency.ShouldBe("PLN");
                        phone.ChangedAt.ShouldBeGreaterThan(oldPhone.ChangedAt);
                        WithoutPriceAndChangeDate(phone).ShouldBe(WithoutPriceAndChangeDate(oldPhone));
                        var correction = corrections.Single(entry => entry.MobilePhoneId == id);
                        correction.Operation.ShouldBe(Operation.Updated);
                        AssertHistoryMatches(phone, correction);
                    }
                    else
                    {
                        JsonSerializer.Serialize(phone).ShouldBe(JsonSerializer.Serialize(oldPhone));
                    }
                }

                after[Iphone16Id].Price.Amount.ShouldBe(1234.56m);
                after[Iphone15Id].Price.Currency.ShouldBe("EUR");
                after[unrelated.Id].Price.Amount.ShouldBe(0m);
                after[PocoId].ChangedAt.ShouldBe(futureTimestamp.AddTicks(1));
                var once = await Snapshot(context);
                await ApplyDeploymentScript(context);
                (await Snapshot(context)).ShouldBe(once);
            });
        }

        [Fact]
        public async Task ReapplyingMigrationsAndEmbeddedSql_ShouldNotChangePricesTimestampsOrHistory()
        {
            await using var context = CreateContext(_fixture.ConnectionString);
            var before = await Snapshot(context);
            await context.Database.MigrateAsync();
            const string resourceName = "ProductCatalog.Infrastructure.Migrations.Scripts.20261101180000_BackfillMobilePhonePrices.sql";
            using var stream = typeof(BackfillMobilePhonePrices).Assembly.GetManifestResourceStream(resourceName);
            stream.ShouldNotBeNull();
            using var reader = new StreamReader(stream);
            await context.Database.ExecuteSqlRawAsync(await reader.ReadToEndAsync());
            (await Snapshot(context)).ShouldBe(before);
        }

        [Fact]
        public async Task FailedHistoryWrite_ShouldRollbackCorrectionAndAllowRetry()
        {
            await InLegacyDatabase(async context =>
            {
                await context.Database.ExecuteSqlRawAsync("ALTER TABLE dbo.TB_MobilePhones_History WITH NOCHECK ADD CONSTRAINT CK_Test_RejectPriceHistory CHECK (Operation = 0)");
                var before = await Snapshot(context);

                await Should.ThrowAsync<SqlException>(() => context.Database.MigrateAsync());
                (await Snapshot(context)).ShouldBe(before);

                await context.Database.ExecuteSqlRawAsync("ALTER TABLE dbo.TB_MobilePhones_History DROP CONSTRAINT CK_Test_RejectPriceHistory");
                await context.Database.MigrateAsync();
                (await context.Database.GetAppliedMigrationsAsync()).ShouldContain(PriceMigration);
                (await context.MobilePhonesHistories.CountAsync()).ShouldBe(26);
                (await context.MobilePhones.CountAsync(phone => phone.Price.Amount == 0)).ShouldBe(0);
            });
        }

        private async Task InLegacyDatabase(Func<ProductsContext, Task> test)
        {
            var connectionString = new SqlConnectionStringBuilder(_fixture.ConnectionString)
            {
                InitialCatalog = "PriceMigration" + Guid.NewGuid().ToString("N")
            }.ConnectionString;
            await using var context = CreateContext(connectionString);
            try
            {
                await context.GetService<IMigrator>().MigrateAsync(PreviousMigration);
                await test(context);
            }
            finally
            {
                await context.Database.EnsureDeletedAsync();
            }
        }

        private static ProductsContext CreateContext(string connectionString) =>
            new(new DbContextOptionsBuilder<ProductsContext>().UseSqlServer(connectionString).Options);

        private static async Task ApplyDeploymentScript(ProductsContext context)
        {
            var script = context.GetService<IMigrator>().GenerateScript(PreviousMigration, PriceMigration,
                MigrationsSqlGenerationOptions.Idempotent);
            await context.Database.OpenConnectionAsync();
            try
            {
                foreach (var batch in Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase))
                {
                    if (!string.IsNullOrWhiteSpace(batch))
                    {
                        await context.Database.ExecuteSqlRawAsync(batch);
                    }
                }
            }
            finally
            {
                await context.Database.CloseConnectionAsync();
            }
        }

        private static async Task<string> Snapshot(ProductsContext context) => JsonSerializer.Serialize(new
        {
            Current = await context.MobilePhones.AsNoTracking().OrderBy(phone => phone.Id).ToListAsync(),
            History = await context.MobilePhonesHistories.AsNoTracking().OrderBy(entry => entry.Id).ToListAsync(),
            Migrations = await context.Database.GetAppliedMigrationsAsync()
        });

        private static string WithoutPriceAndChangeDate(MobilePhone phone)
        {
            var json = JsonSerializer.SerializeToNode(phone)!.AsObject();
            json.Remove(nameof(MobilePhone.Price));
            json.Remove(nameof(MobilePhone.ChangedAt));
            return json.ToJsonString();
        }

        private static void AssertHistoryMatches(MobilePhone phone, MobilePhonesHistory history)
        {
            // Flatten every aggregate value object and compare the entire saved history snapshot.
            var expected = new JsonObject();
            foreach (var (name, value) in JsonSerializer.SerializeToNode(phone)!.AsObject())
            {
                if (name == nameof(MobilePhone.Price))
                {
                    expected[nameof(MobilePhonesHistory.PriceAmount)] = value!["Amount"]!.DeepClone();
                    expected[nameof(MobilePhonesHistory.PriceCurrency)] = value["Currency"]!.DeepClone();
                }
                else if (value is JsonObject nested)
                {
                    foreach (var (property, field) in nested)
                    {
                        expected[property] = field?.DeepClone();
                    }
                }
                else
                {
                    expected[name == nameof(MobilePhone.Id) ? nameof(MobilePhonesHistory.MobilePhoneId) : name] = value?.DeepClone();
                }
            }

            var actual = JsonSerializer.SerializeToNode(history)!.AsObject();
            actual.Remove(nameof(MobilePhonesHistory.Id));
            actual.Remove(nameof(MobilePhonesHistory.Operation));
            JsonNode.DeepEquals(actual, expected).ShouldBeTrue();
        }
    }
}
