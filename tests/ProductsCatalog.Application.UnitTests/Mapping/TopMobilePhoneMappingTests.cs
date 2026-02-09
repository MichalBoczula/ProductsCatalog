using Mapster;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using Shouldly;

namespace ProductsCatalog.Application.UnitTests.Mapping;

public sealed class TopMobilePhoneMappingTests
{
    static TopMobilePhoneMappingTests()
    {
        MappingConfig.RegisterMappings();
    }

    [Fact]
    public void MobilePhoneReadModel_ShouldMapTo_TopMobilePhoneDto()
    {
        var readModel = new MobilePhoneReadModel
        {
            Id = Guid.NewGuid(),
            Name = "Top Model",
            DisplayType = "AMOLED",
            ScreenSizeInches = 6.5m,
            Camera = "50 MP",
            PriceAmount = 899.99m,
            PriceCurrency = "USD"
        };

        var dto = readModel.Adapt<TopMobilePhoneDto>();

        dto.Id.ShouldBe(readModel.Id);
        dto.Name.ShouldBe(readModel.Name);
        dto.DisplayType.ShouldBe(readModel.DisplayType);
        dto.ScreenSizeInches.ShouldBe(readModel.ScreenSizeInches);
        dto.Camera.ShouldBe(readModel.Camera);
        dto.Price.Amount.ShouldBe(readModel.PriceAmount);
        dto.Price.Currency.ShouldBe(readModel.PriceCurrency);
    }
}
