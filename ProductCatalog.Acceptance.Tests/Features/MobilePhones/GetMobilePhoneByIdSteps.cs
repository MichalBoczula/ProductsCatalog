using Microsoft.AspNetCore.Http;
using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Application.Common.Dtos.Common;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using ProductCatalog.Application.Features.Common;
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneById;
using Reqnroll;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Globalization;
using System.Collections.Generic;

namespace ProductCatalog.Acceptance.Tests.Features.MobilePhones
{
    [Binding]
    public class GetMobilePhoneByIdSteps
    {
        private MobilePhoneDetailsDto? _createdMobilePhone;
        private HttpResponseMessage? _response;
        private HttpResponseMessage? _responseFailure;
        private CreateMobilePhoneExternalDto? _request;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
        private Guid _mobilePhoneId;
        private Guid _categoryId;

        [Given("an existing mobile phone id")]
        public async Task GivenAnExistingMobilePhoneId(Table table)
        {
            var categoryCode = $"MOBILE-{Guid.NewGuid():N}";
            var categoryRequest = new CreateCategoryExternalDto(categoryCode, "Mobile category");
            var categoryResponse = await TestRunHooks.Client.PostAsJsonAsync("/categories", categoryRequest);
            categoryResponse.EnsureSuccessStatusCode();

            var category = await categoryResponse.Content.ReadFromJsonAsync<CategoryDto>(_jsonOptions);
            category.ShouldNotBeNull();

            _categoryId = category!.Id;

            var values = MergeDefaultValues(table);

            _request = new CreateMobilePhoneExternalDto(
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

            var response = await TestRunHooks.Client.PostAsJsonAsync("/mobile-phones", _request);
            response.EnsureSuccessStatusCode();

            _createdMobilePhone = await response.Content.ReadFromJsonAsync<MobilePhoneDetailsDto>(_jsonOptions);
            _createdMobilePhone.ShouldNotBeNull();

            _mobilePhoneId = _createdMobilePhone!.Id;
        }

        [When("I request the mobile phone by id")]
        public async Task WhenIRequestTheMobilePhoneById()
        {
            _response = await TestRunHooks.Client.GetAsync($"/mobile-phones/{_mobilePhoneId}");
        }

        [Then("the mobile phone details are returned successfully")]
        public async Task ThenTheMobilePhoneDetailsAreReturnedSuccessfully(Table table)
        {
            var expected = ParseExpectedTable(table);
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(ParseStatusCode(expected, "StatusCode"));

            var mobilePhone = await DeserializeResponse<MobilePhoneDetailsDto>(_response);
            mobilePhone.ShouldNotBeNull();

            mobilePhone.Id.ShouldBe(_mobilePhoneId);
            mobilePhone.CategoryId.ShouldBe(_categoryId);
            if (TryGetBool(expected, "IsActive", out var isActive))
            {
                mobilePhone.IsActive.ShouldBe(isActive);
            }

            mobilePhone.FaceId.ShouldBe(_request!.FaceId);
            mobilePhone.FingerPrint.ShouldBe(_request.FingerPrint);
            mobilePhone.Price.Amount.ShouldBe(ParseDecimal(expected, "PriceAmount", _request.Price.Amount));
            mobilePhone.Price.Currency.ShouldBe(GetExpectedValue(expected, "PriceCurrency", _request.Price.Currency));
            mobilePhone.CommonDescription.Name.ShouldBe(GetExpectedValue(expected, "Name", _request.CommonDescription.Name));
            mobilePhone.CommonDescription.Brand.ShouldBe(GetExpectedValue(expected, "Brand", _request.CommonDescription.Brand));
            mobilePhone.CommonDescription.Description.ShouldBe(_request.CommonDescription.Description);
            mobilePhone.CommonDescription.MainPhoto.ShouldBe(_request.CommonDescription.MainPhoto);
            mobilePhone.CommonDescription.OtherPhotos.ShouldBe(_request.CommonDescription.OtherPhotos);
            mobilePhone.ElectronicDetails.CPU.ShouldBe(_request.ElectronicDetails.CPU);
            mobilePhone.ElectronicDetails.GPU.ShouldBe(_request.ElectronicDetails.GPU);
            mobilePhone.ElectronicDetails.Ram.ShouldBe(_request.ElectronicDetails.Ram);
            mobilePhone.ElectronicDetails.Storage.ShouldBe(_request.ElectronicDetails.Storage);
            mobilePhone.ElectronicDetails.DisplayType.ShouldBe(_request.ElectronicDetails.DisplayType);
            mobilePhone.ElectronicDetails.RefreshRateHz.ShouldBe(_request.ElectronicDetails.RefreshRateHz);
            mobilePhone.ElectronicDetails.ScreenSizeInches.ShouldBe(_request.ElectronicDetails.ScreenSizeInches);
            mobilePhone.ElectronicDetails.Width.ShouldBe(_request.ElectronicDetails.Width);
            mobilePhone.ElectronicDetails.Height.ShouldBe(_request.ElectronicDetails.Height);
            mobilePhone.ElectronicDetails.BatteryType.ShouldBe(_request.ElectronicDetails.BatteryType);
            mobilePhone.ElectronicDetails.BatteryCapacity.ShouldBe(_request.ElectronicDetails.BatteryCapacity);
            mobilePhone.Connectivity.Has5G.ShouldBe(_request.Connectivity.Has5G);
            mobilePhone.Connectivity.WiFi.ShouldBe(_request.Connectivity.WiFi);
            mobilePhone.Connectivity.NFC.ShouldBe(_request.Connectivity.NFC);
            mobilePhone.Connectivity.Bluetooth.ShouldBe(_request.Connectivity.Bluetooth);
            mobilePhone.SatelliteNavigationSystems.GPS.ShouldBe(_request.SatelliteNavigationSystems.GPS);
            mobilePhone.SatelliteNavigationSystems.AGPS.ShouldBe(_request.SatelliteNavigationSystems.AGPS);
            mobilePhone.SatelliteNavigationSystems.Galileo.ShouldBe(_request.SatelliteNavigationSystems.Galileo);
            mobilePhone.SatelliteNavigationSystems.GLONASS.ShouldBe(_request.SatelliteNavigationSystems.GLONASS);
            mobilePhone.SatelliteNavigationSystems.QZSS.ShouldBe(_request.SatelliteNavigationSystems.QZSS);
            mobilePhone.Sensors.Accelerometer.ShouldBe(_request.Sensors.Accelerometer);
            mobilePhone.Sensors.Gyroscope.ShouldBe(_request.Sensors.Gyroscope);
            mobilePhone.Sensors.Proximity.ShouldBe(_request.Sensors.Proximity);
            mobilePhone.Sensors.Compass.ShouldBe(_request.Sensors.Compass);
            mobilePhone.Sensors.Barometer.ShouldBe(_request.Sensors.Barometer);
            mobilePhone.Sensors.Halla.ShouldBe(_request.Sensors.Halla);
            mobilePhone.Sensors.AmbientLight.ShouldBe(_request.Sensors.AmbientLight);
        }

        [Given("a mobile phone without specific id doesnt exists")]
        public void GivenAMobilePhoneWithoutSpecificIdDoesntExists()
        {
            _mobilePhoneId = Guid.NewGuid();
        }

        [When("I send request for mobile phone by not existed id")]
        public async Task WhenIRequestTheMobilePhoneByNotExistedId()
        {
            _responseFailure = await TestRunHooks.Client.GetAsync($"/mobile-phones/{_mobilePhoneId}");
        }

        [Then("response show not found error for mobile phone")]
        public async Task ThenResponseShowNotFoundErrorForMobilePhone(Table table)
        {
            var expected = ParseExpectedTable(table);
            _responseFailure.ShouldNotBeNull();
            _responseFailure!.StatusCode.ShouldBe(ParseStatusCode(expected, "StatusCode"));

            var problem = await DeserializeResponse<NotFoundProblemDetails>(_responseFailure);
            problem.ShouldNotBeNull();

            if (expected.TryGetValue("Title", out var title))
            {
                problem.Title.ShouldBe(title);
            }

            if (TryGetInt(expected, "Status", out var status))
            {
                problem.Status.ShouldBe(status);
            }

            if (expected.TryGetValue("Detail", out var detail))
            {
                problem.Detail.ShouldBe(ReplacePlaceholders(detail));
            }

            if (expected.TryGetValue("Instance", out var instance))
            {
                problem.Instance.ShouldBe(ReplacePlaceholders(instance));
            }

            if (expected.TryGetValue("TraceId", out var traceId))
            {
                problem.TraceId.ShouldBe(ReplacePlaceholders(traceId));
            }
            else
            {
                problem.TraceId.ShouldNotBeNullOrWhiteSpace();
            }
        }

        private async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }

