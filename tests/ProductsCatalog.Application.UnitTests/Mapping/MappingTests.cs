using System.Reflection;
using Mapster;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Application.Common.Dtos.Currencies;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using ProductCatalog.Application.Features.Categories.Commands.UpdateCategory;
using ProductCatalog.Application.Features.Currencies.Commands.CreateCurrency;
using ProductCatalog.Application.Features.Currencies.Commands.UpdateCurrency;
using ProductCatalog.Application.Features.Products.Commands.CreateProduct;
using ProductCatalog.Application.Features.Products.Commands.UpdateProduct;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.History;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.History;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.History;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.ValueObjects;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.ReadModels;
using Shouldly;

namespace ProductsCatalog.Application.UnitTests.Mapping
{
    public class MappingTests
    {
        static MappingTests()
        {
            var mappingConfigType = typeof(ProductCatalog.Application.DependencyInjection)
                .Assembly
                .GetType("ProductCatalog.Application.Mapping.MappingConfig", throwOnError: true)!;

            var registerMappingsMethod = mappingConfigType.GetMethod(
                "RegisterMappings",
                BindingFlags.Static | BindingFlags.Public);

            registerMappingsMethod!.Invoke(null, null);
        }

        [Fact]
        public void CreateProductExternalDto_should_map_to_Product_with_Money()
        {
            var externalPrice = new CreateMoneyExternalDto(10.5m, "usd");
            var externalProduct = new CreateProductExternalDto("Phone", "Nice phone", externalPrice, Guid.NewGuid());

            var result = externalProduct.Adapt<Product>();

            result.Name.ShouldBe(externalProduct.Name);
            result.Description.ShouldBe(externalProduct.Description);
            result.CategoryId.ShouldBe(externalProduct.CategoryId);
            result.Price.Amount.ShouldBe(externalProduct.Price.Amount);
            result.Price.Currency.ShouldBe(externalProduct.Price.Currency.ToUpperInvariant());
        }

