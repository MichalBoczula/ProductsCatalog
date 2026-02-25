using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using ProductCatalog.Application.Features.Common;
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;
using ProductCatalog.Domain.Common.Filters;
using Reqnroll;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.MobilePhones
{
    [Binding]
    public class GetFilteredMobilePhonesSteps
    {
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        private HttpResponseMessage? _response;
        private List<MobilePhoneDto> _result = new();
        private Guid _categoryId;

        [Given("existing mobile phones for filtering by brand")]
        public async Task GivenExistingMobilePhonesForFilteringByBrand(Table table)
        {
            var categoryRequest = new CreateCategoryExternalDto($"FILTER-{Guid.NewGuid():N}", "Filter category");
            var categoryResponse = await TestRunHooks.Client.PostAsJsonAsync("/categories", categoryRequest);
            categoryResponse.EnsureSuccessStatusCode();

            var category = await categoryResponse.Content.ReadFromJsonAsync<CategoryDto>(_jsonOptions);
            category.ShouldNotBeNull();
            _categoryId = category!.Id;

            foreach (var row in table.Rows)
            {
                var request = BuildRequest(row["Name"], row["Brand"], decimal.Parse(row["PriceAmount"], CultureInfo.InvariantCulture));

                AllureJson.AttachObject("Request JSON (create for brand filter)", request, _jsonOptions);

                var createResponse = await TestRunHooks.Client.PostAsJsonAsync("/mobile-phones", request, _jsonOptions);
                createResponse.EnsureSuccessStatusCode();

                var body = await createResponse.Content.ReadAsStringAsync();
                AllureJson.AttachRawJson($"Response JSON ({(int)createResponse.StatusCode})", body);
            }
        }

        [When("I filter mobile phones by brand")]
        public async Task WhenIFilterMobilePhonesByBrand(Table table)
        {
            var payload = ParseFilter(table);
            AllureJson.AttachObject("Request JSON (filter mobile phones)", payload, _jsonOptions);

            _response = await TestRunHooks.Client.PostAsJsonAsync("/mobile-phones/filter", payload, _jsonOptions);

            var body = await _response.Content.ReadAsStringAsync();
            AllureJson.AttachRawJson($"Response JSON ({(int)_response.StatusCode})", body);
        }

        [Then("only mobile phones matching brand are returned")]
        public async Task ThenOnlyMobilePhonesMatchingBrandAreReturned(Table table)
        {
            var expected = ParseExpected(table);

            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe((HttpStatusCode)int.Parse(expected["StatusCode"], CultureInfo.InvariantCulture));

            _result = await DeserializeResponse<List<MobilePhoneDto>>(_response) ?? new List<MobilePhoneDto>();
            _result.Count.ShouldBe(int.Parse(expected["Amount"], CultureInfo.InvariantCulture));
            _result.ShouldAllBe(m => m.Brand.Equals(expected["Brand"], StringComparison.OrdinalIgnoreCase));
        }

        private CreateMobilePhoneExternalDto BuildRequest(string name, string brand, decimal priceAmount)
        {
            return new CreateMobilePhoneExternalDto(
                new CommonDescriptionExtrernalDto(
                    name,
                    brand,
                    "Seed for filter acceptance test",
                    "main-photo.jpg",
                    ["photo-1.jpg"]),
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
                _categoryId,
                new CreateMoneyExternalDto(priceAmount, "USD"),
                "desc2",
                "desc3");
        }

        private static MobilePhoneFilterDto ParseFilter(Table table)
        {
            var values = table.Rows.ToDictionary(row => row["Field"], row => row["Value"], StringComparer.OrdinalIgnoreCase);

            return new MobilePhoneFilterDto
            {
                Brand = Enum.Parse<ProductCatalog.Domain.Common.Enums.MobilePhonesBrand>(values["Brand"], true)
            };
        }

        private static Dictionary<string, string> ParseExpected(Table table)
        {
            return table.Rows.ToDictionary(row => row["Field"], row => row["Value"], StringComparer.OrdinalIgnoreCase);
        }

        private async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }
    }
}
