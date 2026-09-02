using ProductCatalog.Acceptance.Tests.Features.Common;
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
    public class GetMobilePhonesByIdsSteps
    {
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        private readonly List<MobilePhoneDetailsDto> _createdMobilePhones = new();
        private HttpResponseMessage? _response;
        private Guid? _categoryId;

        [Given("an existing mobile phone for the by ids request")]
        public async Task GivenAnExistingMobilePhoneForTheByIdsRequest(Table table)
        {
            await EnsureCategoryExists();
            var values = table.Rows.ToDictionary(row => row["Field"], row => row["Value"], StringComparer.OrdinalIgnoreCase);
            var request = BuildMobilePhoneRequest(_categoryId!.Value, values);
            AllureJson.AttachObject("Request JSON (create phone for get by ids)", request, _jsonOptions);

            var response = await TestRunHooks.Client.PostAsJsonAsync("/mobile-phones", request, _jsonOptions);
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();
            AllureJson.AttachRawJson($"Response JSON ({(int)response.StatusCode})", body);

            var phone = JsonSerializer.Deserialize<MobilePhoneDetailsDto>(body, _jsonOptions);
            phone.ShouldNotBeNull();
            _createdMobilePhones.Add(phone);
        }

        [When("I request the mobile phones by their ids")]
        public async Task WhenIRequestTheMobilePhonesByTheirIds(Table table)
        {
            var ids = table.Rows
                .Select(row => ResolveMobilePhoneId(row["Id"]))
                .ToList();
            AllureJson.AttachObject("Request JSON (get by ids)", ids, _jsonOptions);

            _response = await TestRunHooks.Client.PostAsJsonAsync("/mobile-phones/by-ids", ids, _jsonOptions);

            var body = await _response.Content.ReadAsStringAsync();
            AllureJson.AttachRawJson($"Response JSON ({(int)_response.StatusCode})", body);
        }

        [Then("the requested mobile phones are returned successfully")]
        public async Task ThenTheRequestedMobilePhonesAreReturnedSuccessfully(Table table)
        {
            _response.ShouldNotBeNull();
            var expectedRows = table.Rows.ToList();
            var expectedStatusCode = (HttpStatusCode)int.Parse(expectedRows[0]["StatusCode"], CultureInfo.InvariantCulture);
            _response!.StatusCode.ShouldBe(expectedStatusCode);

            var phones = await _response.Content.ReadFromJsonAsync<List<MobilePhoneDto>>(_jsonOptions);
            phones.ShouldNotBeNull();
            phones.Count.ShouldBe(expectedRows.Count);

            foreach (var expected in expectedRows)
            {
                var expectedId = ResolveMobilePhoneId(expected["Id"]);
                var phone = phones.SingleOrDefault(candidate => candidate.Id == expectedId);
                phone.ShouldNotBeNull();
                phone.Name.ShouldBe(expected["Name"]);
                phone.Brand.ShouldBe(expected["Brand"]);
                phone.DisplayType.ShouldBe(expected["DisplayType"]);
                phone.ScreenSizeInches.ShouldBe(decimal.Parse(expected["ScreenSizeInches"], CultureInfo.InvariantCulture));
                phone.Camera.ShouldBe(expected["Camera"]);
                phone.Price.Amount.ShouldBe(decimal.Parse(expected["PriceAmount"], CultureInfo.InvariantCulture));
                phone.Price.Currency.ShouldBe(expected["PriceCurrency"]);
            }
        }

        private Guid ResolveMobilePhoneId(string placeholder)
        {
            return placeholder switch
            {
                "{FirstMobilePhoneId}" => GetCreatedMobilePhoneId(0, placeholder),
                "{SecondMobilePhoneId}" => GetCreatedMobilePhoneId(1, placeholder),
                _ when Guid.TryParse(placeholder, out var id) => id,
                _ => throw new InvalidOperationException($"Unknown mobile phone id placeholder '{placeholder}'.")
            };
        }

        private Guid GetCreatedMobilePhoneId(int index, string placeholder)
        {
            if (_createdMobilePhones.Count <= index)
            {
                throw new InvalidOperationException($"Mobile phone for placeholder '{placeholder}' has not been created.");
            }

            return _createdMobilePhones[index].Id;
        }

        private async Task EnsureCategoryExists()
        {
            if (_categoryId.HasValue)
            {
                return;
            }

            var categoryRequest = new CreateCategoryExternalDto($"MOBILE-{Guid.NewGuid():N}", "Mobile category");
            var categoryResponse = await TestRunHooks.Client.PostAsJsonAsync("/categories", categoryRequest, _jsonOptions);
            categoryResponse.EnsureSuccessStatusCode();

            var category = await categoryResponse.Content.ReadFromJsonAsync<CategoryDto>(_jsonOptions);
            category.ShouldNotBeNull();
            _categoryId = category!.Id;
        }

        private static CreateMobilePhoneExternalDto BuildMobilePhoneRequest(
            Guid categoryId,
            IReadOnlyDictionary<string, string> values)
        {
            return new CreateMobilePhoneExternalDto(
                new CommonDescriptionExtrernalDto(
                    GetValue(values, "Name"),
                    GetValue(values, "Brand"),
                    GetValue(values, "Description"),
                    GetValue(values, "MainPhoto"),
                    ParseList(values, "OtherPhotos")),
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
                GetValue(values, "Camera"),
                ParseBool(values, "FingerPrint"),
                ParseBool(values, "FaceId"),
                categoryId,
                new CreateMoneyExternalDto(ParseDecimal(values, "PriceAmount"), GetValue(values, "PriceCurrency")),
                GetValue(values, "Description2"),
                GetValue(values, "Description3"));
        }

        private static string GetValue(IReadOnlyDictionary<string, string> values, string key)
        {
            return values.TryGetValue(key, out var value)
                ? value
                : throw new InvalidOperationException($"Missing '{key}' value in mobile phone table.");
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

        private static List<string> ParseList(IReadOnlyDictionary<string, string> values, string key)
        {
            return GetValue(values, key)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();
        }
    }
}
