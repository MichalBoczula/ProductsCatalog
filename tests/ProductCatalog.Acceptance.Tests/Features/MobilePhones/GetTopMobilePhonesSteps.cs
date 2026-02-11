using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Application.Common.Dtos.Common;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
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
    public class GetTopMobilePhonesSteps
    {
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        private readonly List<MobilePhoneDetailsDto> _createdMobilePhones = new();
        private HttpResponseMessage? _response;
        private Guid _categoryId;

        [Given("an existing set of mobile phones for top list")]
        public async Task GivenAnExistingSetOfMobilePhonesForTopList()
        {
            await ClearMobilePhones();
            await EnsureCategoryExists();

            foreach (var name in new[] { "Top Phone 1", "Top Phone 2", "Top Phone 3" })
            {
                var request = BuildMobilePhoneRequest(name);

                AllureJson.AttachObject("Request JSON (create for top)", request, _jsonOptions);

                var response = await TestRunHooks.Client.PostAsJsonAsync("/mobile-phones", request, _jsonOptions);
                response.EnsureSuccessStatusCode();

                var body = await response.Content.ReadAsStringAsync();
                AllureJson.AttachRawJson($"Response JSON ({(int)response.StatusCode})", body);

                var created = JsonSerializer.Deserialize<MobilePhoneDetailsDto>(body, _jsonOptions);
                created.ShouldNotBeNull();

                _createdMobilePhones.Add(created!);
            }
        }

        [Given("mobile phones table is empty for top list")]
        public async Task GivenNoMobilePhonesExistInTheDatabase()
        {
            await ClearMobilePhones();
        }

        [When("I request the top mobile phones list")]
        public async Task WhenIRequestTheTopMobilePhonesList()
        {
            AllureJson.AttachObject(
                "Request JSON (get top)",
                new { Endpoint = "/mobile-phones/top" },
                _jsonOptions);

            _response = await TestRunHooks.Client.GetAsync("/mobile-phones/top");

            var body = await _response.Content.ReadAsStringAsync();
            AllureJson.AttachRawJson($"Response JSON ({(int)_response.StatusCode})", body);
        }

        [Then("the top mobile phones response is successful and contains records")]
        public async Task ThenTheTopMobilePhonesResponseIsSuccessfulAndContainsRecords()
        {
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(HttpStatusCode.OK);

            var result = await DeserializeResponse<List<TopMobilePhoneDto>>(_response);
            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThan(0);

            foreach (var created in _createdMobilePhones)
            {
                result.Any(item => item.Id == created.Id).ShouldBeTrue();
            }
        }

        [Then("the top mobile phones response is not found")]
        public async Task ThenTheTopMobilePhonesResponseIsNotFound()
        {
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(HttpStatusCode.NotFound);

            var problem = await DeserializeResponse<NotFoundProblemDetails>(_response);
            problem.ShouldNotBeNull();
            problem.Status.ShouldBe((int)HttpStatusCode.NotFound);
            problem.Title.ShouldBe("Resource not found.");
            problem.Instance.ShouldBe("/mobile-phones/top");
            problem.TraceId.ShouldNotBeNullOrWhiteSpace();
        }

        private async Task EnsureCategoryExists()
        {
            var categoryCode = $"MOBILE-{Guid.NewGuid():N}";
            var categoryRequest = new CreateCategoryExternalDto(categoryCode, "Mobile category");

            var categoryResponse = await TestRunHooks.Client.PostAsJsonAsync("/categories", categoryRequest);
            categoryResponse.EnsureSuccessStatusCode();

            var category = await categoryResponse.Content.ReadFromJsonAsync<CategoryDto>(_jsonOptions);
            category.ShouldNotBeNull();
            _categoryId = category!.Id;
        }

        private async Task ClearMobilePhones()
        {
            using var scope = TestRunHooks.Factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ProductsContext>();

            context.MobilePhonesHistories.RemoveRange(context.MobilePhonesHistories);
            context.MobilePhones.RemoveRange(context.MobilePhones);

            await context.SaveChangesAsync();
        }

        private CreateMobilePhoneExternalDto BuildMobilePhoneRequest(string name)
        {
            return new CreateMobilePhoneExternalDto(
                new CommonDescriptionExtrernalDto(
                    name,
                    "Brand",
                    "Phone created by top endpoint acceptance test",
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
