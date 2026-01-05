//using Mapster;
//using ProductCatalog.Application.Common.Dtos.Categories;
//using ProductCatalog.Application.Common.Dtos.Common;
//using ProductCatalog.Application.Common.Dtos.Currencies;
//using ProductCatalog.Application.Common.Dtos.MobilePhones;
//using ProductCatalog.Application.Common.Dtos.Products;
//using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
//using ProductCatalog.Application.Features.Categories.Commands.UpdateCategory;
//using ProductCatalog.Application.Features.Common;
//using ProductCatalog.Application.Features.Currencies.Commands.CreateCurrency;
//using ProductCatalog.Application.Features.Currencies.Commands.UpdateCurrency;
//using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;
//using ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone;
//using ProductCatalog.Application.Features.Products.Commands.CreateProduct;
//using ProductCatalog.Application.Features.Products.Commands.UpdateProduct;
//using ProductCatalog.Application.Mapping;
//using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
//using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.History;
//using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
//using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
//using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.History;
//using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
//using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
//using ProductCatalog.Domain.AggregatesModel.ProductAggregate.History;
//using ProductCatalog.Domain.Common.Enums;
//using ProductCatalog.Domain.ReadModels;
//using Shouldly;

//namespace ProductsCatalog.Application.UnitTests.Mapping
//{
//    public class MappingTests
//    {
//        static MappingTests()
//        {
//            MappingConfig.RegisterMappings();
//        }

//        [Fact]
//        public void CreateProductExternalDto_ShouldBeMapTo_ProductWithMoney()
//        {
//            //Arrange
//            var externalPrice = new CreateMoneyExternalDto(10.5m, "usd");
//            var externalProduct = new CreateProductExternalDto("Phone", "Nice phone", externalPrice, Guid.NewGuid());

//            //Act
//            var product = externalProduct.Adapt<Product>();

//            //Assert
//            product.Id.ShouldNotBe(Guid.Empty);
//            product.Name.ShouldBe(externalProduct.Name);
//            product.Description.ShouldBe(externalProduct.Description);
//            product.CategoryId.ShouldBe(externalProduct.CategoryId);
//            product.Price.Amount.ShouldBe(externalProduct.Price.Amount);
//            product.Price.Currency.ShouldBe(externalProduct.Price.Currency.ToUpperInvariant());
//            product.IsActive.ShouldBeTrue();
//            product.ChangedAt.ShouldNotBe(DateTime.MinValue);
//        }


//        [Fact]
//        public void UpdateProductExternalDto_ShouldBeMapTo_ProductWithMoney()
//        {
//            //Arrange
//            var updateMoney = new UpdateMoneyExternalDto(75.75m, "gbp");
//            var updateDto = new UpdateProductExternalDto("Tablet", "Updated", updateMoney, Guid.NewGuid());

//            //Act
//            var product = updateDto.Adapt<Product>();

//            //Assert
//            product.Id.ShouldNotBe(Guid.Empty);
//            product.Name.ShouldBe(updateDto.Name);
//            product.Description.ShouldBe(updateDto.Description);
//            product.CategoryId.ShouldBe(updateDto.CategoryId);
//            product.Price.Amount.ShouldBe(updateMoney.Amount);
//            product.Price.Currency.ShouldBe(updateMoney.Currency.ToUpperInvariant());
//            product.IsActive.ShouldBeTrue();
//            product.ChangedAt.ShouldNotBe(DateTime.MinValue);
//        }

//        [Fact]
//        public void Product_ShouldBeMapTo_ProductDtoWithPrice()
//        {
//            //Arrange
//            var categoryId = Guid.NewGuid();
//            var product = new Product("Laptop", "Powerful", new Money(1500, "usd"), categoryId);

//            //Act
//            var dto = product.Adapt<ProductDto>();

//            //Assert
//            dto.Id.ShouldBe(product.Id);
//            dto.Name.ShouldBe(product.Name);
//            dto.Description.ShouldBe(product.Description);
//            dto.Price.Amount.ShouldBe(product.Price.Amount);
//            dto.Price.Currency.ShouldBe(product.Price.Currency);
//            dto.CategoryId.ShouldBe(categoryId);
//            dto.IsActive.ShouldBe(product.IsActive);
//        }

