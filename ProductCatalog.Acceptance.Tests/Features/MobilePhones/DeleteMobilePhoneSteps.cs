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
        public async Task GivenAnExistingMobilePhoneToDelete()
        {
            var categoryId = await CreateCategoryAsync("MOBILE-DEL");
            _createRequest = BuildCreateMobilePhoneRequest(categoryId);

            var response = await TestRunHooks.Client.PostAsJsonAsync("/mobile-phones", _createRequest, _jsonOptions);
            response.EnsureSuccessStatusCode();

            _createdMobilePhone = await response.Content.ReadFromJsonAsync<MobilePhoneDetailsDto>(_jsonOptions);
            _createdMobilePhone.ShouldNotBeNull();
        }

        [When("I submit the delete mobile phone request")]
        public async Task WhenISubmitTheDeleteMobilePhoneRequest()
        {
            _createdMobilePhone.ShouldNotBeNull();

            _response = await TestRunHooks.Client.DeleteAsync($"/mobile-phones/{_createdMobilePhone!.Id}");
            _deletedMobilePhone = await DeserializeResponse<MobilePhoneDetailsDto>(_response);
        }

        [Then("the mobile phone is deleted successfully")]
        public void ThenTheMobilePhoneIsDeletedSuccessfully()
        {
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(HttpStatusCode.OK);

            _deletedMobilePhone.ShouldNotBeNull();
            _deletedMobilePhone!.Id.ShouldBe(_createdMobilePhone!.Id);
            _deletedMobilePhone.IsActive.ShouldBeFalse();
            _deletedMobilePhone.CategoryId.ShouldBe(_createRequest.CategoryId);
            _deletedMobilePhone.FingerPrint.ShouldBe(_createRequest.FingerPrint);
            _deletedMobilePhone.FaceId.ShouldBe(_createRequest.FaceId);
            _deletedMobilePhone.Price.Amount.ShouldBe(_createRequest.Price.Amount);
            _deletedMobilePhone.Price.Currency.ShouldBe(_createRequest.Price.Currency);
            _deletedMobilePhone.CommonDescription.Name.ShouldBe(_createRequest.CommonDescription.Name);
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
            _response = await TestRunHooks.Client.DeleteAsync($"/mobile-phones/{_missingMobilePhoneId}");
            var json = await _response.Content.ReadAsStringAsync();
            _apiProblem = JsonSerializer.Deserialize<ApiProblemDetails>(json, _jsonOptions);
        }

        [Then("the mobile phone deletion fails with validation errors")]
        public void ThenTheMobilePhoneDeletionFailsWithValidationErrors()
        {
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            _apiProblem.ShouldNotBeNull();
            _apiProblem!.Status.ShouldBe(400);
            _apiProblem.Title.ShouldBe("Validation failed");
            _apiProblem.Detail.ShouldBe("One or more validation errors occurred.");
            _apiProblem.Errors.Count().ShouldBeGreaterThan(0);
            _apiProblem.Errors.ShouldContain(error =>
                error.Message == "Mobile phone cannot be null."
                && error.Entity == "MobilePhone"
                && error.Name == "MobilePhonesIsNullValidationRule");
        }

        private static CreateMobilePhoneExternalDto BuildCreateMobilePhoneRequest(Guid categoryId)
        {
            return new CreateMobilePhoneExternalDto(
                new CommonDescriptionExtrernalDto(
                    "Delete Mobile Phone",
                    "Brand",
                    "Phone created for delete test",
                    "delete-main.jpg",
                    new List<string> { "delete-photo-1.jpg", "delete-photo-2.jpg" }),
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
                new CreateMoneyExternalDto(699.99m, "USD"),
                "desc2",
                "desc3");
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
