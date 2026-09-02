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
    public class GetMobilePhonesByIdsSteps
    {
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        private readonly List<Guid> _mobilePhoneIds = new();
        private HttpResponseMessage? _response;
        private Guid _existingMobilePhoneId;
        private Guid _missingMobilePhoneId;

        [Given("an existing mobile phone and a missing mobile phone id")]
        public async Task GivenAnExistingMobilePhoneAndAMissingMobilePhoneId(Table table)
        {
            var categoryRequest = new CreateCategoryExternalDto($"MOBILE-{Guid.NewGuid():N}", "Mobile category");
            var categoryResponse = await TestRunHooks.Client.PostAsJsonAsync("/categories", categoryRequest, _jsonOptions);
            categoryResponse.EnsureSuccessStatusCode();

            var category = await categoryResponse.Content.ReadFromJsonAsync<CategoryDto>(_jsonOptions);
            category.ShouldNotBeNull();

            var values = table.Rows.ToDictionary(row => row["Field"], row => row["Value"], StringComparer.OrdinalIgnoreCase);
            var request = BuildMobilePhoneRequest(category!.Id, values);
            var response = await TestRunHooks.Client.PostAsJsonAsync("/mobile-phones", request, _jsonOptions);
            response.EnsureSuccessStatusCode();

            var created = await response.Content.ReadFromJsonAsync<MobilePhoneDetailsDto>(_jsonOptions);
            created.ShouldNotBeNull();

            _existingMobilePhoneId = created!.Id;
            _missingMobilePhoneId = Guid.NewGuid();
        }

        [When("I request the mobile phones by ids")]
        public async Task WhenIRequestTheMobilePhonesByIds(Table table)
        {
            var requestIds = table.Rows.ToDictionary(
                row => row["Field"],
                row => ReplaceIdPlaceholders(row["Value"]),
                StringComparer.OrdinalIgnoreCase);

            _mobilePhoneIds.Add(Guid.Parse(requestIds["ExistingMobilePhoneId"]));
            _mobilePhoneIds.Add(Guid.Parse(requestIds["MissingMobilePhoneId"]));

            AllureJson.AttachObject("Request JSON (get by ids)", _mobilePhoneIds, _jsonOptions);
            _response = await TestRunHooks.Client.PostAsJsonAsync("/mobile-phones/by-ids", _mobilePhoneIds, _jsonOptions);

            var body = await _response.Content.ReadAsStringAsync();
            AllureJson.AttachRawJson($"Response JSON ({(int)_response.StatusCode})", body);
        }

        [Then("the get mobile phones by ids response is not found")]
        public async Task ThenTheGetMobilePhonesByIdsResponseIsNotFound(Table table)
        {
            var expected = table.Rows.ToDictionary(row => row["Field"], row => row["Value"], StringComparer.OrdinalIgnoreCase);

            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe((HttpStatusCode)int.Parse(expected["StatusCode"]));

            var problem = await _response.Content.ReadFromJsonAsync<NotFoundProblemDetails>(_jsonOptions);
            problem.ShouldNotBeNull();
            problem!.Status.ShouldBe(int.Parse(expected["StatusCode"]));
            problem.Title.ShouldBe(expected["Title"]);
            problem.Detail.ShouldBe(expected["Detail"].Replace("{MissingMobilePhoneId}", _missingMobilePhoneId.ToString()));
            problem.Instance.ShouldBe(expected["Instance"]);
            problem.TraceId.ShouldNotBeNullOrWhiteSpace();
        }

        private string ReplaceIdPlaceholders(string value)
        {
            return value
                .Replace("{MobilePhoneId}", _existingMobilePhoneId.ToString(), StringComparison.OrdinalIgnoreCase)
                .Replace("{MissingMobilePhoneId}", _missingMobilePhoneId.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        private static CreateMobilePhoneExternalDto BuildMobilePhoneRequest(
            Guid categoryId,
            IReadOnlyDictionary<string, string> values)
        {
            return new CreateMobilePhoneExternalDto(
                new CommonDescriptionExtrernalDto(Value("Name"), Value("Brand"), Value("Description"), Value("MainPhoto"),
                    Value("OtherPhotos").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)),
                new CreateElectronicDetailsExternalDto(Value("CPU"), Value("GPU"), Value("Ram"), Value("Storage"),
                    Value("DisplayType"), Int("RefreshRateHz"), Decimal("ScreenSizeInches"), Int("Width"), Int("Height"),
                    Value("BatteryType"), Int("BatteryCapacity")),
                new CreateConnectivityExternalDto(Bool("Has5G"), Bool("WiFi"), Bool("NFC"), Bool("Bluetooth")),
                new CreateSatelliteNavigationSystemExternalDto(Bool("GPS"), Bool("AGPS"), Bool("Galileo"), Bool("GLONASS"), Bool("QZSS")),
                new CreateSensorsExternalDto(Bool("Accelerometer"), Bool("Gyroscope"), Bool("Proximity"), Bool("Compass"),
                    Bool("Barometer"), Bool("Halla"), Bool("AmbientLight")),
                Value("Camera"),
                Bool("FingerPrint"),
                Bool("FaceId"),
                categoryId,
                new CreateMoneyExternalDto(Decimal("PriceAmount"), Value("PriceCurrency")),
                Value("Description2"),
                Value("Description3"));

            string Value(string field) => values[field];
            bool Bool(string field) => bool.Parse(Value(field));
            int Int(string field) => int.Parse(Value(field), CultureInfo.InvariantCulture);
            decimal Decimal(string field) => decimal.Parse(Value(field), CultureInfo.InvariantCulture);
        }
    }
}