        private static Dictionary<string, string> MergeDefaultValues(Table table)
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

            foreach (var row in table.Rows)
            {
                values[row["Field"]] = row["Value"];
            }

            return values;
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
            return (HttpStatusCode)ParseRequiredInt(expected, key);
        }

        private static int ParseRequiredInt(IReadOnlyDictionary<string, string> expected, string key)
        {
            return int.Parse(GetRequiredValue(expected, key), CultureInfo.InvariantCulture);
        }

        private static decimal ParseDecimal(IReadOnlyDictionary<string, string> expected, string key, decimal fallback)
        {
            if (!expected.TryGetValue(key, out var value))
            {
                return fallback;
            }

            return decimal.Parse(value, CultureInfo.InvariantCulture);
        }

        private static string GetRequiredValue(IReadOnlyDictionary<string, string> expected, string key)
        {
            if (!expected.TryGetValue(key, out var value))
            {
                throw new InvalidOperationException($"Missing '{key}' value in expected mobile phone response table.");
            }

            return value;
        }

        private static string GetExpectedValue(IReadOnlyDictionary<string, string> expected, string key, string fallback)
        {
            return expected.TryGetValue(key, out var value) ? value : fallback;
        }

        private static bool TryGetBool(IReadOnlyDictionary<string, string> expected, string key, out bool value)
        {
            value = default;
            if (!expected.TryGetValue(key, out var raw))
            {
                return false;
            }

            value = bool.Parse(raw);
            return true;
        }

        private static bool TryGetInt(IReadOnlyDictionary<string, string> expected, string key, out int value)
        {
            value = default;
            if (!expected.TryGetValue(key, out var raw))
            {
                return false;
            }

            value = int.Parse(raw, CultureInfo.InvariantCulture);
            return true;
        }

        private string ReplacePlaceholders(string value)
        {
            return value.Replace("{MobilePhoneId}", _mobilePhoneId.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        private static string GetValue(IReadOnlyDictionary<string, string> values, string key)
        {
            if (!values.TryGetValue(key, out var value))
            {
                throw new InvalidOperationException($"Missing '{key}' value in mobile phone contract table.");
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
    }
}
