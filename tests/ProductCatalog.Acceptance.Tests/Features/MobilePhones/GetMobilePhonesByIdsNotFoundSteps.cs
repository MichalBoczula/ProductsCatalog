using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Application.Common.Dtos.Common;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using ProductCatalog.Application.Features.Common;
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;
using Reqnroll;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.MobilePhones
{
    [Binding]
    public class GetMobilePhonesByIdsNotFoundSteps
    {
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        private Guid _mobilePhoneId;
        private Guid _missingMobilePhoneId;
        private HttpResponseMessage? _response;

        [Given("an existing mobile phone and a missing mobile phone id")]
        public async Task GivenAnExistingMobilePhoneAndAMissingMobilePhoneId(Table table)
        {
            var categoryRequest = new CreateCategoryExternalDto($"MOBILE-{Guid.NewGuid():N}", "Mobile category");
            var categoryResponse = await TestRunHooks.Client.PostAsJsonAsync("/categories", categoryRequest, _jsonOptions);
            categoryResponse.EnsureSuccessStatusCode();

            var category = await categoryResponse.Content.ReadFromJsonAsync<CategoryDto>(_jsonOptions);
            category.ShouldNotBeNull();

            var values = table.Rows.ToDictionary(row => row["Field"], row => row["Value"], StringComparer.OrdinalIgnoreCase);
            var request = BuildMobilePhoneRequest(category.Id, values);
            AllureJson.AttachObject("Request JSON (create phone for get by ids not found)", request, _jsonOptions);

            var response = await TestRunHooks.Client.PostAsJsonAsync("/mobile-phones", request, _jsonOptions);
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();
            AllureJson.AttachRawJson($"Response JSON ({(int)response.StatusCode})", body);

            var mobilePhone = JsonSerializer.Deserialize<MobilePhoneDetailsDto>(body, _jsonOptions);
            mobilePhone.ShouldNotBeNull();
            _mobilePhoneId = mobilePhone.Id;
            _missingMobilePhoneId = Guid.NewGuid();
        }

        [When("I request the mobile phones by ids")]
        public async Task WhenIRequestTheMobilePhonesByIds(Table table)
        {
            var values = ParseTable(table);
            var ids = new List<Guid>
            {
                ResolvePlaceholder(GetValue(values, "ExistingMobilePhoneId")),
                ResolvePlaceholder(GetValue(values, "MissingMobilePhoneId"))
            };
            AllureJson.AttachObject("Request JSON (get by ids with missing phone)", ids, _jsonOptions);

            _response = await TestRunHooks.Client.PostAsJsonAsync("/mobile-phones/by-ids", ids, _jsonOptions);

            var body = await _response.Content.ReadAsStringAsync();
            AllureJson.AttachRawJson($"Response JSON ({(int)_response.StatusCode})", body);
        }

        [Then("the get mobile phones by ids response is not found")]
        public async Task ThenTheGetMobilePhonesByIdsResponseIsNotFound(Table table)
        {
            var expected = ParseTable(table);
            _response.ShouldNotBeNull();

            var expectedStatus = (HttpStatusCode)int.Parse(GetValue(expected, "StatusCode"), CultureInfo.InvariantCulture);
            _response.StatusCode.ShouldBe(expectedStatus);

            var problem = await _response.Content.ReadFromJsonAsync<NotFoundProblemDetails>(_jsonOptions);
            problem.ShouldNotBeNull();
            problem.Status.ShouldBe((int)expectedStatus);
            problem.Title.ShouldBe(GetValue(expected, "Title"));
            problem.Detail.ShouldBe(ReplacePlaceholders(GetValue(expected, "Detail")));
            problem.Instance.ShouldBe(ReplacePlaceholders(GetValue(expected, "Instance")));
            problem.TraceId.ShouldNotBeNullOrWhiteSpace();
        }

        private static CreateMobilePhoneExternalDto BuildMobilePhoneRequest(
            Guid categoryId,
            IReadOnlyDictionary<string, string> values)
        {
            return new CreateMobilePhoneExternalDto(
                new CommonDescriptionExtrernalDto(
                    GetValue(values, "Name"), GetValue(values, "Brand"), GetValue(values, "Description"),
                    GetValue(values, "MainPhoto"), ParseList(values, "OtherPhotos")),
                new CreateElectronicDetailsExternalDto(
                    GetValue(values, "CPU"), GetValue(values, "GPU"), GetValue(values, "Ram"),
                    GetValue(values, "Storage"), GetValue(values, "DisplayType"), ParseInt(values, "RefreshRateHz"),
                    ParseDecimal(values, "ScreenSizeInches"), ParseInt(values, "Width"), ParseInt(values, "Height"),
                    GetValue(values, "BatteryType"), ParseInt(values, "BatteryCapacity")),
                new CreateConnectivityExternalDto(
                    ParseBool(values, "Has5G"), ParseBool(values, "WiFi"),
                    ParseBool(values, "NFC"), ParseBool(values, "Bluetooth")),
                new CreateSatelliteNavigationSystemExternalDto(
                    ParseBool(values, "GPS"), ParseBool(values, "AGPS"), ParseBool(values, "Galileo"),
                    ParseBool(values, "GLONASS"), ParseBool(values, "QZSS")),
                new CreateSensorsExternalDto(
                    ParseBool(values, "Accelerometer"), ParseBool(values, "Gyroscope"),
                    ParseBool(values, "Proximity"), ParseBool(values, "Compass"),
                    ParseBool(values, "Barometer"), ParseBool(values, "Halla"),
                    ParseBool(values, "AmbientLight")),
                GetValue(values, "Camera"), ParseBool(values, "FingerPrint"), ParseBool(values, "FaceId"), categoryId,
                new CreateMoneyExternalDto(ParseDecimal(values, "PriceAmount"), GetValue(values, "PriceCurrency")),
                GetValue(values, "Description2"), GetValue(values, "Description3"));
        }

        private Guid ResolvePlaceholder(string value)
        {
            return value switch
            {
                "{MobilePhoneId}" => _mobilePhoneId,
                "{MissingMobilePhoneId}" => _missingMobilePhoneId,
                _ when Guid.TryParse(value, out var id) => id,
                _ => throw new InvalidOperationException($"Unknown mobile phone id placeholder '{value}'.")
            };
        }

        private string ReplacePlaceholders(string value)
        {
            return value
                .Replace("{MobilePhoneId}", _mobilePhoneId.ToString(), StringComparison.OrdinalIgnoreCase)
                .Replace("{MissingMobilePhoneId}", _missingMobilePhoneId.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        private static Dictionary<string, string> ParseTable(Table table)
        {
            return table.Rows.ToDictionary(row => row["Field"], row => row["Value"], StringComparer.OrdinalIgnoreCase);
        }

        private static string GetValue(IReadOnlyDictionary<string, string> values, string key)
        {
            return values.TryGetValue(key, out var value)
                ? value
                : throw new InvalidOperationException($"Missing '{key}' value in mobile phone table.");
        }

        private static bool ParseBool(IReadOnlyDictionary<string, string> values, string key) =>
            bool.Parse(GetValue(values, key));

        private static int ParseInt(IReadOnlyDictionary<string, string> values, string key) =>
            int.Parse(GetValue(values, key), CultureInfo.InvariantCulture);

        private static decimal ParseDecimal(IReadOnlyDictionary<string, string> values, string key) =>
            decimal.Parse(GetValue(values, key), CultureInfo.InvariantCulture);

        private static List<string> ParseList(IReadOnlyDictionary<string, string> values, string key) =>
            GetValue(values, key).Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
    }
}
