using Moq;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneByIds;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Validation.Common;
using Shouldly;

namespace ProductsCatalog.Application.UnitTests.Features.MobilePhones.Queries;

public class GetMobilePhoneByIdsQueryHandlerTests
{
    static GetMobilePhoneByIdsQueryHandlerTests()
    {
        MappingConfig.RegisterMappings();
    }

    [Fact]
    public async Task Handle_ShouldInvokeRepositoryAndMapResults()
    {
        // Arrange
        var mobilePhoneIds = new[] { Guid.NewGuid(), Guid.NewGuid() };
        var query = new GetMobilePhoneByIdsQuery(mobilePhoneIds);
        var mobilePhones = new List<MobilePhoneReadModel>
        {
            CreateMobilePhone(mobilePhoneIds[0], "Model X", "Brand X", 799.99m),
            CreateMobilePhone(mobilePhoneIds[1], "Model Y", "Brand Y", 999.99m)
        }.AsReadOnly();

        var queriesRepositoryMock = new Mock<IMobilePhonesQueriesRepository>(MockBehavior.Strict);
        queriesRepositoryMock
            .Setup(repo => repo.GetByIds(mobilePhoneIds, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mobilePhones);

        var handler = new GetMobilePhoneByIdsQueryHandler(
            queriesRepositoryMock.Object,
            new GetMobilePhoneByIdsQueryFlowDescribtor());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        queriesRepositoryMock.Verify(
            repo => repo.GetByIds(mobilePhoneIds, It.IsAny<CancellationToken>()),
            Times.Once);

        result.Count.ShouldBe(2);
        result[0].Id.ShouldBe(mobilePhones[0].Id);
        result[0].Name.ShouldBe(mobilePhones[0].Name);
        result[0].Brand.ShouldBe(mobilePhones[0].Brand);
        result[0].DisplayType.ShouldBe(mobilePhones[0].DisplayType);
        result[0].ScreenSizeInches.ShouldBe(mobilePhones[0].ScreenSizeInches);
        result[0].Camera.ShouldBe(mobilePhones[0].Camera);
        result[0].Price.Amount.ShouldBe(mobilePhones[0].PriceAmount);
        result[0].Price.Currency.ShouldBe(mobilePhones[0].PriceCurrency);
        result[1].Id.ShouldBe(mobilePhones[1].Id);
        result[1].Name.ShouldBe(mobilePhones[1].Name);
        result[1].Price.Amount.ShouldBe(mobilePhones[1].PriceAmount);
    }

    [Fact]
    public async Task Handle_WhenAnyMobilePhoneIsNotFound_ShouldThrowResourceNotFoundException()
    {
        // Arrange
        var foundMobilePhoneId = Guid.NewGuid();
        var missingMobilePhoneId = Guid.NewGuid();
        var mobilePhoneIds = new[] { foundMobilePhoneId, missingMobilePhoneId };
        var query = new GetMobilePhoneByIdsQuery(mobilePhoneIds);
        IReadOnlyList<MobilePhoneReadModel> mobilePhones = new List<MobilePhoneReadModel>
        {
            CreateMobilePhone(foundMobilePhoneId, "Model X", "Brand X", 799.99m)
        }.AsReadOnly();

        var queriesRepositoryMock = new Mock<IMobilePhonesQueriesRepository>(MockBehavior.Strict);
        queriesRepositoryMock
            .Setup(repo => repo.GetByIds(mobilePhoneIds, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mobilePhones);

        var handler = new GetMobilePhoneByIdsQueryHandler(
            queriesRepositoryMock.Object,
            new GetMobilePhoneByIdsQueryFlowDescribtor());

        // Act & Assert
        await Should.ThrowAsync<ResourceNotFoundException>(() => handler.Handle(query, CancellationToken.None));

        queriesRepositoryMock.Verify(
            repo => repo.GetByIds(mobilePhoneIds, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static MobilePhoneReadModel CreateMobilePhone(Guid id, string name, string brand, decimal price)
    {
        return new MobilePhoneReadModel
        {
            Id = id,
            Name = name,
            Brand = brand,
            Description = "Flagship device",
            MainPhoto = "main-photo.jpg",
            OtherPhotos = "[]",
            CPU = "CPU",
            GPU = "GPU",
            Ram = "12 GB",
            Storage = "256 GB",
            DisplayType = "AMOLED",
            RefreshRateHz = 120,
            ScreenSizeInches = 6.8m,
            Width = 70,
            Height = 150,
            BatteryType = "Li-Ion",
            BatteryCapacity = 5000,
            GPS = true,
            AGPS = true,
            Galileo = true,
            GLONASS = true,
            QZSS = true,
            Accelerometer = true,
            Gyroscope = true,
            Proximity = true,
            Compass = true,
            Barometer = true,
            Halla = false,
            AmbientLight = true,
            Has5G = true,
            WiFi = true,
            NFC = true,
            Bluetooth = true,
            Camera = "48 MP",
            FingerPrint = true,
            FaceId = true,
            CategoryId = Guid.NewGuid(),
            PriceAmount = price,
            PriceCurrency = "USD",
            Description2 = "Secondary description",
            Description3 = "Tertiary description",
            IsActive = true
        };
    }
}
