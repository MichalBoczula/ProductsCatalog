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
using ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone;
using Reqnroll;
using Shouldly;

namespace ProductCatalog.Acceptance.Tests.Features.MobilePhones
{
    [Binding]
    public class UpdateMobilePhoneSteps
    {
        private CreateMobilePhoneExternalDto _createRequest = default!;
        private UpdateMobilePhoneExternalDto _updateRequest = default!;
        private MobilePhoneDetailsDto? _createdMobilePhone;
        private MobilePhoneDetailsDto? _updatedMobilePhone;
        private HttpResponseMessage? _response;
        private ApiProblemDetails? _apiProblem;
        private Guid _missingMobilePhoneId;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        [Given("an existing mobile phone which will be updated")]
        public async Task GivenAnExistingMobilePhoneWhichWillBeUpdated()
        {
            var categoryId = await CreateCategoryAsync("MOBILE-BASE");
            _createRequest = BuildCreateMobilePhoneRequest(categoryId);

            var response = await TestRunHooks.Client.PostAsJsonAsync("/mobile-phones", _createRequest, _jsonOptions);
            response.EnsureSuccessStatusCode();

            _createdMobilePhone = await response.Content.ReadFromJsonAsync<MobilePhoneDetailsDto>(_jsonOptions);
            _createdMobilePhone.ShouldNotBeNull();

            var updatedCategoryId = await CreateCategoryAsync("MOBILE-UPD");
            _updateRequest = BuildUpdateMobilePhoneRequest(updatedCategoryId);
        }

        [When("I submit the update mobile phone request")]
        public async Task WhenISubmitTheUpdateMobilePhoneRequest()
        {
            _createdMobilePhone.ShouldNotBeNull();

            _response = await TestRunHooks.Client.PutAsJsonAsync(
                $"/mobile-phones/{_createdMobilePhone!.Id}",
                _updateRequest,
                _jsonOptions);

            _updatedMobilePhone = await DeserializeResponse<MobilePhoneDetailsDto>(_response);
        }

        [Then("the mobile phone is updated successfully")]
        public void ThenTheMobilePhoneIsUpdatedSuccessfully()
        {
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(HttpStatusCode.OK);

            _updatedMobilePhone.ShouldNotBeNull();
            _updatedMobilePhone!.Id.ShouldBe(_createdMobilePhone!.Id);
            _updatedMobilePhone.CategoryId.ShouldBe(_updateRequest.CategoryId);
            _updatedMobilePhone.FaceId.ShouldBe(_updateRequest.FaceId);
            _updatedMobilePhone.FingerPrint.ShouldBe(_updateRequest.FingerPrint);
            _updatedMobilePhone.Price.Amount.ShouldBe(_updateRequest.Price.Amount);
            _updatedMobilePhone.Price.Currency.ShouldBe(_updateRequest.Price.Currency);
            _updatedMobilePhone.CommonDescription.Name.ShouldBe(_updateRequest.CommonDescription.Name);
            _updatedMobilePhone.CommonDescription.Description.ShouldBe(_updateRequest.CommonDescription.Description);
            _updatedMobilePhone.CommonDescription.MainPhoto.ShouldBe(_updateRequest.CommonDescription.MainPhoto);
            _updatedMobilePhone.CommonDescription.OtherPhotos.ShouldBe(_updateRequest.CommonDescription.OtherPhotos);
            _updatedMobilePhone.ElectronicDetails.CPU.ShouldBe(_updateRequest.ElectronicDetails.CPU);
            _updatedMobilePhone.ElectronicDetails.GPU.ShouldBe(_updateRequest.ElectronicDetails.GPU);
            _updatedMobilePhone.ElectronicDetails.Ram.ShouldBe(_updateRequest.ElectronicDetails.Ram);
            _updatedMobilePhone.ElectronicDetails.Storage.ShouldBe(_updateRequest.ElectronicDetails.Storage);
            _updatedMobilePhone.ElectronicDetails.DisplayType.ShouldBe(_updateRequest.ElectronicDetails.DisplayType);
            _updatedMobilePhone.ElectronicDetails.RefreshRateHz.ShouldBe(_updateRequest.ElectronicDetails.RefreshRateHz);
            _updatedMobilePhone.ElectronicDetails.ScreenSizeInches.ShouldBe(_updateRequest.ElectronicDetails.ScreenSizeInches);
            _updatedMobilePhone.ElectronicDetails.Width.ShouldBe(_updateRequest.ElectronicDetails.Width);
            _updatedMobilePhone.ElectronicDetails.Height.ShouldBe(_updateRequest.ElectronicDetails.Height);
            _updatedMobilePhone.ElectronicDetails.BatteryType.ShouldBe(_updateRequest.ElectronicDetails.BatteryType);
            _updatedMobilePhone.ElectronicDetails.BatteryCapacity.ShouldBe(_updateRequest.ElectronicDetails.BatteryCapacity);
            _updatedMobilePhone.Connectivity.Has5G.ShouldBe(_updateRequest.Connectivity.Has5G);
            _updatedMobilePhone.Connectivity.WiFi.ShouldBe(_updateRequest.Connectivity.WiFi);
            _updatedMobilePhone.Connectivity.NFC.ShouldBe(_updateRequest.Connectivity.NFC);
            _updatedMobilePhone.Connectivity.Bluetooth.ShouldBe(_updateRequest.Connectivity.Bluetooth);
            _updatedMobilePhone.SatelliteNavigationSystems.GPS.ShouldBe(_updateRequest.SatelliteNavigationSystems.GPS);
            _updatedMobilePhone.SatelliteNavigationSystems.AGPS.ShouldBe(_updateRequest.SatelliteNavigationSystems.AGPS);
            _updatedMobilePhone.SatelliteNavigationSystems.Galileo.ShouldBe(_updateRequest.SatelliteNavigationSystems.Galileo);
            _updatedMobilePhone.SatelliteNavigationSystems.GLONASS.ShouldBe(_updateRequest.SatelliteNavigationSystems.GLONASS);
            _updatedMobilePhone.SatelliteNavigationSystems.QZSS.ShouldBe(_updateRequest.SatelliteNavigationSystems.QZSS);
            _updatedMobilePhone.Sensors.Accelerometer.ShouldBe(_updateRequest.Sensors.Accelerometer);
            _updatedMobilePhone.Sensors.Gyroscope.ShouldBe(_updateRequest.Sensors.Gyroscope);
            _updatedMobilePhone.Sensors.Proximity.ShouldBe(_updateRequest.Sensors.Proximity);
            _updatedMobilePhone.Sensors.Compass.ShouldBe(_updateRequest.Sensors.Compass);
            _updatedMobilePhone.Sensors.Barometer.ShouldBe(_updateRequest.Sensors.Barometer);
            _updatedMobilePhone.Sensors.Halla.ShouldBe(_updateRequest.Sensors.Halla);
            _updatedMobilePhone.Sensors.AmbientLight.ShouldBe(_updateRequest.Sensors.AmbientLight);
        }

