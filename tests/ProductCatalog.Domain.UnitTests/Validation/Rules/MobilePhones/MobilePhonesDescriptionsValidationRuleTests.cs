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
    public class MobilePhonesDescriptionsValidationRuleTests
    {
        [Fact]
        public void IsValid_Description2IsNull_ShouldReturnError()
        {
            //Arrange
            var mobilePhone = CreateMobilePhone(null, "Description 3");
            var rule = new MobilePhonesStringValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(mobilePhone, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Description2 cannot be null or whitespace.");
            error.Name.ShouldContain("MobilePhonesDescriptionsValidationRule");
            error.Entity.ShouldContain("MobilePhone");
        }

        [Fact]
        public void IsValid_Description3IsWhitespace_ShouldReturnError()
        {
            //Arrange
            var mobilePhone = CreateMobilePhone("Description 2", "   ");
            var rule = new MobilePhonesStringValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(mobilePhone, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Description3 cannot be null or whitespace.");
            error.Name.ShouldContain("MobilePhonesDescriptionsValidationRule");
            error.Entity.ShouldContain("MobilePhone");
        }

        [Fact]
        public void IsValid_DescriptionsAreValid_ShouldNotReturnErrors()
        {
            //Arrange
            var mobilePhone = CreateMobilePhone("Description 2", "Description 3");
            var rule = new MobilePhonesStringValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(mobilePhone, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count.ShouldBe(0);
        }

        [Fact]
        public void Describe_ShouldReturnCorrectRules()
        {
            //Arrange
            var rule = new MobilePhonesStringValidationRule();
            //Act
            var result = rule.Describe();
            //Assert
            result.Count.ShouldBe(2);
            result.ShouldContain(e => e.Message.Contains("Description2 cannot be null or whitespace."));
            result.ShouldContain(e => e.Message.Contains("Description3 cannot be null or whitespace."));
        }

        private static MobilePhone CreateMobilePhone(string description2, string description3)
        {
            var commonDescription = new CommonDescription(
                "Name",
                "Brand",
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
                description2,
                description3);
        }
    }
}
