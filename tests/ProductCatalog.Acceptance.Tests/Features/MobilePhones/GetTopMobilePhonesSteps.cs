using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Application.Common.Dtos.Common;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using ProductCatalog.Application.Features.Common;
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;
using ProductCatalog.Infrastructure.Contexts.Commands;
using Reqnroll;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.MobilePhones
{
    [Binding]
    public class GetTopMobilePhonesSteps
    {
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        private readonly List<MobilePhoneDetailsDto> _createdMobilePhones = new();
        private HttpResponseMessage? _response;
        private Guid _categoryId;

        [Given("an existing set of mobile phones for top list")]
        public async Task GivenAnExistingSetOfMobilePhonesForTopList(Table table)
        {
            await ClearMobilePhones();
            await EnsureCategoryExists();

            var values = MergeDefaultValues(table);
            var baseName = GetValue(values, "Name");

            for (var i = 1; i <= 3; i++)
            {
                values["Name"] = $"{baseName} {i}";
                var request = BuildMobilePhoneRequest(values);

                AllureJson.AttachObject("Request JSON (create for top)", request, _jsonOptions);

                var response = await TestRunHooks.Client.PostAsJsonAsync("/mobile-phones", request, _jsonOptions);
                response.EnsureSuccessStatusCode();

                var body = await response.Content.ReadAsStringAsync();
                AllureJson.AttachRawJson($"Response JSON ({(int)response.StatusCode})", body);

                var created = JsonSerializer.Deserialize<MobilePhoneDetailsDto>(body, _jsonOptions);
                created.ShouldNotBeNull();

                _createdMobilePhones.Add(created!);
            }
        }

        [Given("mobile phones table is empty for top list")]
        public async Task GivenNoMobilePhonesExistInTheDatabase()
        {
            await ClearMobilePhones();
        }

        [When("I request the top mobile phones list")]
        public async Task WhenIRequestTheTopMobilePhonesList()
        {
            AllureJson.AttachObject(
                "Request JSON (get top)",
                new { Endpoint = "/mobile-phones/top" },
                _jsonOptions);

            _response = await TestRunHooks.Client.GetAsync("/mobile-phones/top");

            var body = await _response.Content.ReadAsStringAsync();
            AllureJson.AttachRawJson($"Response JSON ({(int)_response.StatusCode})", body);
        }

        [Then("the top mobile phones response is successful and contains records")]
        public async Task ThenTheTopMobilePhonesResponseIsSuccessfulAndContainsRecords()
        {
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(HttpStatusCode.OK);

            var result = await DeserializeResponse<List<TopMobilePhoneDto>>(_response);
            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThan(0);

            foreach (var created in _createdMobilePhones)
            {
                result.Any(item => item.Id == created.Id).ShouldBeTrue();
            }
        }

        [Then("the top mobile phones response is not found")]
        public async Task ThenTheTopMobilePhonesResponseIsNotFound()
        {
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(HttpStatusCode.NotFound);

            var problem = await DeserializeResponse<NotFoundProblemDetails>(_response);
            problem.ShouldNotBeNull();
            problem.Status.ShouldBe((int)HttpStatusCode.NotFound);
            problem.Title.ShouldBe("Resource not found.");
            problem.Instance.ShouldBe("/mobile-phones/top");
            problem.TraceId.ShouldNotBeNullOrWhiteSpace();
        }

        private async Task EnsureCategoryExists()
        {
            var categoryCode = $"MOBILE-{Guid.NewGuid():N}";
            var categoryRequest = new CreateCategoryExternalDto(categoryCode, "Mobile category");

            var categoryResponse = await TestRunHooks.Client.PostAsJsonAsync("/categories", categoryRequest);
            categoryResponse.EnsureSuccessStatusCode();

            var category = await categoryResponse.Content.ReadFromJsonAsync<CategoryDto>(_jsonOptions);
            category.ShouldNotBeNull();
            _categoryId = category!.Id;
        }

        private async Task ClearMobilePhones()
        {
            using var scope = TestRunHooks.Factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ProductsContext>();

            context.MobilePhonesHistories.RemoveRange(context.MobilePhonesHistories);
            context.MobilePhones.RemoveRange(context.MobilePhones);

            await context.SaveChangesAsync();
        }

        private CreateMobilePhoneExternalDto BuildMobilePhoneRequest(IReadOnlyDictionary<string, string> values)
        {
            return new CreateMobilePhoneExternalDto(
                new CommonDescriptionExtrernalDto(
                    GetValue(values, "Name"),
                    GetValue(values, "Brand"),
                    GetValue(values, "Description"),
                    GetValue(values, "MainPhoto"),
                    ParseList(values, "OtherPhotos")),
                new CreateElectronicDetailsExternalDto(
                    GetValue(values, "CPU"),
                    GetValue(values, "GPU"),
                    GetValue(values, "Ram"),
                    GetValue(values, "Storage"),
                    GetValue(values, "DisplayType"),
                    ParseInt(values, "RefreshRateHz"),
                    ParseDecimal(values, "ScreenSizeInches"),
                    ParseInt(values, "Width"),
                    ParseInt(values, "Height"),
                    GetValue(values, "BatteryType"),
                    ParseInt(values, "BatteryCapacity")),
                new CreateConnectivityExternalDto(
                    ParseBool(values, "Has5G"),
                    ParseBool(values, "WiFi"),
                    ParseBool(values, "NFC"),
                    ParseBool(values, "Bluetooth")),
                new CreateSatelliteNavigationSystemExternalDto(
                    ParseBool(values, "GPS"),
                    ParseBool(values, "AGPS"),
                    ParseBool(values, "Galileo"),
                    ParseBool(values, "GLONASS"),
                    ParseBool(values, "QZSS")),
                new CreateSensorsExternalDto(
                    ParseBool(values, "Accelerometer"),
                    ParseBool(values, "Gyroscope"),
                    ParseBool(values, "Proximity"),
                    ParseBool(values, "Compass"),
                    ParseBool(values, "Barometer"),
                    ParseBool(values, "Halla"),
                    ParseBool(values, "AmbientLight")),
                GetValue(values, "Camera"),
                ParseBool(values, "FingerPrint"),
                ParseBool(values, "FaceId"),
                _categoryId,
                new CreateMoneyExternalDto(
                    ParseDecimal(values, "PriceAmount"),
                    GetValue(values, "PriceCurrency")),
                GetValue(values, "Description2"),
                GetValue(values, "Description3"));
        }

        private static Dictionary<string, string> MergeDefaultValues(Table? table)
        {
            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Name"] = "Top Phone",
                ["Brand"] = "Brand",
                ["Description"] = "Phone created by top endpoint acceptance test",
                ["MainPhoto"] = "main-photo.jpg",
                ["OtherPhotos"] = "photo-1.jpg, photo-2.jpg",
                ["CPU"] = "Octa-core",
                ["GPU"] = "Adreno",
                ["Ram"] = "8GB",
                ["Storage"] = "256GB",
                ["DisplayType"] = "OLED",
                ["RefreshRateHz"] = "120",
                ["ScreenSizeInches"] = "6.4",
                ["Width"] = "72",
                ["Height"] = "152",
                ["BatteryType"] = "Li-Ion",
                ["BatteryCapacity"] = "4500",
                ["Has5G"] = "true",
                ["WiFi"] = "true",
                ["NFC"] = "true",
                ["Bluetooth"] = "true",
                ["GPS"] = "true",
                ["AGPS"] = "true",
                ["Galileo"] = "true",
                ["GLONASS"] = "true",
                ["QZSS"] = "true",
                ["Accelerometer"] = "true",
                ["Gyroscope"] = "true",
                ["Proximity"] = "true",
                ["Compass"] = "true",
                ["Barometer"] = "true",
                ["Halla"] = "false",
                ["AmbientLight"] = "true",
                ["Camera"] = "camera",
                ["FingerPrint"] = "true",
                ["FaceId"] = "true",
                ["PriceAmount"] = "799.99",
                ["PriceCurrency"] = "USD",
                ["Description2"] = "desc2",
                ["Description3"] = "desc3"
            };

            if (table is null)
            {
                return values;
            }

            foreach (var row in table.Rows)
            {
                values[row["Field"]] = row["Value"];
            }

            return values;
        }

        private static string GetValue(IReadOnlyDictionary<string, string> values, string key)
        {
            if (!values.TryGetValue(key, out var value))
            {
                throw new InvalidOperationException($"Missing '{key}' value in top mobile phone contract table.");
            }

            return value;
        }

        private static bool ParseBool(IReadOnlyDictionary<string, string> values, string key)
        {
            return bool.Parse(GetValue(values, key));
        }

        private static int ParseInt(IReadOnlyDictionary<string, string> values, string key)
        {
            return int.Parse(GetValue(values, key), CultureInfo.InvariantCulture);
        }

        private static decimal ParseDecimal(IReadOnlyDictionary<string, string> values, string key)
        {
            return decimal.Parse(GetValue(values, key), CultureInfo.InvariantCulture);
        }

        private static IReadOnlyList<string> ParseList(IReadOnlyDictionary<string, string> values, string key)
        {
            return GetValue(values, key)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }

        private async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }
    }
}
