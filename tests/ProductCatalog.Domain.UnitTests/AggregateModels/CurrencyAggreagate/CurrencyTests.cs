using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.AggregateModels.CurrencyAggreagate
{
    public class CurrencyTests
    {
        [Fact]
        public void Deactivate_IsActive_ShouldBeFalse()
        {
            //Arrange
            var currency = new Currency("test", "desc");
            var actualDate = currency.ChangedAt;
            //Act
            currency.Deactivate();
            //Assert
            currency.IsActive.ShouldBeFalse();
            actualDate.ShouldBeLessThan(currency.ChangedAt);
        }

        [Fact]
        public void AssigneNewCurrencyInformation_ShouldBeCorrectlyAssigned()
        {
            //Arrange
            var currency = new Currency("test", "desc");
            var incoming = new Currency("newCode", "newName");
            var actualDate = currency.ChangedAt;
            //Act
            currency.AssigneNewCurrencyInformation(incoming);
            //Assert
            currency.Code.ShouldBe("newCode");
            currency.Description.ShouldBe("newName");
            actualDate.ShouldBeLessThan(currency.ChangedAt);
        }

        [Fact]
        public void SetChangeDate_ShouldDateBeAssigned()
        {
            //Arrange
            var currency = new Currency("test", "desc");
            var actualDate = currency.ChangedAt;
            //Act
            currency.SetChangeDate();
            //Assert
            actualDate.ShouldBeLessThan(currency.ChangedAt);
        }

        [Fact]
        public void CreateAggreageteWithId_ShouldCreateId()
        {
            //Arrange & Act
            var currency = new Currency("test", "desc");
            //Assert
            currency.Id.ShouldNotBe(Guid.Empty);
        }
    }
}
