using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Application.Common.Dtos.Common;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using ProductCatalog.Application.Features.Common;
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone;
using Reqnroll;
using Shouldly;

namespace ProductCatalog.Acceptance.Tests.Features.MobilePhones
{
    [Binding]
    public class UpdateMobilePhoneSteps
    {
        private CreateMobilePhoneExternalDto _createRequest = default!;
        private UpdateMobilePhoneExternalDto _updateRequest = default!;
        private MobilePhoneDetailsDto? _createdMobilePhone;
        private MobilePhoneDetailsDto? _updatedMobilePhone;
        private HttpResponseMessage? _response;
        private ApiProblemDetails? _apiProblem;
        private Guid _missingMobilePhoneId;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        [Given("an existing mobile phone which will be updated")]
        public async Task GivenAnExistingMobilePhoneWhichWillBeUpdated(Table table)
        {
            var categoryId = await CreateCategoryAsync("MOBILE-BASE");
            _createRequest = BuildCreateMobilePhoneRequest(categoryId);

            var response = await TestRunHooks.Client.PostAsJsonAsync("/mobile-phones", _createRequest, _jsonOptions);
            response.EnsureSuccessStatusCode();

            _createdMobilePhone = await response.Content.ReadFromJsonAsync<MobilePhoneDetailsDto>(_jsonOptions);
            _createdMobilePhone.ShouldNotBeNull();

            var updatedCategoryId = await CreateCategoryAsync("MOBILE-UPD");
            _updateRequest = BuildUpdateMobilePhoneRequest(updatedCategoryId, table);
        }

        [When("I submit the update mobile phone request")]
        public async Task WhenISubmitTheUpdateMobilePhoneRequest()
        {
            _createdMobilePhone.ShouldNotBeNull();

            _response = await TestRunHooks.Client.PutAsJsonAsync(
                $"/mobile-phones/{_createdMobilePhone!.Id}",
                _updateRequest,
                _jsonOptions);

            _updatedMobilePhone = await DeserializeResponse<MobilePhoneDetailsDto>(_response);
        }

        [Then("the mobile phone is updated successfully")]
        public void ThenTheMobilePhoneIsUpdatedSuccessfully(Table table)
        {
            var expected = ParseExpectedTable(table);
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(ParseStatusCode(expected, "StatusCode"));

            _updatedMobilePhone.ShouldNotBeNull();
            _updatedMobilePhone!.Id.ShouldBe(_createdMobilePhone!.Id);
            _updatedMobilePhone.CategoryId.ShouldBe(_updateRequest.CategoryId);
            _updatedMobilePhone.FaceId.ShouldBe(_updateRequest.FaceId);
            _updatedMobilePhone.FingerPrint.ShouldBe(_updateRequest.FingerPrint);
            _updatedMobilePhone.Price.Amount.ShouldBe(ParseDecimal(expected, "PriceAmount", _updateRequest.Price.Amount));
            _updatedMobilePhone.Price.Currency.ShouldBe(GetExpectedValue(expected, "PriceCurrency", _updateRequest.Price.Currency));
            _updatedMobilePhone.CommonDescription.Name.ShouldBe(GetExpectedValue(expected, "Name", _updateRequest.CommonDescription.Name));
            _updatedMobilePhone.CommonDescription.Brand.ShouldBe(GetExpectedValue(expected, "Brand", _updateRequest.CommonDescription.Brand));
            _updatedMobilePhone.CommonDescription.Description.ShouldBe(_updateRequest.CommonDescription.Description);
            _updatedMobilePhone.CommonDescription.MainPhoto.ShouldBe(_updateRequest.CommonDescription.MainPhoto);
            _updatedMobilePhone.CommonDescription.OtherPhotos.ShouldBe(_updateRequest.CommonDescription.OtherPhotos);
            _updatedMobilePhone.ElectronicDetails.CPU.ShouldBe(_updateRequest.ElectronicDetails.CPU);
            _updatedMobilePhone.ElectronicDetails.GPU.ShouldBe(_updateRequest.ElectronicDetails.GPU);
            _updatedMobilePhone.ElectronicDetails.Ram.ShouldBe(_updateRequest.ElectronicDetails.Ram);
            _updatedMobilePhone.ElectronicDetails.Storage.ShouldBe(_updateRequest.ElectronicDetails.Storage);
            _updatedMobilePhone.ElectronicDetails.DisplayType.ShouldBe(_updateRequest.ElectronicDetails.DisplayType);
            _updatedMobilePhone.ElectronicDetails.RefreshRateHz.ShouldBe(_updateRequest.ElectronicDetails.RefreshRateHz);
            _updatedMobilePhone.ElectronicDetails.ScreenSizeInches.ShouldBe(_updateRequest.ElectronicDetails.ScreenSizeInches);
            _updatedMobilePhone.ElectronicDetails.Width.ShouldBe(_updateRequest.ElectronicDetails.Width);
            _updatedMobilePhone.ElectronicDetails.Height.ShouldBe(_updateRequest.ElectronicDetails.Height);
            _updatedMobilePhone.ElectronicDetails.BatteryType.ShouldBe(_updateRequest.ElectronicDetails.BatteryType);
            _updatedMobilePhone.ElectronicDetails.BatteryCapacity.ShouldBe(_updateRequest.ElectronicDetails.BatteryCapacity);
            _updatedMobilePhone.Connectivity.Has5G.ShouldBe(_updateRequest.Connectivity.Has5G);
            _updatedMobilePhone.Connectivity.WiFi.ShouldBe(_updateRequest.Connectivity.WiFi);
            _updatedMobilePhone.Connectivity.NFC.ShouldBe(_updateRequest.Connectivity.NFC);
            _updatedMobilePhone.Connectivity.Bluetooth.ShouldBe(_updateRequest.Connectivity.Bluetooth);
            _updatedMobilePhone.SatelliteNavigationSystems.GPS.ShouldBe(_updateRequest.SatelliteNavigationSystems.GPS);
            _updatedMobilePhone.SatelliteNavigationSystems.AGPS.ShouldBe(_updateRequest.SatelliteNavigationSystems.AGPS);
            _updatedMobilePhone.SatelliteNavigationSystems.Galileo.ShouldBe(_updateRequest.SatelliteNavigationSystems.Galileo);
            _updatedMobilePhone.SatelliteNavigationSystems.GLONASS.ShouldBe(_updateRequest.SatelliteNavigationSystems.GLONASS);
            _updatedMobilePhone.SatelliteNavigationSystems.QZSS.ShouldBe(_updateRequest.SatelliteNavigationSystems.QZSS);
            _updatedMobilePhone.Sensors.Accelerometer.ShouldBe(_updateRequest.Sensors.Accelerometer);
            _updatedMobilePhone.Sensors.Gyroscope.ShouldBe(_updateRequest.Sensors.Gyroscope);
            _updatedMobilePhone.Sensors.Proximity.ShouldBe(_updateRequest.Sensors.Proximity);
            _updatedMobilePhone.Sensors.Compass.ShouldBe(_updateRequest.Sensors.Compass);
            _updatedMobilePhone.Sensors.Barometer.ShouldBe(_updateRequest.Sensors.Barometer);
            _updatedMobilePhone.Sensors.Halla.ShouldBe(_updateRequest.Sensors.Halla);
            _updatedMobilePhone.Sensors.AmbientLight.ShouldBe(_updateRequest.Sensors.AmbientLight);
        }

