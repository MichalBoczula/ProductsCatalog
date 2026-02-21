using Moq;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Features.MobilePhones.Queries.GetFilteredMobilePhones;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Common.Filters;
using Shouldly;

namespace ProductsCatalog.Application.UnitTests.Features.MobilePhones.Queries;

public sealed class GetFilteredMobilePhonesQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldInvokeFlowStepsInOrder()
    {
        var filter = new MobilePhoneFilterDto
        {
            Brand = "Brand Y",
            MinimalPrice = 200,
            MaximalPrice = 1200
        };

        var query = new GetFilteredMobilePhonesQuery(filter);

        var readModels = new List<MobilePhoneReadModel>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Model X",
                Brand = "Brand Y"
            }
        }.AsReadOnly();

        var expectedDtos = new List<MobilePhoneDto>
        {
            new()
            {
                Id = readModels[0].Id,
                Name = readModels[0].Name,
                Brand = readModels[0].Brand
            }
        };

        var repoMock = new Mock<IMobilePhonesQueriesRepository>(MockBehavior.Strict);
        var flowDescriberMock = new Mock<GetFilteredMobilePhonesQueryFlowDescribtor>(MockBehavior.Strict);
        var sequence = new MockSequence();

        flowDescriberMock
            .InSequence(sequence)
            .Setup(flow => flow.GetFilteredMobilePhones(repoMock.Object, filter, It.IsAny<CancellationToken>()))
            .ReturnsAsync(readModels);

        flowDescriberMock
            .InSequence(sequence)
            .Setup(flow => flow.MapMobilePhonesToDto(readModels))
            .Returns(expectedDtos);

        var handler = new GetFilteredMobilePhonesQueryHandler(repoMock.Object, flowDescriberMock.Object);

        var result = await handler.Handle(query, CancellationToken.None);

        result.ShouldBe(expectedDtos);
        flowDescriberMock.VerifyAll();
    }
}
