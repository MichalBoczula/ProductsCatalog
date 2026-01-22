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
        private readonly List<MobilePhoneDto> _createdMobilePhones = new();
        private List<MobilePhoneDto> _result = new();
        private HttpResponseMessage? _response;
        private Guid _categoryId;
        private int _amount;

        [Given("an existing list of mobile phones")]
        public async Task GivenAnExistingListOfMobilePhones()
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
                BuildMobilePhoneRequest("Filter Phone 1"),
                BuildMobilePhoneRequest("Filter Phone 2"),
                BuildMobilePhoneRequest("Filter Phone 3")
            };

            foreach (var request in requests)
            {
                var response = await TestRunHooks.Client.PostAsJsonAsync("/mobile-phones", request);
                response.EnsureSuccessStatusCode();

                var created = await response.Content.ReadFromJsonAsync<MobilePhoneDto>(_jsonOptions);
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
        public async Task ThenTheMobilePhoneListIsReturnedWithTheRequestedAmount()
        {
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(HttpStatusCode.OK);

            _result = await DeserializeResponse<List<MobilePhoneDto>>(_response) ?? new List<MobilePhoneDto>();
            _result.Count.ShouldBe(_amount);
        }

        [Then("an empty mobile phone list is returned")]
        public async Task ThenAnEmptyMobilePhoneListIsReturned()
        {
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(HttpStatusCode.OK);

            _result = await DeserializeResponse<List<MobilePhoneDto>>(_response) ?? new List<MobilePhoneDto>();
            _result.ShouldBeEmpty();
        }

        private CreateMobilePhoneExternalDto BuildMobilePhoneRequest(string name)
        {
            return new CreateMobilePhoneExternalDto(
                new CommonDescriptionExtrernalDto(
                    name,
                    "Phone created by filter test",
                    "main-photo.jpg",
                    new List<string> { "photo-1.jpg", "photo-2.jpg" }),
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
                new CreateMoneyExternalDto(799.99m, "USD"),
                "desc2",
                "desc3");
        }

        private async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }
    }
}
