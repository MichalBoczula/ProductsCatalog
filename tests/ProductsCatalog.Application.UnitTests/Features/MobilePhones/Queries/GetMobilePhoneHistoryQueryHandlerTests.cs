using Moq;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetMobilePhoneHistory;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.Common.Pagination;
using ProductCatalog.Domain.Validation.Concrete.Policies;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.Validation.Common;
using Shouldly;
using System.Text.Json;

namespace ProductsCatalog.Application.UnitTests.Features.MobilePhones.Queries;

public class GetMobilePhoneHistoryQueryHandlerTests
{
    static GetMobilePhoneHistoryQueryHandlerTests()
    {
        MappingConfig.RegisterMappings();
    }

    [Fact]
    public async Task Handle_ShouldInvokeRepositoryAndMapHistoryEntries()
    {
        // Arrange
        var mobilePhoneId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var pageNumber = 1;
        var pageSize = 10;
        var otherPhotos = new List<string> { "photo1", "photo2" };
        var otherPhotosJson = JsonSerializer.Serialize<IReadOnlyList<string>>(otherPhotos);
        var changedAt = new DateTime(2024, 3, 4, 10, 15, 0, DateTimeKind.Utc);

        var historyEntries = new List<MobilePhonesHistory>
        {
            new()
            {
                MobilePhoneId = mobilePhoneId,
                Name = "Model X",
                Brand = "BrandY",
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
                IsActive = true,
                ChangedAt = changedAt,
                Operation = Operation.Inserted
            }
        }.AsReadOnly();

        var query = new GetMobilePhoneHistoryQuery(mobilePhoneId, new PaginationParameters(pageNumber, pageSize));

        var queriesRepositoryMock = new Mock<IMobilePhonesQueriesRepository>(MockBehavior.Strict);
        queriesRepositoryMock
            .Setup(repo => repo.GetHistoryOfChanges(mobilePhoneId, pageNumber, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(historyEntries);
        queriesRepositoryMock
            .Setup(repo => repo.GetById(mobilePhoneId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new MobilePhoneReadModel
            {
                Id = mobilePhoneId,
                Name = "Existing phone",
                Brand = "Brand",
                Description = "Description",
                MainPhoto = "main",
                OtherPhotos = "[]",
                CPU = "CPU",
                GPU = "GPU",
                Ram = "8GB",
                Storage = "128GB",
                DisplayType = "OLED",
                RefreshRateHz = 60,
                ScreenSizeInches = 6m,
                Width = 70,
                Height = 150,
                BatteryType = "Li-Ion",
                BatteryCapacity = 4000,
                GPS = true,
                AGPS = true,
                Galileo = true,
                GLONASS = true,
                QZSS = false,
                Accelerometer = true,
                Gyroscope = true,
                Proximity = true,
                Compass = true,
                Barometer = false,
                Halla = false,
                AmbientLight = true,
                Has5G = true,
                WiFi = true,
                NFC = true,
                Bluetooth = true,
                Camera = "12 MP",
                FingerPrint = true,
                FaceId = false,
                CategoryId = Guid.NewGuid(),
                PriceAmount = 100m,
                PriceCurrency = "USD",
                Description2 = "Description 2",
                Description3 = "Description 3",
                IsActive = true
            });

        var handler = new GetMobilePhoneHistoryQueryHandler(
            queriesRepositoryMock.Object,
            new PaginationParametersValidationPolicy(),
            new GetMobilePhoneHistoryQueryFlowDescribtor());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        queriesRepositoryMock.Verify(
            repo => repo.GetHistoryOfChanges(mobilePhoneId, pageNumber, pageSize, It.IsAny<CancellationToken>()),
            Times.Once);

        result.ShouldNotBeNull();
        result.Count.ShouldBe(historyEntries.Count);
        result[0].MobilePhoneId.ShouldBe(mobilePhoneId);
        result[0].CommonDescription.Name.ShouldBe(historyEntries[0].Name);
        result[0].CommonDescription.Description.ShouldBe(historyEntries[0].Description);
        result[0].CommonDescription.OtherPhotos.ShouldBe(otherPhotos);
        result[0].ElectronicDetails.CPU.ShouldBe(historyEntries[0].CPU);
        result[0].Connectivity.Has5G.ShouldBe(historyEntries[0].Has5G);
        result[0].Camera.ShouldBe(historyEntries[0].Camera);
        result[0].CategoryId.ShouldBe(historyEntries[0].CategoryId);
        result[0].Price.Amount.ShouldBe(historyEntries[0].PriceAmount);
        result[0].Price.Currency.ShouldBe(historyEntries[0].PriceCurrency);
        result[0].ChangedAt.ShouldBe(changedAt);
        result[0].Operation.ShouldBe(Operation.Inserted);
    }

    [Fact]
    public async Task Handle_WhenMobilePhoneDoesNotExist_ShouldThrowResourceNotFoundException()
    {
        // Arrange
        var mobilePhoneId = Guid.NewGuid();
        var pageNumber = 1;
        var pageSize = 10;
        var query = new GetMobilePhoneHistoryQuery(mobilePhoneId, new PaginationParameters(pageNumber, pageSize));

        var queriesRepositoryMock = new Mock<IMobilePhonesQueriesRepository>(MockBehavior.Strict);
        queriesRepositoryMock
            .Setup(repo => repo.GetHistoryOfChanges(mobilePhoneId, pageNumber, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync((IReadOnlyList<MobilePhonesHistory>?)null);
        queriesRepositoryMock
            .Setup(repo => repo.GetById(mobilePhoneId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((MobilePhoneReadModel?)null);

        var handler = new GetMobilePhoneHistoryQueryHandler(
            queriesRepositoryMock.Object,
            new PaginationParametersValidationPolicy(),
            new GetMobilePhoneHistoryQueryFlowDescribtor());

        // Act & Assert
        await Should.ThrowAsync<ResourceNotFoundException>(() => handler.Handle(query, CancellationToken.None));

        queriesRepositoryMock.Verify(
            repo => repo.GetById(mobilePhoneId, It.IsAny<CancellationToken>()),
            Times.Once);
        queriesRepositoryMock.Verify(
            repo => repo.GetHistoryOfChanges(mobilePhoneId, pageNumber, pageSize, It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
