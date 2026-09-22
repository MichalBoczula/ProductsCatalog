using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
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
            var currencyCode = "USD";
            var policy = new MobilePhonesValidationPolicy();
            var mobilePhone = CreateMobilePhone(currencyCode, null!, " ");
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
            var currencyCode = "USD";
            var policy = new MobilePhonesValidationPolicy();
            var mobilePhone = CreateMobilePhone(currencyCode, "Description 2", "Description 3", " ", " ");
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
            var policy = new MobilePhonesValidationPolicy();
            //Act
            var result = policy.Describe();
            //Assert
            result.Rules.ShouldContain(r => r.RuleName == "MobilePhonesStringValidationRule");
        }

        private static MobilePhone CreateMobilePhone(
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
                price,
                description2,
                description3);
        }

    }
}
