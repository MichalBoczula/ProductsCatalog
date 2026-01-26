using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ValueObjects;
using ProductCatalog.Domain.ReadModels;
using ProductCatalog.Domain.Validation.Concrete.Policies;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Policies
{
    public class MobilePhonesValidationPolicyTests
    {
        [Fact]
        public async Task Validate_WhenDescriptionsAreInvalid_ShouldReturnErrors()
        {
            //Arrange
            var categoryId = Guid.NewGuid();
            var currencyCode = "USD";
            var policy = new MobilePhonesValidationPolicy(
                new FakeCategoriesQueriesRepository(categoryId),
                new FakeCurrenciesQueriesRepository(currencyCode));
            var mobilePhone = CreateMobilePhone(categoryId, currencyCode, null, " ");
            //Act
            var result = await policy.Validate(mobilePhone);
            //Assert
            result.GetValidatonErrors().Count.ShouldBe(2);
            result.GetValidatonErrors().ShouldContain(e => e.Message.Contains("Description2 cannot be null or whitespace."));
            result.GetValidatonErrors().ShouldContain(e => e.Message.Contains("Description3 cannot be null or whitespace."));
        }

        [Fact]
        public async Task Validate_WhenBrandAndCameraAreInvalid_ShouldReturnErrors()
        {
            //Arrange
            var categoryId = Guid.NewGuid();
            var currencyCode = "USD";
            var policy = new MobilePhonesValidationPolicy(
                new FakeCategoriesQueriesRepository(categoryId),
                new FakeCurrenciesQueriesRepository(currencyCode));
            var mobilePhone = CreateMobilePhone(categoryId, currencyCode, "Description 2", "Description 3", " ", " ");
            //Act
            var result = await policy.Validate(mobilePhone);
            //Assert
            result.GetValidatonErrors().Count.ShouldBe(2);
            result.GetValidatonErrors().ShouldContain(e => e.Message.Contains("Brand cannot be null or whitespace."));
            result.GetValidatonErrors().ShouldContain(e => e.Message.Contains("Camera cannot be null or whitespace."));
        }

        [Fact]
        public void Describe_ShouldIncludeDescriptionsRule()
        {
            //Arrange
            var policy = new MobilePhonesValidationPolicy(
                new FakeCategoriesQueriesRepository(Guid.NewGuid()),
                new FakeCurrenciesQueriesRepository("USD"));
            //Act
            var result = policy.Describe();
            //Assert
            result.Rules.ShouldContain(r => r.RuleName == "MobilePhonesStringValidationRule");
        }

        private static MobilePhone CreateMobilePhone(
            Guid categoryId,
            string currencyCode,
            string description2,
            string description3,
            string brand = "Brand",
            string camera = "Camera")
        {
            var commonDescription = new CommonDescription(
                "Name",
                brand,
                "Description",
                "main-photo.jpg",
                new List<string> { "other-photo.jpg" });
            var electronicDetails = new ElectronicDetails(
                "CPU",
                "GPU",
                "RAM",
                "Storage",
                "Display",
                120,
                6.5m,
                70,
                150,
                "Li-Ion",
                5000);
            var connectivity = new Connectivity(true, true, true, true);
            var navigationSystem = new SatelliteNavigationSystem(true, true, true, true, true);
            var sensors = new Sensors(true, true, true, true, true, true, true);
            var price = new Money(999.99m, currencyCode);

            return new MobilePhone(
                commonDescription,
                electronicDetails,
                connectivity,
                navigationSystem,
                sensors,
                camera,
                true,
                true,
                categoryId,
                price,
                description2,
                description3);
        }

        private sealed class FakeCategoriesQueriesRepository : ICategoriesQueriesRepository
        {
            private readonly Guid _categoryId;

            public FakeCategoriesQueriesRepository(Guid categoryId)
            {
                _categoryId = categoryId;
            }

            public Task<CategoryReadModel?> GetById(Guid id, CancellationToken ct)
            {
                if (id != _categoryId)
                {
                    return Task.FromResult<CategoryReadModel?>(null);
                }

                return Task.FromResult<CategoryReadModel?>(new CategoryReadModel
                {
                    Id = _categoryId,
                    Code = "CAT",
                    Name = "Category",
                    IsActive = true
                });
            }

            public Task<IReadOnlyList<CategoryReadModel>> GetCategories(CancellationToken ct)
            {
                IReadOnlyList<CategoryReadModel> categories = new List<CategoryReadModel>
                {
                    new CategoryReadModel
                    {
                        Id = _categoryId,
                        Code = "CAT",
                        Name = "Category",
                        IsActive = true
                    }
                };

                return Task.FromResult(categories);
            }
        }

        private sealed class FakeCurrenciesQueriesRepository : ICurrenciesQueriesRepository
        {
            private readonly string _currencyCode;

            public FakeCurrenciesQueriesRepository(string currencyCode)
            {
                _currencyCode = currencyCode;
            }

            public Task<IReadOnlyList<CurrencyReadModel>> GetCurrencies(CancellationToken ct)
            {
                IReadOnlyList<CurrencyReadModel> currencies = new List<CurrencyReadModel>
                {
                    new CurrencyReadModel
                    {
                        Id = Guid.NewGuid(),
                        Code = _currencyCode,
                        Description = "Currency",
                        IsActive = true
                    }
                };

                return Task.FromResult(currencies);
            }
        }
    }
}