        [Fact]
        public void Product_should_map_to_ProductDto_with_nested_price()
        {
            var expectedId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var product = new Product("Laptop", "Powerful", new Money(1500, "usd"), categoryId);

            typeof(Product)
                .GetProperty(nameof(Product.Id), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
                .SetValue(product, expectedId);

            var dto = product.Adapt<ProductDto>();

            dto.Id.ShouldBe(expectedId);
            dto.Name.ShouldBe(product.Name);
            dto.Description.ShouldBe(product.Description);
            dto.CategoryId.ShouldBe(categoryId);
            dto.Price.Amount.ShouldBe(product.Price.Amount);
            dto.Price.Currency.ShouldBe(product.Price.Currency);
        }

        [Fact]
        public void UpdateProductExternalDto_should_map_to_Product_with_new_money()
        {
            var updateMoney = new UpdateMoneyExternalDto(75.75m, "gbp");
            var updateDto = new UpdateProductExternalDto("Tablet", "Updated", updateMoney, Guid.NewGuid());

            var product = updateDto.Adapt<Product>();

            product.Name.ShouldBe(updateDto.Name);
            product.Description.ShouldBe(updateDto.Description);
            product.CategoryId.ShouldBe(updateDto.CategoryId);
            product.Price.Amount.ShouldBe(updateMoney.Amount);
            product.Price.Currency.ShouldBe(updateMoney.Currency.ToUpperInvariant());
        }

        [Fact]
        public void ProductReadModel_should_map_to_ProductDto_with_flattened_price()
        {
            var priceAmount = 23.5m;
            var priceCurrency = "EUR";
            var readModel = new ProductReadModel
            {
                Id = Guid.NewGuid(),
                Name = "Monitor",
                Description = "4k",
                PriceAmount = priceAmount,
                PriceCurrency = priceCurrency,
                IsActive = true,
                CategoryId = Guid.NewGuid()
            };

            var dto = readModel.Adapt<ProductDto>();

            dto.Price.Amount.ShouldBe(priceAmount);
            dto.Price.Currency.ShouldBe(priceCurrency);
            dto.Name.ShouldBe(readModel.Name);
            dto.Description.ShouldBe(readModel.Description);
            dto.CategoryId.ShouldBe(readModel.CategoryId);
        }

        [Fact]
        public void CreateCategoryExternalDto_should_map_to_Category()
        {
            var dto = new CreateCategoryExternalDto("tech", "Technology");

            var category = dto.Adapt<Category>();

            category.Code.ShouldBe(dto.Code);
            category.Name.ShouldBe(dto.Name);
        }

        [Fact]
        public void UpdateCategoryExternalDto_should_map_to_Category()
        {
            var dto = new UpdateCategoryExternalDto("home", "Home goods");

            var category = dto.Adapt<Category>();

            category.Code.ShouldBe(dto.Code);
            category.Name.ShouldBe(dto.Name);
        }

        [Fact]
        public void Category_read_model_should_map_to_CategoryDto()
        {
            var readModel = new CategoryReadModel
            {
                Id = Guid.NewGuid(),
                Code = "ELEC",
                Name = "Electronics",
                IsActive = true
            };

            var dto = readModel.Adapt<CategoryDto>();

            dto.Id.ShouldBe(readModel.Id);
            dto.Code.ShouldBe(readModel.Code);
            dto.Name.ShouldBe(readModel.Name);
            dto.IsActive.ShouldBeTrue();
        }

        [Fact]
        public void CreateCurrencyExternalDto_should_map_to_Currency()
        {
            var dto = new CreateCurrencyExternalDto("usd", "US Dollar");

            var currency = dto.Adapt<Currency>();

            currency.Code.ShouldBe(dto.Code);
            currency.Description.ShouldBe(dto.Description);
        }

        [Fact]
        public void UpdateCurrencyExternalDto_should_map_to_Currency()
        {
            var dto = new UpdateCurrencyExternalDto("Canadian Dollar");

            var currency = dto.Adapt<Currency>();

            currency.Description.ShouldBe(dto.Description);
        }

        [Fact]
        public void Currency_read_model_should_map_to_CurrencyDto()
        {
            var readModel = new CurrencyReadModel
            {
                Id = Guid.NewGuid(),
                Code = "USD",
                Description = "US Dollar",
                IsActive = true
            };

            var dto = readModel.Adapt<CurrencyDto>();

            dto.Id.ShouldBe(readModel.Id);
            dto.Code.ShouldBe(readModel.Code);
            dto.Description.ShouldBe(readModel.Description);
            dto.IsActive.ShouldBeTrue();
        }

        [Fact]
        public void Domain_objects_should_map_to_history_records_with_operation_from_context()
        {
            var productId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var currencyId = Guid.NewGuid();

            var product = new Product("Mouse", "Wireless", new Money(40, "usd"), categoryId);
            typeof(Product)
                .GetProperty(nameof(Product.Id), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
                .SetValue(product, productId);

            var category = new Category("accessories", "Accessories");
            typeof(Category)
                .GetProperty(nameof(Category.Id), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
                .SetValue(category, categoryId);

            var currency = new Currency("usd", "US Dollar");
            typeof(Currency)
                .GetProperty(nameof(Currency.Id), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
                .SetValue(currency, currencyId);

            var mapContext = new MapContext
            {
                Parameters = { ["operation"] = Operation.Create }
            };

            var productHistory = product.Adapt<ProductsHistory>(mapContext);
            var categoryHistory = category.Adapt<CategoriesHistory>(mapContext);
            var currencyHistory = currency.Adapt<CurrenciesHistory>(mapContext);

            productHistory.ProductId.ShouldBe(productId);
            productHistory.Operation.ShouldBe(Operation.Create);
            productHistory.Id.ShouldNotBe(Guid.Empty);

            categoryHistory.CategoryId.ShouldBe(categoryId);
            categoryHistory.Operation.ShouldBe(Operation.Create);
            categoryHistory.Id.ShouldNotBe(Guid.Empty);

            currencyHistory.CurrencyId.ShouldBe(currencyId);
            currencyHistory.Operation.ShouldBe(Operation.Create);
            currencyHistory.Id.ShouldNotBe(Guid.Empty);
        }
    }
}