//        [Fact]
//        public void ProductReadModel_ShouldBeMapTo_ProductDtoWithPrice()
//        {
//            //Arrange
//            var priceAmount = 23.5m;
//            var priceCurrency = "EUR";
//            var readModel = new ProductReadModel
//            {
//                Id = Guid.NewGuid(),
//                Name = "Monitor",
//                Description = "4k",
//                PriceAmount = priceAmount,
//                PriceCurrency = priceCurrency,
//                IsActive = true,
//                CategoryId = Guid.NewGuid()
//            };

//            //Act
//            var dto = readModel.Adapt<ProductDto>();

//            //Assert
//            dto.Id.ShouldBe(readModel.Id);
//            dto.Name.ShouldBe(readModel.Name);
//            dto.Description.ShouldBe(readModel.Description);
//            dto.Price.Amount.ShouldBe(priceAmount);
//            dto.Price.Currency.ShouldBe(priceCurrency);
//            dto.IsActive.ShouldBe(readModel.IsActive);
//            dto.CategoryId.ShouldBe(readModel.CategoryId);
//        }

//        [Fact]
//        public void CreateMobilePhoneExternalDto_ShouldBeMapTo_MobilePhone()
//        {
//            //Arrange
//            var commonDescription = new CommonDescriptionExtrernalDto("Phone", "Good phone", "main-photo", new List<string> { "photo1" });
//            var price = new CreateMoneyExternalDto(250.5m, "usd");
//            var dto = new CreateMobilePhoneExternalDto(commonDescription, true, false, Guid.NewGuid(), price);

//            //Act
//            var mobilePhone = dto.Adapt<MobilePhone>();

//            //Assert
//            mobilePhone.Id.ShouldNotBe(Guid.Empty);
//            mobilePhone.CommonDescription.Name.ShouldBe(commonDescription.Name);
//            mobilePhone.CommonDescription.Description.ShouldBe(commonDescription.Description);
//            mobilePhone.CommonDescription.MainPhoto.ShouldBe(commonDescription.MainPhoto);
//            mobilePhone.CommonDescription.OtherPhotos.ShouldBe(commonDescription.OtherPhotos);
//            mobilePhone.FingerPrint.ShouldBeTrue();
//            mobilePhone.FaceId.ShouldBeFalse();
//            mobilePhone.CategoryId.ShouldBe(dto.CategoryId);
//            mobilePhone.Price.Amount.ShouldBe(price.Amount);
//            mobilePhone.Price.Currency.ShouldBe(price.Currency.ToUpperInvariant());
//            mobilePhone.IsActive.ShouldBeTrue();
//            mobilePhone.ChangedAt.ShouldNotBe(DateTime.MinValue);
//        }

//        [Fact]
//        public void UpdateMobilePhoneExternalDto_ShouldBeMapTo_MobilePhone()
//        {
//            //Arrange
//            var commonDescription = new CommonDescriptionExtrernalDto("Phone 2", "Better phone", "main-photo2", new List<string>());
//            var price = new UpdateMoneyExternalDto(300.25m, "eur");
//            var dto = new UpdateMobilePhoneExternalDto(commonDescription, false, true, Guid.NewGuid(), price);

//            //Act
//            var mobilePhone = dto.Adapt<MobilePhone>();

//            //Assert
//            mobilePhone.Id.ShouldNotBe(Guid.Empty);
//            mobilePhone.CommonDescription.Name.ShouldBe(commonDescription.Name);
//            mobilePhone.CommonDescription.Description.ShouldBe(commonDescription.Description);
//            mobilePhone.CommonDescription.MainPhoto.ShouldBe(commonDescription.MainPhoto);
//            mobilePhone.CommonDescription.OtherPhotos.ShouldBe(commonDescription.OtherPhotos);
//            mobilePhone.FingerPrint.ShouldBeFalse();
//            mobilePhone.FaceId.ShouldBeTrue();
//            mobilePhone.CategoryId.ShouldBe(dto.CategoryId);
//            mobilePhone.Price.Amount.ShouldBe(price.Amount);
//            mobilePhone.Price.Currency.ShouldBe(price.Currency.ToUpperInvariant());
//            mobilePhone.IsActive.ShouldBeTrue();
//            mobilePhone.ChangedAt.ShouldNotBe(DateTime.MinValue);
//        }