        [Given("mobile phone identify by id not exists")]
        public async Task GivenMobilePhoneIdentifyByIdNotExists()
        {
            _missingMobilePhoneId = Guid.NewGuid();
            var categoryId = await CreateCategoryAsync("MOBILE-MISSING");
            _updateRequest = BuildUpdateMobilePhoneRequest(categoryId);
        }

        [When("I submit the update mobile phone request for missing mobile phone")]
        public async Task WhenISubmitTheUpdateMobilePhoneRequestForMissingMobilePhone()
        {
            _response = await TestRunHooks.Client.PutAsJsonAsync(
                $"/mobile-phones/{_missingMobilePhoneId}",
                _updateRequest,
                _jsonOptions);

            var json = await _response.Content.ReadAsStringAsync();
            _apiProblem = JsonSerializer.Deserialize<ApiProblemDetails>(json, _jsonOptions);
        }

        [Then("the mobile phone update fails with validation errors")]
        public void ThenTheMobilePhoneUpdateFailsWithValidationErrors()
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
                    "Baseline Mobile Phone",
                    "Brand",
                    "Phone created for update test",
                    "base-main.jpg",
                    new List<string> { "base-photo-1.jpg", "base-photo-2.jpg" }),
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
                new CreateMoneyExternalDto(799.99m, "USD"),
                "desc2",
                "desc3");
        }

        private static UpdateMobilePhoneExternalDto BuildUpdateMobilePhoneRequest(Guid categoryId)
        {
            return new UpdateMobilePhoneExternalDto(
                new CommonDescriptionExtrernalDto(
                    "Updated Mobile Phone", 
                    "Brand",
                    "Updated by acceptance test",
                    "updated-main.jpg",
                    new List<string> { "updated-photo-1.jpg", "updated-photo-2.jpg" }),
                new UpdateElectronicDetailsExternalDto(
                    "Deca-core",
                    "Mali",
                    "12GB",
                    "512GB",
                    "AMOLED",
                    144,
                    6.8m,
                    74,
                    160,
                    "Li-Poly",
                    5200),
                new UpdateConnectivityExternalDto(false, true, false, true),
                new UpdateSatelliteNavigationSystemExternalDto(true, false, true, true, false),
                new UpdateSensorsExternalDto(true, false, true, true, false, true, true),
                "camera",
                false,
                true,
                categoryId,
                new UpdateMoneyExternalDto(899.99m, "EUR"),
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
