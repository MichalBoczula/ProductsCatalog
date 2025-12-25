using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.AggregateModels.CategoryAggregate
{
    public class CategoryTests
    {
        [Fact]
        public void Deactivate_IsActive_ShouldBeFalse()
        {
            //Arrange
            var category = new Category("test", "desc");
            var actualDate = category.ChangedAt;
            //Act
            category.Deactivate();
            //Assert
            category.IsActive.ShouldBeFalse();
            actualDate.ShouldBeLessThan(category.ChangedAt);
        }

        [Fact]
        public void AssigneNewcategoryInformation_ShouldBeCorrectlyAssigned()
        {
            //Arrange
            var category = new Category("test", "desc");
            var incoming = new Category("newCode", "newName");
            var actualDate = category.ChangedAt;
            //Act
            category.AssigneNewCategoryInformation(incoming);
            //Assert
            category.Code.ShouldBe("newCode");
            category.Name.ShouldBe("newName");
            actualDate.ShouldBeLessThan(category.ChangedAt);
        }

        [Fact]
        public void SetChangeDate_ShouldDateBeAssigned()
        {
            //Arrange
            var category = new Category("test", "desc");
            var actualDate = category.ChangedAt;
            //Act
            category.SetChangeDate();
            //Assert
            actualDate.ShouldBeLessThan(category.ChangedAt);
        }

        [Fact]
        public void CreateAggreageteWithId_ShouldCreateId()
        {
            //Arrange & Act
            var category = new Category("test", "desc");
            //Assert
            category.Id.ShouldNotBe(Guid.Empty);
        }
    }
}
