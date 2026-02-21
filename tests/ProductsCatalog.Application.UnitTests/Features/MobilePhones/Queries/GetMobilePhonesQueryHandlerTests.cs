using Moq;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhones;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using Shouldly;
using System.Text.Json;

namespace ProductsCatalog.Application.UnitTests.Features.MobilePhones.Queries;

public class GetMobilePhonesQueryHandlerTests
{
    static GetMobilePhonesQueryHandlerTests()
    {
        MappingConfig.RegisterMappings();
    }

    [Fact]
    public async Task Handle_ShouldInvokeRepositoryAndMapMobilePhones()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var amount = 2;
        var otherPhotos = new List<string> { "photo1", "photo2" };
        var otherPhotosJson = JsonSerializer.Serialize<IReadOnlyList<string>>(otherPhotos);
        var mobilePhones = new List<MobilePhoneReadModel>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Model X",
                Brand = "Brand Y",
                Description = "Flagship device",
                MainPhoto = "main-photo",
                OtherPhotos = otherPhotosJson,
                CPU = "CPU",
                GPU = "GPU",
                Ram = "12GB",
                Storage = "256GB",
                DisplayType = "AMOLED",
                RefreshRateHz = 120,
                ScreenSizeInches = 6.8m,
                Width = 70,
                Height = 150,
                BatteryType = "Li-Ion",
                BatteryCapacity = 5000,
                GPS = true,
                AGPS = false,
                Galileo = true,
                GLONASS = false,
                QZSS = true,
                Accelerometer = true,
                Gyroscope = false,
                Proximity = true,
                Compass = false,
                Barometer = true,
                Halla = false,
                AmbientLight = true,
                Has5G = true,
                WiFi = true,
                NFC = true,
                Bluetooth = true,
                Camera = "12 MP",
                FingerPrint = true,
                FaceId = false,
                CategoryId = categoryId,
                PriceAmount = 799.99m,
                PriceCurrency = "USD",
                Description2 = "description 2",
                Description3 = "description 3",
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Model Y",
                Brand = "Brand Y",
                Description = "Budget device",
                MainPhoto = "main-photo-2",
                OtherPhotos = otherPhotosJson,
                CPU = "CPU 2",
                GPU = "GPU 2",
                Ram = "8GB",
                Storage = "128GB",
                DisplayType = "OLED",
                RefreshRateHz = 90,
                ScreenSizeInches = 6.1m,
                Width = 68,
                Height = 145,
                BatteryType = "Li-Po",
                BatteryCapacity = 4300,
                GPS = true,
                AGPS = true,
                Galileo = false,
                GLONASS = true,
                QZSS = false,
                Accelerometer = true,
                Gyroscope = true,
                Proximity = true,
                Compass = true,
                Barometer = false,
                Halla = false,
                AmbientLight = true,
                Has5G = false,
                WiFi = true,
                NFC = false,
                Bluetooth = true,
                Camera = "8 MP",
                FingerPrint = false,
                FaceId = true,
                CategoryId = categoryId,
                PriceAmount = 499.99m,
                PriceCurrency = "USD",
                Description2 = "description 2",
                Description3 = "description 3",
                IsActive = true
            }
        }.AsReadOnly();

        var query = new GetMobilePhonesQuery(amount);
        var validationResult = new ValidationResult();

        var queriesRepositoryMock = new Mock<IMobilePhonesQueriesRepository>(MockBehavior.Strict);
        queriesRepositoryMock
            .Setup(repo => repo.GetPhones(amount, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mobilePhones);

        var queriesValidationPolicyMock = new Mock<IValidationPolicy<int>>(MockBehavior.Strict);
        queriesValidationPolicyMock
            .Setup(repo => repo.Validate(It.IsAny<int>()))
            .ReturnsAsync(validationResult);

        var handler = new GetMobilePhonesQueryHandler(
            queriesRepositoryMock.Object,
            queriesValidationPolicyMock.Object,
            new GetMobilePhonesQueryFlowDescribtor());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        queriesRepositoryMock.Verify(
            repo => repo.GetPhones(amount, It.IsAny<CancellationToken>()),
            Times.Once);

        result.ShouldNotBeNull();
        result.Count.ShouldBe(mobilePhones.Count);
        result.Select(phone => phone.Id).ShouldBe(mobilePhones.Select(phone => phone.Id));
        result.Select(phone => phone.Name).ShouldBe(mobilePhones.Select(phone => phone.Name));
        result.Select(phone => phone.Camera).ShouldBe(mobilePhones.Select(phone => phone.Camera));
        result.Select(phone => phone.Price.Amount).ShouldBe(mobilePhones.Select(phone => phone.PriceAmount));
    }

    [Fact]
    public async Task Handle_WhenMobilePhonesNotFound_ShouldThrowResourceNotFoundException()
    {
        // Arrange
        var amount = 5;
        var query = new GetMobilePhonesQuery(amount);
        var validationResult = new ValidationResult();

        var queriesRepositoryMock = new Mock<IMobilePhonesQueriesRepository>(MockBehavior.Strict);
        queriesRepositoryMock
            .Setup(repo => repo.GetPhones(amount, It.IsAny<CancellationToken>()))
            .ReturnsAsync((IReadOnlyList<MobilePhoneReadModel>?)null);

        var queriesValidationPolicyMock = new Mock<IValidationPolicy<int>>(MockBehavior.Strict);
        queriesValidationPolicyMock
            .Setup(repo => repo.Validate(It.IsAny<int>()))
            .ReturnsAsync(validationResult);


        var handler = new GetMobilePhonesQueryHandler(
            queriesRepositoryMock.Object,
            queriesValidationPolicyMock.Object,
            new GetMobilePhonesQueryFlowDescribtor());

        // Act & Assert
        await Should.ThrowAsync<ResourceNotFoundException>(() => handler.Handle(query, CancellationToken.None));

        queriesRepositoryMock.Verify(
            repo => repo.GetPhones(amount, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
