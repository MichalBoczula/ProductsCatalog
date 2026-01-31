using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Application.Common.Dtos.Common;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using ProductCatalog.Application.Features.Common;
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;
using ProductCatalog.Infrastructure.Contexts.Commands;
using Reqnroll;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Globalization;

namespace ProductCatalog.Acceptance.Tests.Features.MobilePhones
{
    [Binding]
    public class GetMobilePhonesByFilterSteps
    {
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
        private readonly List<MobilePhoneDetailsDto> _createdMobilePhones = new();
        private List<MobilePhoneDto> _result = new();
        private HttpResponseMessage? _response;
        private Guid _categoryId;
        private int _amount;

        [Given("an existing list of mobile phones")]
        public async Task GivenAnExistingListOfMobilePhones(Table table)
        {
            var categoryCode = $"MOBILE-{Guid.NewGuid():N}";
            var categoryRequest = new CreateCategoryExternalDto(categoryCode, "Mobile category");
            var categoryResponse = await TestRunHooks.Client.PostAsJsonAsync("/categories", categoryRequest);
            categoryResponse.EnsureSuccessStatusCode();

            var category = await categoryResponse.Content.ReadFromJsonAsync<CategoryDto>(_jsonOptions);
            category.ShouldNotBeNull();

            _categoryId = category!.Id;

            var requests = new List<CreateMobilePhoneExternalDto>
            {
                BuildMobilePhoneRequest("Filter Phone 1", table),
                BuildMobilePhoneRequest("Filter Phone 2", table),
                BuildMobilePhoneRequest("Filter Phone 3", table)
            };

            foreach (var request in requests)
            {
                var response = await TestRunHooks.Client.PostAsJsonAsync("/mobile-phones", request);
                response.EnsureSuccessStatusCode();

                var created = await response.Content.ReadFromJsonAsync<MobilePhoneDetailsDto>(_jsonOptions);
                created.ShouldNotBeNull();
                _createdMobilePhones.Add(created!);
            }
        }

        [Given("no mobile phones exist in the database")]
        public async Task GivenNoMobilePhonesExistInTheDatabase()
        {
            using var scope = TestRunHooks.Factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ProductsContext>();

            context.MobilePhonesHistories.RemoveRange(context.MobilePhonesHistories);
            context.MobilePhones.RemoveRange(context.MobilePhones);

            await context.SaveChangesAsync();
        }

        [When("I request mobile phones with amount {int}")]
        public async Task WhenIRequestMobilePhonesWithAmount(int amount)
        {
            _amount = amount;
            _response = await TestRunHooks.Client.GetAsync($"/mobile-phones?amount={amount}");
        }

        [Then("the mobile phone list is returned with the requested amount")]
        public async Task ThenTheMobilePhoneListIsReturnedWithTheRequestedAmount(Table table)
        {
            var expected = ParseExpectedTable(table);
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(ParseStatusCode(expected, "StatusCode"));

            _result = await DeserializeResponse<List<MobilePhoneDto>>(_response) ?? new List<MobilePhoneDto>();
            var expectedAmount = expected.TryGetValue("Amount", out var amountValue)
                ? int.Parse(amountValue, CultureInfo.InvariantCulture)
                : _amount;
            _result.Count.ShouldBe(expectedAmount);

            if (expected.TryGetValue("NamePrefix", out var namePrefix))
            {
                _result.ShouldAllBe(mobilePhone => mobilePhone.Name.StartsWith(namePrefix, StringComparison.OrdinalIgnoreCase));
            }

            if (expected.TryGetValue("DisplayType", out var displayType))
            {
                _result.ShouldAllBe(mobilePhone => mobilePhone.DisplayType.Equals(displayType, StringComparison.OrdinalIgnoreCase));
            }

            if (expected.TryGetValue("ScreenSizeInches", out var screenSize))
            {
                var expectedScreenSize = decimal.Parse(screenSize, CultureInfo.InvariantCulture);
                _result.ShouldAllBe(mobilePhone => mobilePhone.ScreenSizeInches == expectedScreenSize);
            }

            if (expected.TryGetValue("Camera", out var camera))
            {
                _result.ShouldAllBe(mobilePhone => mobilePhone.Camera.Equals(camera, StringComparison.OrdinalIgnoreCase));
            }

            if (expected.TryGetValue("PriceAmount", out var priceAmount))
            {
                var expectedPriceAmount = decimal.Parse(priceAmount, CultureInfo.InvariantCulture);
                _result.ShouldAllBe(mobilePhone => mobilePhone.Price.Amount == expectedPriceAmount);
            }

            if (expected.TryGetValue("PriceCurrency", out var priceCurrency))
            {
                _result.ShouldAllBe(mobilePhone => mobilePhone.Price.Currency.Equals(priceCurrency, StringComparison.OrdinalIgnoreCase));
            }
        }

        [Then("an empty mobile phone list is returned")]
        public async Task ThenAnEmptyMobilePhoneListIsReturned()
        {
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(HttpStatusCode.OK);

            _result = await DeserializeResponse<List<MobilePhoneDto>>(_response) ?? new List<MobilePhoneDto>();
            _result.ShouldBeEmpty();
        }

        private CreateMobilePhoneExternalDto BuildMobilePhoneRequest(string name, Table? table)
        {
            var values = MergeDefaultValues(table);
            values["Name"] = name;
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
                ["Name"] = "Filter Phone",
                ["Brand"] = "Brand",
                ["Description"] = "Phone created by filter test",
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
                throw new InvalidOperationException($"Missing '{key}' value in mobile phone contract table.");
            }

            return value;
        }

        private static Dictionary<string, string> ParseExpectedTable(Table table)
        {
            var expected = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var row in table.Rows)
            {
                expected[row["Field"]] = row["Value"];
            }

            return expected;
        }

        private static HttpStatusCode ParseStatusCode(IReadOnlyDictionary<string, string> expected, string key)
        {
            return (HttpStatusCode)int.Parse(GetRequiredValue(expected, key), CultureInfo.InvariantCulture);
        }

        private static string GetRequiredValue(IReadOnlyDictionary<string, string> expected, string key)
        {
            if (!expected.TryGetValue(key, out var value))
            {
                throw new InvalidOperationException($"Missing '{key}' value in expected mobile phone response table.");
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
