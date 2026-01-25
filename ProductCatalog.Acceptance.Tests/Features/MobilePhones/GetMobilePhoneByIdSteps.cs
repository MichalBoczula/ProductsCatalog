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

namespace ProductCatalog.Acceptance.Tests.Features.MobilePhones
{
    [Binding]
    public class GetMobilePhoneByIdSteps
    {
        private MobilePhoneDto? _createdMobilePhone;
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
        public async Task GivenAnExistingMobilePhoneId()
        {
            var categoryCode = $"MOBILE-{Guid.NewGuid():N}";
            var categoryRequest = new CreateCategoryExternalDto(categoryCode, "Mobile category");
            var categoryResponse = await TestRunHooks.Client.PostAsJsonAsync("/categories", categoryRequest);
            categoryResponse.EnsureSuccessStatusCode();

            var category = await categoryResponse.Content.ReadFromJsonAsync<CategoryDto>(_jsonOptions);
            category.ShouldNotBeNull();

            _categoryId = category!.Id;

            _request = new CreateMobilePhoneExternalDto(
                new CommonDescriptionExtrernalDto(
                    "Test Mobile Phone",
                    "Brand",
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
                "camera",
                true,
                true,
                _categoryId,
                new CreateMoneyExternalDto(799.99m, "USD"),
                "desc2",
                "desc3");

            var response = await TestRunHooks.Client.PostAsJsonAsync("/mobile-phones", _request);
            response.EnsureSuccessStatusCode();

            _createdMobilePhone = await response.Content.ReadFromJsonAsync<MobilePhoneDto>(_jsonOptions);
            _createdMobilePhone.ShouldNotBeNull();

            _mobilePhoneId = _createdMobilePhone!.Id;
        }

        [When("I request the mobile phone by id")]
        public async Task WhenIRequestTheMobilePhoneById()
        {
            _response = await TestRunHooks.Client.GetAsync($"/mobile-phones/{_mobilePhoneId}");
        }

        [Then("the mobile phone details are returned successfully")]
        public async Task ThenTheMobilePhoneDetailsAreReturnedSuccessfully()
        {
            _response.ShouldNotBeNull();
            _response!.StatusCode.ShouldBe(HttpStatusCode.OK);

            var mobilePhone = await DeserializeResponse<MobilePhoneDto>(_response);
            mobilePhone.ShouldNotBeNull();

            mobilePhone.Id.ShouldBe(_mobilePhoneId);
            mobilePhone.CategoryId.ShouldBe(_categoryId);
            mobilePhone.IsActive.ShouldBeTrue();
            mobilePhone.FaceId.ShouldBe(_request!.FaceId);
            mobilePhone.FingerPrint.ShouldBe(_request.FingerPrint);
            mobilePhone.Price.Amount.ShouldBe(_request.Price.Amount);
            mobilePhone.Price.Currency.ShouldBe(_request.Price.Currency);
            mobilePhone.CommonDescription.Name.ShouldBe(_request.CommonDescription.Name);
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
        public async Task ThenResponseShowNotFoundErrorForMobilePhone()
        {
            _responseFailure.ShouldNotBeNull();
            _responseFailure!.StatusCode.ShouldBe(HttpStatusCode.NotFound);

            var problem = await DeserializeResponse<NotFoundProblemDetails>(_responseFailure);
            problem.ShouldNotBeNull();

            problem.Title.ShouldBe("Resource not found.");
            problem.Status.ShouldBe(StatusCodes.Status404NotFound);
            problem.Detail.ShouldBe($"Resource {nameof(MobilePhoneDto)} identify by id {_mobilePhoneId} cannot be found in databese during action {nameof(GetMobilePhoneByIdQuery)}.");
            problem.Instance.ShouldBe($"/mobile-phones/{_mobilePhoneId}");
            problem.TraceId.ShouldNotBeNullOrWhiteSpace();
        }

        private async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }
    }
}
