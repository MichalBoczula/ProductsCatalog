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
using Reqnroll;
using Shouldly;

namespace ProductCatalog.Acceptance.Tests.Features.MobilePhones
{
    [Binding]
    public class DeleteMobilePhoneSteps
    {
        private CreateMobilePhoneExternalDto _createRequest = default!;
        private MobilePhoneDetailsDto? _createdMobilePhone;
        private MobilePhoneDetailsDto? _deletedMobilePhone;
        private HttpResponseMessage? _response;
        private ApiProblemDetails? _apiProblem;
        private Guid _missingMobilePhoneId;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        [Given("an existing mobile phone to delete")]
        public async Task GivenAnExistingMobilePhoneToDelete(Table table)
        {
            var categoryId = await CreateCategoryAsync("MOBILE-DEL");
            var values = MergeDefaultValues(table);
            _createRequest = BuildCreateMobilePhoneRequest(categoryId, values);

            AllureJson.AttachObject(
                "Request JSON (create for delete)",
                _createRequest,
                _jsonOptions
            );

            var response = await TestRunHooks.Client.PostAsJsonAsync("/mobile-phones", _createRequest, _jsonOptions);
            response.EnsureSuccessStatusCode();

            _createdMobilePhone = await response.Content.ReadFromJsonAsync<MobilePhoneDetailsDto>(_jsonOptions);
            _createdMobilePhone.ShouldNotBeNull();
        }

        [When("I submit the delete mobile phone request")]
        public async Task WhenISubmitTheDeleteMobilePhoneRequest()
        {
            _createdMobilePhone.ShouldNotBeNull();

            var deleteRequest = new
            {
                MobilePhoneId = _createdMobilePhone!.Id
            };

            AllureJson.AttachObject(
                "Request JSON (delete)",
                deleteRequest,
                _jsonOptions
            );

            _response = await TestRunHooks.Client.DeleteAsync($"/mobile-phones/{_createdMobilePhone!.Id}");
            var body = await _response.Content.ReadAsStringAsync();

            AllureJson.AttachRawJson($"Response JSON ({(int)_response.StatusCode})", body);

            _deletedMobilePhone = JsonSerializer.Deserialize<MobilePhoneDetailsDto>(body, _jsonOptions);
        }

        [Then("the mobile phone is deleted successfully")]
        public void ThenTheMobilePhoneIsDeletedSuccessfully(Table table)
        {
            var expected = ParseExpectedTable(table);
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(ParseStatusCode(expected, "StatusCode"));

            _deletedMobilePhone.ShouldNotBeNull();
            _deletedMobilePhone!.Id.ShouldBe(_createdMobilePhone!.Id);
            if (TryGetBool(expected, "HasId", out var hasId))
            {
                if (hasId)
                {
                    _deletedMobilePhone.Id.ShouldNotBe(Guid.Empty);
                }
                else
                {
                    _deletedMobilePhone.Id.ShouldBe(Guid.Empty);
                }
            }

            if (TryGetBool(expected, "IsActive", out var isActive))
            {
                _deletedMobilePhone.IsActive.ShouldBe(isActive);
            }
            _deletedMobilePhone.CategoryId.ShouldBe(_createRequest.CategoryId);
            _deletedMobilePhone.FingerPrint.ShouldBe(_createRequest.FingerPrint);
            _deletedMobilePhone.FaceId.ShouldBe(_createRequest.FaceId);
            _deletedMobilePhone.Price.Amount.ShouldBe(ParseDecimal(expected, "PriceAmount", _createRequest.Price.Amount));
            _deletedMobilePhone.Price.Currency.ShouldBe(GetExpectedValue(expected, "PriceCurrency", _createRequest.Price.Currency));
            _deletedMobilePhone.CommonDescription.Name.ShouldBe(GetExpectedValue(expected, "Name", _createRequest.CommonDescription.Name));
            _deletedMobilePhone.CommonDescription.Brand.ShouldBe(GetExpectedValue(expected, "Brand", _createRequest.CommonDescription.Brand));
            _deletedMobilePhone.CommonDescription.Description.ShouldBe(_createRequest.CommonDescription.Description);
            _deletedMobilePhone.CommonDescription.MainPhoto.ShouldBe(_createRequest.CommonDescription.MainPhoto);
            _deletedMobilePhone.CommonDescription.OtherPhotos.ShouldBe(_createRequest.CommonDescription.OtherPhotos);
        }

        [Given("a mobile phone id that does not exist")]
        public void GivenAMobilePhoneIdThatDoesNotExist()
        {
            _missingMobilePhoneId = Guid.NewGuid();
        }

        [When("I submit the delete mobile phone request for missing mobile phone")]
        public async Task WhenISubmitTheDeleteMobilePhoneRequestForMissingMobilePhone()
        {
            var deleteRequest = new
            {
                MobilePhoneId = _missingMobilePhoneId
            };

            AllureJson.AttachObject(
                "Request JSON (delete missing)",
                deleteRequest,
                _jsonOptions
            );

            _response = await TestRunHooks.Client.DeleteAsync($"/mobile-phones/{_missingMobilePhoneId}");
            var json = await _response.Content.ReadAsStringAsync();

            AllureJson.AttachRawJson($"Response JSON ({(int)_response.StatusCode})", json);

            _apiProblem = JsonSerializer.Deserialize<ApiProblemDetails>(json, _jsonOptions);
        }

        [Then("the mobile phone deletion fails with validation errors")]
        public void ThenTheMobilePhoneDeletionFailsWithValidationErrors(Table table)
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

        private static CreateMobilePhoneExternalDto BuildCreateMobilePhoneRequest(
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

        private static Dictionary<string, string> MergeDefaultValues(Table? table)
        {
            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Name"] = "Delete Mobile Phone",
                ["Brand"] = "Brand",
                ["Description"] = "Phone created for delete test",
                ["MainPhoto"] = "delete-main.jpg",
                ["OtherPhotos"] = "delete-photo-1.jpg, delete-photo-2.jpg",
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
                ["PriceAmount"] = "699.99",
                ["PriceCurrency"] = "USD",
                ["Description2"] = "desc2",
                ["Description3"] = "desc3"
            };

            if (table == null)
            {
                return values;
            }

            foreach (var row in table.Rows)
            {
                values[row["Field"]] = row["Value"];
            }

            return values;
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

        private static string GetValue(IReadOnlyDictionary<string, string> values, string key)
        {
            if (!values.TryGetValue(key, out var value))
            {
                throw new InvalidOperationException($"Missing '{key}' value in mobile phone contract table.");
            }

            return value;
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
    }
}
