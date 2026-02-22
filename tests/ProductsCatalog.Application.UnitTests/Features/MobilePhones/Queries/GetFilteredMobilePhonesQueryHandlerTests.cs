using Moq;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetFilteredMobilePhones;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.Common.Filters;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using Shouldly;

namespace ProductsCatalog.Application.UnitTests.Features.MobilePhones.Queries;

public sealed class GetFilteredMobilePhonesQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldInvokeFlowStepsInOrder()
    {
        var filter = new MobilePhoneFilterDto
        {
            Brand = MobilePhonesBrand.Samsung,
            MinimalPrice = 200,
            MaximalPrice = 1200
        };

        var query = new GetFilteredMobilePhonesQuery(filter);
        var validationResult = new ValidationResult();
        var brandNameFilter = new MobilePhoneReadFilterDto
        {
            BrandName = "Samsung",
            MinimalPrice = 200,
            MaximalPrice = 1200
        };

        var readModels = new List<MobilePhoneReadModel>
        {
            BuildReadModel()
        }.AsReadOnly();

        var expectedDtos = new List<MobilePhoneDto>
        {
            new()
            {
                Id = readModels[0].Id,
                Name = readModels[0].Name,
                Brand = readModels[0].Brand,
                DisplayType = readModels[0].DisplayType,
                ScreenSizeInches = readModels[0].ScreenSizeInches,
                Camera = readModels[0].Camera,
                Price = new()
                {
                    Amount = readModels[0].PriceAmount,
                    Currency = readModels[0].PriceCurrency
                }
            }
        };

        var repoMock = new Mock<IMobilePhonesQueriesRepository>(MockBehavior.Strict);
        var validationPolicyMock = new Mock<IValidationPolicy<MobilePhoneFilterDto>>(MockBehavior.Strict);
        var flowDescriberMock = new Mock<GetFilteredMobilePhonesQueryFlowDescribtor>(MockBehavior.Strict);
        var sequence = new MockSequence();

        flowDescriberMock
            .InSequence(sequence)
            .Setup(flow => flow.ValidateFilter(filter, validationPolicyMock.Object))
            .ReturnsAsync(validationResult);

        flowDescriberMock
            .InSequence(sequence)
            .Setup(flow => flow.ThrowValidationExceptionIfFilterInvalid(validationResult));

        flowDescriberMock
            .InSequence(sequence)
            .Setup(flow => flow.CastBrandToString(filter))
            .Returns(brandNameFilter);

        flowDescriberMock
            .InSequence(sequence)
            .Setup(flow => flow.GetFilteredMobilePhones(repoMock.Object, brandNameFilter, It.IsAny<CancellationToken>()))
            .ReturnsAsync(readModels);

        flowDescriberMock
            .InSequence(sequence)
            .Setup(flow => flow.MapMobilePhonesToDto(readModels))
            .Returns(expectedDtos);

        var handler = new GetFilteredMobilePhonesQueryHandler(repoMock.Object, validationPolicyMock.Object, flowDescriberMock.Object);

        var result = await handler.Handle(query, CancellationToken.None);

        result.ShouldBe(expectedDtos);
        flowDescriberMock.VerifyAll();
    }

    [Fact]
    public async Task Handle_WhenFilterValidationFails_ShouldThrowValidationException()
    {
        var filter = new MobilePhoneFilterDto { MinimalPrice = -1 };
        var query = new GetFilteredMobilePhonesQuery(filter);

        var validationPolicyMock = new Mock<IValidationPolicy<MobilePhoneFilterDto>>();
        var flowDescriber = new GetFilteredMobilePhonesQueryFlowDescribtor();
        var handler = new GetFilteredMobilePhonesQueryHandler(Mock.Of<IMobilePhonesQueriesRepository>(), validationPolicyMock.Object, flowDescriber);

        var invalidResult = new ValidationResult();
        invalidResult.AddValidationError(new ValidationError { Name = "error", Message = "error", Entity = "entity" });

        validationPolicyMock
            .Setup(policy => policy.Validate(filter))
            .ReturnsAsync(invalidResult);

        await Should.ThrowAsync<ValidationException>(() => handler.Handle(query, CancellationToken.None));
    }

    private static MobilePhoneReadModel BuildReadModel()
    {
        return new MobilePhoneReadModel
        {
            Id = Guid.NewGuid(),
            Name = "Model X",
            Brand = "Brand Y",
            Description = "Flagship device",
            MainPhoto = "main-photo",
            OtherPhotos = "[]",
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
            CategoryId = Guid.NewGuid(),
            PriceAmount = 799.99m,
            PriceCurrency = "USD",
            Description2 = "description 2",
            Description3 = "description 3",
            IsActive = true
        };
    }
}
