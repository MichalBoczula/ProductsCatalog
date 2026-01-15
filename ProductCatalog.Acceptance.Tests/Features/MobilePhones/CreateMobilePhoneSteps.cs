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
        public async Task GivenIHaveValidMobilePhoneDetails()
        {
            var categoryId = await CreateCategoryAsync();
            _validRequest = BuildMobilePhoneRequest(categoryId);
        }

        [When("I submit the create mobile phone request")]
        public async Task WhenISubmitTheCreateMobilePhoneRequest()
        {
            _response = await TestRunHooks.Client.PostAsJsonAsync("/mobile-phones", _validRequest, _jsonOptions);
        }

        [Then("the mobile phone is created successfully")]
        public async Task ThenTheMobilePhoneIsCreatedSuccessfully()
        {
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(HttpStatusCode.Created);

            var mobilePhone = await DeserializeResponse<MobilePhoneDto>(_response);
            mobilePhone.ShouldNotBeNull();

            mobilePhone.Id.ShouldNotBe(Guid.Empty);
            mobilePhone.IsActive.ShouldBeTrue();
            mobilePhone.CategoryId.ShouldBe(_validRequest.CategoryId);
            mobilePhone.FingerPrint.ShouldBe(_validRequest.FingerPrint);
            mobilePhone.FaceId.ShouldBe(_validRequest.FaceId);
            mobilePhone.Price.Amount.ShouldBe(_validRequest.Price.Amount);
            mobilePhone.Price.Currency.ShouldBe(_validRequest.Price.Currency);
            mobilePhone.CommonDescription.Name.ShouldBe(_validRequest.CommonDescription.Name);
            mobilePhone.CommonDescription.Description.ShouldBe(_validRequest.CommonDescription.Description);
            mobilePhone.CommonDescription.MainPhoto.ShouldBe(_validRequest.CommonDescription.MainPhoto);
            mobilePhone.CommonDescription.OtherPhotos.ShouldBe(_validRequest.CommonDescription.OtherPhotos);
            mobilePhone.ElectronicDetails.CPU.ShouldBe(_validRequest.ElectronicDetails.CPU);
            mobilePhone.ElectronicDetails.GPU.ShouldBe(_validRequest.ElectronicDetails.GPU);
            mobilePhone.ElectronicDetails.Ram.ShouldBe(_validRequest.ElectronicDetails.Ram);
            mobilePhone.ElectronicDetails.Storage.ShouldBe(_validRequest.ElectronicDetails.Storage);
        }

        [Given("I have invalid mobile phone details")]
        public void GivenIHaveInvalidMobilePhoneDetails()
        {
            _invalidRequest = BuildMobilePhoneRequest(Guid.NewGuid());
        }

        [When("I submit the create invalid mobile phone request")]
        public async Task WhenISubmitTheCreateInvalidMobilePhoneRequest()
        {
            _response = await TestRunHooks.Client.PostAsJsonAsync("/mobile-phones", _invalidRequest, _jsonOptions);
            var json = await _response.Content.ReadAsStringAsync();
            _apiProblem = JsonSerializer.Deserialize<ApiProblemDetails>(json, _jsonOptions);
        }

        [Then("the mobile phone creation fails with validation errors")]
        public void ThenTheMobilePhoneCreationFailsWithValidationErrors()
        {
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            _apiProblem.ShouldNotBeNull();
            _apiProblem!.Status.ShouldBe(400);
            _apiProblem.Title.ShouldBe("Validation failed");
            _apiProblem.Detail.ShouldBe("One or more validation errors occurred.");
            _apiProblem.Errors.Count().ShouldBeGreaterThan(0);
            _apiProblem.Errors.ShouldContain(error =>
                error.Message == "CategoryId does not exist."
                && error.Entity == "MobilePhone"
                && error.Name == "MobilePhonesCategoryIdValidationRule");
        }

        private static CreateMobilePhoneExternalDto BuildMobilePhoneRequest(Guid categoryId)
        {
            return new CreateMobilePhoneExternalDto(
                new CommonDescriptionExtrernalDto(
                    "Test Mobile Phone",
                    "Phone created by acceptance test",
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
                true,
                true,
                categoryId,
                new CreateMoneyExternalDto(799.99m, "USD"));
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
