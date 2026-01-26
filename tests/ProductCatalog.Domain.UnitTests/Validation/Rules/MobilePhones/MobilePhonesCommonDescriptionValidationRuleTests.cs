using System;
using System.Collections.Generic;
using System.Linq;
using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ValueObjects;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.MobilePhones;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules.MobilePhones
{
    public class MobilePhonesCommonDescriptionValidationRuleTests
    {
        [Fact]
        public void IsValid_BrandIsWhitespace_ShouldReturnError()
        {
            //Arrange
            var mobilePhone = CreateMobilePhone(" ");
            var rule = new MobilePhonesCommonDescriptionValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(mobilePhone, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count.ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Brand cannot be null or whitespace.");
            error.Name.ShouldContain("MobilePhonesCommonDescriptionValidationRule");
            error.Entity.ShouldContain("MobilePhone");
        }

        [Fact]
        public void IsValid_BrandHasValue_ShouldNotReturnError()
        {
            //Arrange
            var mobilePhone = CreateMobilePhone("Brand");
            var rule = new MobilePhonesCommonDescriptionValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(mobilePhone, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count.ShouldBe(0);
        }

        private static MobilePhone CreateMobilePhone(string brand)
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
            var price = new Money(999.99m, "USD");

            return new MobilePhone(
                commonDescription,
                electronicDetails,
                connectivity,
                navigationSystem,
                sensors,
                "Camera",
                true,
                true,
                Guid.NewGuid(),
                price,
                "Description 2",
                "Description 3");
        }
    }
}