//        [Fact]
//        public void MobilePhone_ShouldBeMapTo_MobilePhoneDto()
//        {
//            //Arrange
//            var categoryId = Guid.NewGuid();
//            var mobilePhone = new MobilePhone(
//                new CommonDescription("Phone", "Good phone", "main-photo", new List<string>()),
//                true,
//                true,
//                categoryId,
//                new Money(199.99m, "usd"));

//            //Act
//            var dto = mobilePhone.Adapt<MobilePhoneDto>();

//            //Assert
//            dto.Id.ShouldBe(mobilePhone.Id);
//            dto.CommonDescription.Name.ShouldBe(mobilePhone.CommonDescription.Name);
//            dto.CommonDescription.Description.ShouldBe(mobilePhone.CommonDescription.Description);
//            dto.CommonDescription.MainPhoto.ShouldBe(mobilePhone.CommonDescription.MainPhoto);
//            dto.CommonDescription.OtherPhotos.ShouldBe(mobilePhone.CommonDescription.OtherPhotos);
//            dto.FingerPrint.ShouldBe(mobilePhone.FingerPrint);
//            dto.FaceId.ShouldBe(mobilePhone.FaceId);
//            dto.CategoryId.ShouldBe(categoryId);
//            dto.Price.Amount.ShouldBe(mobilePhone.Price.Amount);
//            dto.Price.Currency.ShouldBe(mobilePhone.Price.Currency);
//        }

//        [Fact]
//        public void CreateCategoryExternalDto_ShouldBeMapTo_Category()
//        {
//            //Arrange
//            var dto = new CreateCategoryExternalDto("tech", "Technology");

//            //Act
//            var category = dto.Adapt<Category>();

//            //Assert
//            category.Id.ShouldNotBe(Guid.Empty);
//            category.Code.ShouldBe(dto.Code);
//            category.Name.ShouldBe(dto.Name);
//            category.IsActive.ShouldBeTrue();
//            category.ChangedAt.ShouldNotBe(DateTime.MinValue);
//        }

//        [Fact]
//        public void UpdateCategoryExternalDto_ShouldBeMapTo_Category()
//        {
//            //Arrange
//            var dto = new UpdateCategoryExternalDto("home", "Home goods");

//            //Act
//            var category = dto.Adapt<Category>();

//            //Assert
//            category.Id.ShouldNotBe(Guid.Empty);
//            category.Code.ShouldBe(dto.Code);
//            category.Name.ShouldBe(dto.Name);
//            category.IsActive.ShouldBeTrue();
//            category.ChangedAt.ShouldNotBe(DateTime.MinValue);
//        }

//        [Fact]
//        public void Category_ShouldBeMapTo_CategoryDto()
//        {
//            //Arrange
//            var category = new Category("home", "Home goods");

//            //Act
//            var dto = category.Adapt<CategoryDto>();

//            //Assert
//            dto.Id.ShouldBe(category.Id);
//            dto.Code.ShouldBe(category.Code);
//            dto.Name.ShouldBe(category.Name);
//            dto.IsActive.ShouldBe(category.IsActive);
//        }

//        [Fact]
//        public void CategoryReadModel_ShouldBeMapTo_CategoryDto()
//        {
//            //Arrange
//            var readModel = new CategoryReadModel
//            {
//                Id = Guid.NewGuid(),
//                Code = "ELEC",
//                Name = "Electronics",
//                IsActive = true
//            };

//            //Act
//            var dto = readModel.Adapt<CategoryDto>();

//            //Assert
//            dto.Id.ShouldBe(readModel.Id);
//            dto.Code.ShouldBe(readModel.Code);
//            dto.Name.ShouldBe(readModel.Name);
//            dto.IsActive.ShouldBeTrue();
//        }

//        [Fact]
//        public void CreateCurrencyExternalDto_ShouldBeMapTo_Currency()
//        {
//            //Arrange
//            var dto = new CreateCurrencyExternalDto("usd", "US Dollar");

//            //Act
//            var currency = dto.Adapt<Currency>();

//            //Assert
//            currency.Id.ShouldNotBe(Guid.Empty);
//            currency.Code.ShouldBe(dto.Code);
//            currency.Description.ShouldBe(dto.Description);
//            currency.IsActive.ShouldBeTrue();
//            currency.ChangedAt.ShouldNotBe(DateTime.MinValue);
//        }

//        [Fact]
//        public void UpdateCurrencyExternalDto_ShouldBeMapTo_Currency()
//        {
//            //Arrange
//            var dto = new UpdateCurrencyExternalDto("CAD", "Canadian Dollar");