        [Given("mobile phone identify by id not exists")]
        public async Task GivenMobilePhoneIdentifyByIdNotExists(Table table)
        {
            _missingMobilePhoneId = Guid.NewGuid();
            var categoryId = await CreateCategoryAsync("MOBILE-MISSING");
            _updateRequest = BuildUpdateMobilePhoneRequest(categoryId, table);
        }

        [When("I submit the update mobile phone request for missing mobile phone")]
        public async Task WhenISubmitTheUpdateMobilePhoneRequestForMissingMobilePhone()
        {
            _response = await TestRunHooks.Client.PutAsJsonAsync(
                $"/mobile-phones/{_missingMobilePhoneId}",
                _updateRequest,
                _jsonOptions);

            var json = await _response.Content.ReadAsStringAsync();
            _apiProblem = JsonSerializer.Deserialize<ApiProblemDetails>(json, _jsonOptions);
        }

        [Then("the mobile phone update fails with validation errors")]
        public void ThenTheMobilePhoneUpdateFailsWithValidationErrors(Table table)
        {
            var expected = ParseExpectedTable(table);
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(ParseStatusCode(expected, "StatusCode"));

            _apiProblem.ShouldNotBeNull();
            _apiProblem!.Status.ShouldBe(ParseRequiredInt(expected, "Status"));
            _apiProblem.Title.ShouldBe(GetRequiredValue(expected, "Title"));
            _apiProblem.Detail.ShouldBe(GetRequiredValue(expected, "Detail"));
            _apiProblem.Errors.Count().ShouldBeGreaterThan(0);
            _apiProblem.Errors.ShouldContain(error =>
                error.Message == GetRequiredValue(expected, "ErrorMessage")
                && error.Entity == GetRequiredValue(expected, "ErrorEntity")
                && error.Name == GetRequiredValue(expected, "ErrorName"));
        }

        private static CreateMobilePhoneExternalDto BuildCreateMobilePhoneRequest(Guid categoryId)
        {
            return new CreateMobilePhoneExternalDto(
                new CommonDescriptionExtrernalDto(
                    "Baseline Mobile Phone",
                    "Brand",
                    "Phone created for update test",
                    "base-main.jpg",
                    new List<string> { "base-photo-1.jpg", "base-photo-2.jpg" }),
                new CreateElectronicDetailsExternalDto(
                    "Octa-core",
                    "Adreno",
                    "8GB",
                    "256GB",
                    "OLED",
                    120,
                    6.4m,
                    72,
                    152,
                    "Li-Ion",
                    4500),
                new CreateConnectivityExternalDto(true, true, true, true),
                new CreateSatelliteNavigationSystemExternalDto(true, true, true, true, true),
                new CreateSensorsExternalDto(true, true, true, true, true, false, true),
                "camera",
                true,
                true,
                categoryId,
                new CreateMoneyExternalDto(799.99m, "USD"),
                "desc2",
                "desc3");
        }

