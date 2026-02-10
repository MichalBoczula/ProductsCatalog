using Mapster;
using Moq;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetTopMobilePhones;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using Shouldly;

namespace ProductsCatalog.Application.UnitTests.Features.MobilePhones.Queries;

public sealed class GetTopMobilePhonesQueryHandlerTests
{
    static GetTopMobilePhonesQueryHandlerTests()
    {
        MappingConfig.RegisterMappings();
    }

    [Fact]
    public async Task Handle_ShouldInvokeFlowStepsInOrder()
    {
        var query = new GetTopMobilePhonesQuery();

        var readModels = new List<MobilePhoneReadModel>
        {
            BuildReadModel()
        }.AsReadOnly();

        var expectedDtos = readModels.Adapt<List<TopMobilePhoneDto>>().AsReadOnly();

        var repoMock = new Mock<IMobilePhonesQueriesRepository>(MockBehavior.Strict);
        var flowDescriberMock = new Mock<GetTopMobilePhonesQueryFlowDescribtor>(MockBehavior.Strict);
        var sequence = new MockSequence();

        flowDescriberMock
            .InSequence(sequence)
            .Setup(flow => flow.GetTopMobilePhones(repoMock.Object, It.IsAny<CancellationToken>()))
            .ReturnsAsync(readModels);

        flowDescriberMock
            .InSequence(sequence)
            .Setup(flow => flow.EnsureTopMobilePhonesFound(readModels))
            .Returns(readModels);

        flowDescriberMock
            .InSequence(sequence)
            .Setup(flow => flow.MapTopMobilePhonesToDto(readModels))
            .Returns(expectedDtos);

        var handler = new GetTopMobilePhonesQueryHandler(repoMock.Object, flowDescriberMock.Object);

        var result = await handler.Handle(query, CancellationToken.None);

        result.ShouldBe(expectedDtos);
        flowDescriberMock.VerifyAll();
    }

    private static MobilePhoneReadModel BuildReadModel()
    {
        return new MobilePhoneReadModel
        {
            Id = Guid.NewGuid(),
            Name = "Top Model",
            Brand = "Brand Z",
            Description = "Top tier device",
            MainPhoto = "main-photo",
            OtherPhotos = "[]",
            CPU = "CPU",
            GPU = "GPU",
            Ram = "12GB",
            Storage = "256GB",
            DisplayType = "OLED",
            RefreshRateHz = 120,
            ScreenSizeInches = 6.1m,
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
            CategoryId = Guid.NewGuid(),
            PriceAmount = 599.99m,
            PriceCurrency = "USD",
            Description2 = "description 2",
            Description3 = "description 3",
            IsActive = true
        };
    }
}