//            //Act
//            var currency = dto.Adapt<Currency>();

//            //Assert
//            currency.Id.ShouldNotBe(Guid.Empty);
//            currency.Description.ShouldBe(dto.Description);
//            currency.Code.ShouldBe(dto.Code);
//            currency.IsActive.ShouldBeTrue();
//            currency.ChangedAt.ShouldNotBe(DateTime.MinValue);
//        }

//        [Fact]
//        public void Currency_ShouldBeMapTo_CurrencyDto()
//        {
//            //Arrange
//            var currency = new Currency("CAD", "Canadian Dollar");

//            //Act
//            var dto = currency.Adapt<CurrencyDto>();

//            //Assert
//            dto.Id.ShouldBe(currency.Id);
//            dto.Code.ShouldBe(currency.Code);
//            dto.Description.ShouldBe(currency.Description);
//            dto.IsActive.ShouldBe(currency.IsActive);
//        }

//        [Fact]
//        public void CurrencyReadModel_ShouldBeMapTo_CurrencyDto()
//        {
//            //Arrange
//            var readModel = new CurrencyReadModel
//            {
//                Id = Guid.NewGuid(),
//                Code = "USD",
//                Description = "US Dollar",
//                IsActive = true
//            };

//            //Act
//            var dto = readModel.Adapt<CurrencyDto>();

//            //Assert
//            dto.Id.ShouldBe(readModel.Id);
//            dto.Code.ShouldBe(readModel.Code);
//            dto.Description.ShouldBe(readModel.Description);
//            dto.IsActive.ShouldBeTrue();
//        }

//        ///Whole history mapping tests
//        [Fact]
//        public void Product_ShouldBeMapTo_ProductsHistory()
//        {
//            //Arrange
//            var categoryId = Guid.NewGuid();
//            var product = new Product("Console", "Next gen", new Money(499.99m, "usd"), categoryId);
//            var operation = Operation.Updated;

//            //Act
//            var history = product.BuildAdapter()
//                .AddParameters("operation", operation)
//                .AdaptToType<ProductsHistory>();

//            //Assert
//            history.Id.ShouldNotBe(Guid.Empty);
//            history.ProductId.ShouldBe(product.Id);
//            history.Name.ShouldBe(product.Name);
//            history.Description.ShouldBe(product.Description);
//            history.PriceAmount.ShouldBe(product.Price.Amount);
//            history.PriceCurrency.ShouldBe(product.Price.Currency);
//            history.CategoryId.ShouldBe(categoryId);
//            history.IsActive.ShouldBe(product.IsActive);
//            history.ChangedAt.ShouldBe(product.ChangedAt);
//            history.Operation.ShouldBe(operation);
//        }

//        [Fact]
//        public void Category_ShouldBeMapTo_CategoriesHistory()
//        {
//            //Arrange
//            var category = new Category("sport", "Sport");
//            var operation = Operation.Inserted;

//            //Act
//            var history = category.BuildAdapter()
//                .AddParameters("operation", operation)
//                .AdaptToType<CategoriesHistory>();

//            //Assert
//            history.Id.ShouldNotBe(Guid.Empty);
//            history.CategoryId.ShouldBe(category.Id);
//            history.Code.ShouldBe(category.Code);
//            history.Name.ShouldBe(category.Name);
//            history.IsActive.ShouldBe(category.IsActive);
//            history.ChangedAt.ShouldBe(category.ChangedAt);
//            history.Operation.ShouldBe(operation);
//        }

//        [Fact]
//        public void Currency_ShouldBeMapTo_CurrenciesHistory()
//        {
//            //Arrange
//            var currency = new Currency("eur", "Euro");
//            var operation = Operation.Deleted;

//            //Act
//            var history = currency.BuildAdapter()
//                .AddParameters("operation", operation)
//                .AdaptToType<CurrenciesHistory>();

//            //Assert
//            history.Id.ShouldNotBe(Guid.Empty);
//            history.CurrencyId.ShouldBe(currency.Id);
//            history.Code.ShouldBe(currency.Code);
//            history.Description.ShouldBe(currency.Description);
//            history.IsActive.ShouldBe(currency.IsActive);
//            history.ChangedAt.ShouldBe(currency.ChangedAt);
//            history.Operation.ShouldBe(operation);
//        }
//    }
//}
