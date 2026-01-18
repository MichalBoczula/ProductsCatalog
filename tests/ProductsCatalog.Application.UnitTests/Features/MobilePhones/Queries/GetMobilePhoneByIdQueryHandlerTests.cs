using Moq;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneById;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Validation.Common;
using Shouldly;
using System.Text.Json;

namespace ProductsCatalog.Application.UnitTests.Features.MobilePhones.Queries;

public class GetMobilePhoneByIdQueryHandlerTests
{
    static GetMobilePhoneByIdQueryHandlerTests()
    {
        MappingConfig.RegisterMappings();
    }

    [Fact]
    public async Task Handle_ShouldInvokeRepositoryAndMapResult()
    {
        // Arrange
        var mobilePhoneId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var otherPhotos = new List<string> { "photo1", "photo2" };
        var query = new GetMobilePhoneByIdQuery(mobilePhoneId);

        var mobilePhoneReadModel = new MobilePhoneReadModel
        {
            Id = mobilePhoneId,
            Name = "Model X",
            Description = "Flagship device",
            MainPhoto = "main-photo",
            OtherPhotos = JsonSerializer.Serialize<IReadOnlyList<string>>(otherPhotos),
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
            IsActive = true
        };

        var queriesRepositoryMock = new Mock<IMobilePhonesQueriesRepository>(MockBehavior.Strict);

        queriesRepositoryMock
            .Setup(repo => repo.GetById(mobilePhoneId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mobilePhoneReadModel);

        var handler = new GetMobilePhoneByIdQueryHandler(
            queriesRepositoryMock.Object,
            new GetMobilePhoneByIdQueryFlowDescribtor());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        queriesRepositoryMock.Verify(
            repo => repo.GetById(mobilePhoneId, It.IsAny<CancellationToken>()),
            Times.Once);

        result.ShouldNotBeNull();
        result!.Id.ShouldBe(mobilePhoneReadModel.Id);
        result.CommonDescription.Name.ShouldBe(mobilePhoneReadModel.Name);
        result.CommonDescription.Description.ShouldBe(mobilePhoneReadModel.Description);
        result.CommonDescription.OtherPhotos.ShouldBe(otherPhotos);
        result.ElectronicDetails.CPU.ShouldBe(mobilePhoneReadModel.CPU);
        result.Connectivity.Has5G.ShouldBe(mobilePhoneReadModel.Has5G);
        result.Camera.ShouldBe(mobilePhoneReadModel.Camera);
        result.CategoryId.ShouldBe(mobilePhoneReadModel.CategoryId);
        result.Price.Amount.ShouldBe(mobilePhoneReadModel.PriceAmount);
        result.Price.Currency.ShouldBe(mobilePhoneReadModel.PriceCurrency);
    }

    [Fact]
    public async Task Handle_WhenMobilePhoneNotFound_ShouldThrowResourceNotFoundException()
    {
        // Arrange
        var mobilePhoneId = Guid.NewGuid();
        var query = new GetMobilePhoneByIdQuery(mobilePhoneId);

        var queriesRepositoryMock = new Mock<IMobilePhonesQueriesRepository>(MockBehavior.Strict);

        queriesRepositoryMock
            .Setup(repo => repo.GetById(mobilePhoneId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((MobilePhoneReadModel?)null);

        var handler = new GetMobilePhoneByIdQueryHandler(
            queriesRepositoryMock.Object,
            new GetMobilePhoneByIdQueryFlowDescribtor());

        // Act & Assert
        await Should.ThrowAsync<ResourceNotFoundException>(() => handler.Handle(query, CancellationToken.None));

        queriesRepositoryMock.Verify(
            repo => repo.GetById(mobilePhoneId, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