        private static UpdateMobilePhoneExternalDto BuildUpdateMobilePhoneRequest(Guid categoryId, Table? table)
        {
            var values = MergeDefaultValues(table);
            return new UpdateMobilePhoneExternalDto(
                new CommonDescriptionExtrernalDto(
                    GetValue(values, "Name"),
                    GetValue(values, "Brand"),
                    GetValue(values, "Description"),
                    GetValue(values, "MainPhoto"),
                    ParseList(values, "OtherPhotos")),
                new UpdateElectronicDetailsExternalDto(
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
                new UpdateConnectivityExternalDto(
                    ParseBool(values, "Has5G"),
                    ParseBool(values, "WiFi"),
                    ParseBool(values, "NFC"),
                    ParseBool(values, "Bluetooth")),
                new UpdateSatelliteNavigationSystemExternalDto(
                    ParseBool(values, "GPS"),
                    ParseBool(values, "AGPS"),
                    ParseBool(values, "Galileo"),
                    ParseBool(values, "GLONASS"),
                    ParseBool(values, "QZSS")),
                new UpdateSensorsExternalDto(
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
                categoryId,
                new UpdateMoneyExternalDto(
                    ParseDecimal(values, "PriceAmount"),
                    GetValue(values, "PriceCurrency")),
                GetValue(values, "Description2"),
                GetValue(values, "Description3"));
        }

        private static Dictionary<string, string> MergeDefaultValues(Table? table)
        {
            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Name"] = "Updated Mobile Phone",
                ["Brand"] = "Brand",
                ["Description"] = "Updated by acceptance test",
                ["MainPhoto"] = "updated-main.jpg",
                ["OtherPhotos"] = "updated-photo-1.jpg, updated-photo-2.jpg",
                ["CPU"] = "Deca-core",
                ["GPU"] = "Mali",
                ["Ram"] = "12GB",
                ["Storage"] = "512GB",
                ["DisplayType"] = "AMOLED",
                ["RefreshRateHz"] = "144",
                ["ScreenSizeInches"] = "6.8",
                ["Width"] = "74",
                ["Height"] = "160",
                ["BatteryType"] = "Li-Poly",
                ["BatteryCapacity"] = "5200",
                ["Has5G"] = "false",
                ["WiFi"] = "true",
                ["NFC"] = "false",
                ["Bluetooth"] = "true",
                ["GPS"] = "true",
                ["AGPS"] = "false",
                ["Galileo"] = "true",
                ["GLONASS"] = "true",
                ["QZSS"] = "false",
                ["Accelerometer"] = "true",
                ["Gyroscope"] = "false",
                ["Proximity"] = "true",
                ["Compass"] = "true",
                ["Barometer"] = "false",
                ["Halla"] = "true",
                ["AmbientLight"] = "true",
                ["Camera"] = "camera",
                ["FingerPrint"] = "false",
                ["FaceId"] = "true",
                ["PriceAmount"] = "899.99",
                ["PriceCurrency"] = "EUR",
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
            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var row in table.Rows)
            {
                values[row["Field"]] = row["Value"];
            }

            return values;
        }

        private static string GetRequiredValue(IReadOnlyDictionary<string, string> values, string key)
        {
            if (!values.TryGetValue(key, out var value))
            {
                throw new InvalidOperationException($"Missing '{key}' value in mobile phone expected result table.");
            }

            return value;
        }

        private static string GetExpectedValue(IReadOnlyDictionary<string, string> values, string key, string fallback)
        {
            return values.TryGetValue(key, out var value) ? value : fallback;
        }

        private static HttpStatusCode ParseStatusCode(IReadOnlyDictionary<string, string> values, string key)
        {
            var value = GetRequiredValue(values, key);
            return (HttpStatusCode)int.Parse(value, CultureInfo.InvariantCulture);
        }

        private static int ParseRequiredInt(IReadOnlyDictionary<string, string> values, string key)
        {
            var value = GetRequiredValue(values, key);
            return int.Parse(value, CultureInfo.InvariantCulture);
        }

        private static decimal ParseDecimal(IReadOnlyDictionary<string, string> values, string key, decimal fallback)
        {
            if (!values.TryGetValue(key, out var value))
            {
                return fallback;
            }

            return decimal.Parse(value, CultureInfo.InvariantCulture);
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

        private async Task<Guid> CreateCategoryAsync(string prefix)
        {
            var categoryCode = $"{prefix}-{Guid.NewGuid():N}";
            var categoryRequest = new CreateCategoryExternalDto(categoryCode, "Mobile category");
            var categoryResponse = await TestRunHooks.Client.PostAsJsonAsync("/categories", categoryRequest, _jsonOptions);
            categoryResponse.EnsureSuccessStatusCode();

            var category = await categoryResponse.Content.ReadFromJsonAsync<CategoryDto>(_jsonOptions);
            category.ShouldNotBeNull();

            return category!.Id;
        }

        private async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }
    }
}
