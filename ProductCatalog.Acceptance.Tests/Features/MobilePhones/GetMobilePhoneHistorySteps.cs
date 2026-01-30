using Microsoft.AspNetCore.Http;
using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Application.Common.Dtos.Common;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using ProductCatalog.Application.Features.Common;
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneHistory;
using ProductCatalog.Domain.Common.Enums;
using Reqnroll;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.MobilePhones
{
    [Binding]
    public class GetMobilePhoneHistorySteps
    {
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
        private const int PageNumber = 1;
        private const int PageSize = 10;

        [Given("an existing mobile phone with history")]
        public async Task GivenAnExistingMobilePhoneWithHistory(Table table)
        {
            await CreateMobilePhoneAsync(table);
        }

        [When("I request the mobile phone history")]
        public async Task WhenIRequestTheMobilePhoneHistory()
        {
            _response = await TestRunHooks.Client.GetAsync($"/mobile-phones/{_mobilePhoneId}/history?pageNumber={PageNumber}&pageSize={PageSize}");
        }

        [Then("the mobile phone history is returned successfully")]
        public async Task ThenTheMobilePhoneHistoryIsReturnedSuccessfully(Table table)
        {
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(HttpStatusCode.OK);

            var historyEntries = await DeserializeResponse<List<MobilePhoneHistoryDto>>(_response);
            historyEntries.ShouldNotBeNull();
            historyEntries!.Count.ShouldBeGreaterThan(0);

            var historyEntry = historyEntries[0];
            historyEntry.Id.ShouldNotBe(Guid.Empty);
            historyEntry.MobilePhoneId.ShouldBe(_mobilePhoneId);
            historyEntry.CategoryId.ShouldBe(_categoryId);
            historyEntry.IsActive.ShouldBeTrue();
            historyEntry.FingerPrint.ShouldBe(_request!.FingerPrint);
            historyEntry.FaceId.ShouldBe(_request.FaceId);
            historyEntry.Price.Amount.ShouldBe(_request.Price.Amount);
            historyEntry.Price.Currency.ShouldBe(_request.Price.Currency);
            historyEntry.CommonDescription.Name.ShouldBe(_request.CommonDescription.Name);
            historyEntry.CommonDescription.Description.ShouldBe(_request.CommonDescription.Description);
            historyEntry.CommonDescription.MainPhoto.ShouldBe(_request.CommonDescription.MainPhoto);
            historyEntry.CommonDescription.OtherPhotos.ShouldBe(_request.CommonDescription.OtherPhotos);
            historyEntry.ElectronicDetails.CPU.ShouldBe(_request.ElectronicDetails.CPU);
            historyEntry.ElectronicDetails.GPU.ShouldBe(_request.ElectronicDetails.GPU);
            historyEntry.ElectronicDetails.Ram.ShouldBe(_request.ElectronicDetails.Ram);
            historyEntry.ElectronicDetails.Storage.ShouldBe(_request.ElectronicDetails.Storage);
            historyEntry.ElectronicDetails.DisplayType.ShouldBe(_request.ElectronicDetails.DisplayType);
            historyEntry.ElectronicDetails.RefreshRateHz.ShouldBe(_request.ElectronicDetails.RefreshRateHz);
            historyEntry.ElectronicDetails.ScreenSizeInches.ShouldBe(_request.ElectronicDetails.ScreenSizeInches);
            historyEntry.ElectronicDetails.Width.ShouldBe(_request.ElectronicDetails.Width);
            historyEntry.ElectronicDetails.Height.ShouldBe(_request.ElectronicDetails.Height);
            historyEntry.ElectronicDetails.BatteryType.ShouldBe(_request.ElectronicDetails.BatteryType);
            historyEntry.ElectronicDetails.BatteryCapacity.ShouldBe(_request.ElectronicDetails.BatteryCapacity);
            historyEntry.Connectivity.Has5G.ShouldBe(_request.Connectivity.Has5G);
            historyEntry.Connectivity.WiFi.ShouldBe(_request.Connectivity.WiFi);
            historyEntry.Connectivity.NFC.ShouldBe(_request.Connectivity.NFC);
            historyEntry.Connectivity.Bluetooth.ShouldBe(_request.Connectivity.Bluetooth);
            historyEntry.SatelliteNavigationSystems.GPS.ShouldBe(_request.SatelliteNavigationSystems.GPS);
            historyEntry.SatelliteNavigationSystems.AGPS.ShouldBe(_request.SatelliteNavigationSystems.AGPS);
            historyEntry.SatelliteNavigationSystems.Galileo.ShouldBe(_request.SatelliteNavigationSystems.Galileo);
            historyEntry.SatelliteNavigationSystems.GLONASS.ShouldBe(_request.SatelliteNavigationSystems.GLONASS);
            historyEntry.SatelliteNavigationSystems.QZSS.ShouldBe(_request.SatelliteNavigationSystems.QZSS);
            historyEntry.Sensors.Accelerometer.ShouldBe(_request.Sensors.Accelerometer);
            historyEntry.Sensors.Gyroscope.ShouldBe(_request.Sensors.Gyroscope);
            historyEntry.Sensors.Proximity.ShouldBe(_request.Sensors.Proximity);
            historyEntry.Sensors.Compass.ShouldBe(_request.Sensors.Compass);
            historyEntry.Sensors.Barometer.ShouldBe(_request.Sensors.Barometer);
            historyEntry.Sensors.Halla.ShouldBe(_request.Sensors.Halla);
            historyEntry.Sensors.AmbientLight.ShouldBe(_request.Sensors.AmbientLight);
            historyEntry.Operation.ShouldBe(Operation.Inserted);
            historyEntry.ChangedAt.ShouldNotBe(default(DateTime));

            var expectations = ToExpectationMap(table);
            if (expectations.TryGetValue("Operation", out var expectedOperation))
            {
                historyEntry.Operation.ShouldBe(Enum.Parse<Operation>(expectedOperation, true));
            }

            if (expectations.TryGetValue("IsActive", out var expectedIsActive))
            {
                historyEntry.IsActive.ShouldBe(bool.Parse(expectedIsActive));
            }

            if (expectations.TryGetValue("ChangedAt", out var expectedChangedAt))
            {
                if (string.Equals(expectedChangedAt, "set", StringComparison.OrdinalIgnoreCase))
                {
                    historyEntry.ChangedAt.ShouldNotBe(default(DateTime));
                }
                else if (string.Equals(expectedChangedAt, "unset", StringComparison.OrdinalIgnoreCase))
                {
                    historyEntry.ChangedAt.ShouldBe(default(DateTime));
                }
            }
        }

        [Given("a missing mobile phone id")]
        public void GivenAMissingMobilePhoneId()
        {
            _mobilePhoneId = Guid.NewGuid();
        }

        [When("I request the mobile phone history for the missing id")]
        public async Task WhenIRequestTheMobilePhoneHistoryForTheMissingId()
        {
            _responseFailure = await TestRunHooks.Client.GetAsync($"/mobile-phones/{_mobilePhoneId}/history?pageNumber={PageNumber}&pageSize={PageSize}");
        }

        [Then("response show not found error for mobile phone history")]
        public async Task ThenResponseShowNotFoundErrorForMobilePhoneHistory()
        {
            _responseFailure.ShouldNotBeNull();
            _responseFailure!.StatusCode.ShouldBe(HttpStatusCode.NotFound);

            var problem = await DeserializeResponse<NotFoundProblemDetails>(_responseFailure);
            problem.ShouldNotBeNull();

            problem.Title.ShouldBe("Resource not found.");
            problem.Status.ShouldBe(StatusCodes.Status404NotFound);
            problem.Detail.ShouldBe($"Resource {nameof(List<MobilePhoneHistoryDto>)} identify by id {_mobilePhoneId} cannot be found in databese during action {nameof(GetMobilePhoneHistoryQuery)}.");
            problem.Instance.ShouldBe($"/mobile-phones/{_mobilePhoneId}/history");
            problem.TraceId.ShouldNotBeNullOrWhiteSpace();
        }

        private async Task CreateMobilePhoneAsync(Table? table)
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

            var createdMobilePhone = await response.Content.ReadFromJsonAsync<MobilePhoneDetailsDto>(_jsonOptions);
            createdMobilePhone.ShouldNotBeNull();

            _mobilePhoneId = createdMobilePhone!.Id;
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

        private static Dictionary<string, string> ToExpectationMap(Table table)
        {
            var expectations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var row in table.Rows)
            {
                expectations[row["Field"]] = row["Expected"];
            }

            return expectations;
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

        private async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }
    }
}
