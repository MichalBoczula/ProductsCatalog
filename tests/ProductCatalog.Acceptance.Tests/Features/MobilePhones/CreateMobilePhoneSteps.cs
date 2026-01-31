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
    public class CreateMobilePhoneSteps
    {
        private CreateMobilePhoneExternalDto _validRequest = default!;
        private CreateMobilePhoneExternalDto _invalidRequest = default!;
        private HttpResponseMessage? _response;
        private ApiProblemDetails? _apiProblem;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        [Given("I have valid mobile phone details")]
        public async Task GivenIHaveValidMobilePhoneDetails(Table table)
        {
            var categoryId = await CreateCategoryAsync();
            _validRequest = BuildMobilePhoneRequest(categoryId, table);

            AllureJson.AttachObject(
                "Request JSON (valid)",
                _validRequest,
                _jsonOptions
            );
        }

        [When("I submit the create mobile phone request")]
        public async Task WhenISubmitTheCreateMobilePhoneRequest()
        {
            _response = await TestRunHooks.Client.PostAsJsonAsync("/mobile-phones", _validRequest, _jsonOptions);

            var body = await _response.Content.ReadAsStringAsync();

            AllureJson.AttachRawJson($"Response JSON ({(int)_response.StatusCode})", body);
        }

        [Then("the mobile phone is created successfully")]
        public async Task ThenTheMobilePhoneIsCreatedSuccessfully(Table table)
        {
            var expected = ParseExpectedTable(table);
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(ParseStatusCode(expected, "StatusCode"));

            var mobilePhone = await DeserializeResponse<MobilePhoneDetailsDto>(_response);
            mobilePhone.ShouldNotBeNull();

            if (TryGetBool(expected, "HasId", out var hasId))
            {
                if (hasId)
                {
                    mobilePhone.Id.ShouldNotBe(Guid.Empty);
                }
                else
                {
                    mobilePhone.Id.ShouldBe(Guid.Empty);
                }
            }

            if (TryGetBool(expected, "IsActive", out var isActive))
            {
                mobilePhone.IsActive.ShouldBe(isActive);
            }

            mobilePhone.CategoryId.ShouldBe(_validRequest.CategoryId);
            mobilePhone.FingerPrint.ShouldBe(_validRequest.FingerPrint);
            mobilePhone.FaceId.ShouldBe(_validRequest.FaceId);
            mobilePhone.Price.Amount.ShouldBe(ParseDecimal(expected, "PriceAmount", _validRequest.Price.Amount));
            mobilePhone.Price.Currency.ShouldBe(GetExpectedValue(expected, "PriceCurrency", _validRequest.Price.Currency));
            mobilePhone.CommonDescription.Name.ShouldBe(GetExpectedValue(expected, "Name", _validRequest.CommonDescription.Name));
            mobilePhone.CommonDescription.Brand.ShouldBe(GetExpectedValue(expected, "Brand", _validRequest.CommonDescription.Brand));
            mobilePhone.CommonDescription.Description.ShouldBe(_validRequest.CommonDescription.Description);
            mobilePhone.CommonDescription.MainPhoto.ShouldBe(_validRequest.CommonDescription.MainPhoto);
            mobilePhone.CommonDescription.OtherPhotos.ShouldBe(_validRequest.CommonDescription.OtherPhotos);
            mobilePhone.ElectronicDetails.CPU.ShouldBe(_validRequest.ElectronicDetails.CPU);
            mobilePhone.ElectronicDetails.GPU.ShouldBe(_validRequest.ElectronicDetails.GPU);
            mobilePhone.ElectronicDetails.Ram.ShouldBe(_validRequest.ElectronicDetails.Ram);
            mobilePhone.ElectronicDetails.Storage.ShouldBe(_validRequest.ElectronicDetails.Storage);
            mobilePhone.ElectronicDetails.DisplayType.ShouldBe(_validRequest.ElectronicDetails.DisplayType);
            mobilePhone.ElectronicDetails.RefreshRateHz.ShouldBe(_validRequest.ElectronicDetails.RefreshRateHz);
            mobilePhone.ElectronicDetails.ScreenSizeInches.ShouldBe(_validRequest.ElectronicDetails.ScreenSizeInches);
            mobilePhone.ElectronicDetails.Width.ShouldBe(_validRequest.ElectronicDetails.Width);
            mobilePhone.ElectronicDetails.Height.ShouldBe(_validRequest.ElectronicDetails.Height);
            mobilePhone.ElectronicDetails.BatteryType.ShouldBe(_validRequest.ElectronicDetails.BatteryType);
            mobilePhone.ElectronicDetails.BatteryCapacity.ShouldBe(_validRequest.ElectronicDetails.BatteryCapacity);
            mobilePhone.Connectivity.Has5G.ShouldBe(_validRequest.Connectivity.Has5G);
            mobilePhone.Connectivity.WiFi.ShouldBe(_validRequest.Connectivity.WiFi);
            mobilePhone.Connectivity.NFC.ShouldBe(_validRequest.Connectivity.NFC);
            mobilePhone.Connectivity.Bluetooth.ShouldBe(_validRequest.Connectivity.Bluetooth);
            mobilePhone.SatelliteNavigationSystems.GPS.ShouldBe(_validRequest.SatelliteNavigationSystems.GPS);
            mobilePhone.SatelliteNavigationSystems.AGPS.ShouldBe(_validRequest.SatelliteNavigationSystems.AGPS);
            mobilePhone.SatelliteNavigationSystems.Galileo.ShouldBe(_validRequest.SatelliteNavigationSystems.Galileo);
            mobilePhone.SatelliteNavigationSystems.GLONASS.ShouldBe(_validRequest.SatelliteNavigationSystems.GLONASS);
            mobilePhone.SatelliteNavigationSystems.QZSS.ShouldBe(_validRequest.SatelliteNavigationSystems.QZSS);
            mobilePhone.Sensors.Accelerometer.ShouldBe(_validRequest.Sensors.Accelerometer);
            mobilePhone.Sensors.Gyroscope.ShouldBe(_validRequest.Sensors.Gyroscope);
            mobilePhone.Sensors.Proximity.ShouldBe(_validRequest.Sensors.Proximity);
            mobilePhone.Sensors.Compass.ShouldBe(_validRequest.Sensors.Compass);
            mobilePhone.Sensors.Barometer.ShouldBe(_validRequest.Sensors.Barometer);
            mobilePhone.Sensors.Halla.ShouldBe(_validRequest.Sensors.Halla);
            mobilePhone.Sensors.AmbientLight.ShouldBe(_validRequest.Sensors.AmbientLight);
            mobilePhone.Camera.ShouldBe(_validRequest.Camera);
            mobilePhone.Description2.ShouldBe(_validRequest.Description2);
            mobilePhone.Description3.ShouldBe(_validRequest.Description3);
        }

        [Given("I have invalid mobile phone details")]
        public void GivenIHaveInvalidMobilePhoneDetails(Table table)
        {
            _invalidRequest = BuildMobilePhoneRequest(Guid.NewGuid(), table);

            AllureJson.AttachObject(
              "Request JSON (invalid)",
              _invalidRequest,
              _jsonOptions
          );
        }

        [When("I submit the create invalid mobile phone request")]
        public async Task WhenISubmitTheCreateInvalidMobilePhoneRequest()
        {
            _response = await TestRunHooks.Client.PostAsJsonAsync("/mobile-phones", _invalidRequest, _jsonOptions);

            var json = await _response.Content.ReadAsStringAsync();
            
            AllureJson.AttachRawJson($"Response JSON ({(int)_response.StatusCode})", json);

            _apiProblem = JsonSerializer.Deserialize<ApiProblemDetails>(json, _jsonOptions);
        }

        [Then("the mobile phone creation fails with validation errors")]
        public void ThenTheMobilePhoneCreationFailsWithValidationErrors(Table table)
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

        private static CreateMobilePhoneExternalDto BuildMobilePhoneRequest(Guid categoryId, Table? table)
        {
            var values = MergeDefaultValues(table);
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
                categoryId,
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
                ["Name"] = "Test Mobile Phone",
                ["Brand"] = "Brand",
                ["Description"] = "Phone created by acceptance test",
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

        private static bool TryGetBool(IReadOnlyDictionary<string, string> values, string key, out bool result)
        {
            if (!values.TryGetValue(key, out var value))
            {
                result = false;
                return false;
            }

            result = bool.Parse(value);
            return true;
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

        private async Task<Guid> CreateCategoryAsync()
        {
            var categoryCode = $"MOBILE-{Guid.NewGuid():N}";
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
