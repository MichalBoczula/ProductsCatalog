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
        var amount = 2;
        var query = new GetTopMobilePhonesQuery(amount);

        var readModels = new List<MobilePhoneReadModel>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Top Model",
                DisplayType = "OLED",
                ScreenSizeInches = 6.1m,
                Camera = "12 MP",
                PriceAmount = 599.99m,
                PriceCurrency = "USD"
            }
        }.AsReadOnly();

        var expectedDtos = readModels.Adapt<List<TopMobilePhoneDto>>().AsReadOnly();

        var repoMock = new Mock<IMobilePhonesQueriesRepository>(MockBehavior.Strict);
        var flowDescriberMock = new Mock<GetTopMobilePhonesQueryFlowDescribtor>(MockBehavior.Strict);
        var sequence = new MockSequence();

        flowDescriberMock
            .InSequence(sequence)
            .Setup(flow => flow.GetTopMobilePhones(repoMock.Object, amount, It.IsAny<CancellationToken>()))
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
}
