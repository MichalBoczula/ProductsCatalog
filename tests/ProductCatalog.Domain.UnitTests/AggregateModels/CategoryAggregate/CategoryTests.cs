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
            //Act
            category.Deactivate();
            //Assert
            category.IsActive.ShouldBeFalse();
        }

        [Fact]
        public void AssigneNewcategoryInformation_ShouldBeCorrectlyAssigned()
        {
            //Arrange
            var category = new Category("test", "desc");
            var incoming = new Category("newCode", "newName");
            //Act
            category.AssigneNewCategoryInformation(incoming);
            //Assert
            category.Code.ShouldBe("newCode");
            category.Name.ShouldBe("newName");
        }
    }
}
