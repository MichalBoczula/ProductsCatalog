using Moq;
using ProductCatalog.Application.Common.Dtos.Common;
using ProductCatalog.Application.Features.Common;
using ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ValueObjects;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using Shouldly;

namespace ProductsCatalog.Application.UnitTests.Features.MobilePhones.Commands;

public class UpdateMobilePhoneCommandHandlerTests
{
    static UpdateMobilePhoneCommandHandlerTests()
    {
        MappingConfig.RegisterMappings();
    }

    [Fact]
    public async Task Handle_ShouldInvokeDependenciesInOrderAndReturnDto()
    {
        // Arrange
        var mobilePhoneId = Guid.NewGuid();
        var command = new UpdateMobilePhoneCommand(
            mobilePhoneId,
            new UpdateMobilePhoneExternalDto(
                new CommonDescriptionExtrernalDto(
                    "Phone Updated",
                    "Updated description",
                    "updated-main-photo",
                    new List<string> { "updated-photo" }),
                new UpdateElectronicDetailsExternalDto(
                    "Updated CPU",
                    "Updated GPU",
                    "12GB",
                    "256GB",
                    "OLED",
                    144,
                    6.9m,
                    75,
                    160,
                    "Li-Poly",
                    5500),
                new UpdateConnectivityExternalDto(true, false, true, false),
                new UpdateSatelliteNavigationSystemExternalDto(true, false, true, true, false),
                new UpdateSensorsExternalDto(true, false, true, true, false, true, false),
                true,
                true,
                Guid.NewGuid(),
                new UpdateMoneyExternalDto(999.99m, "eur")));

        var existingMobilePhone = new MobilePhone(
            new CommonDescription(
                "Original Phone",
                "Original description",
                "original-main-photo",
                new List<string> { "original-photo" }),
            new ElectronicDetails(
                "Original CPU",
                "Original GPU",
                "4GB",
                "64GB",
                "LCD",
                60,
                6.1m,
                68,
                140,
                "Li-Ion",
                3000),
            new Connectivity(false, true, false, true),
            new SatelliteNavigationSystem(true, true, false, false, true),
            new Sensors(true, true, false, false, true, false, true),
            false,
            false,
            Guid.NewGuid(),
            new Money(199.99m, "usd"));

        var mobilePhonesRepoMock = new Mock<IMobilePhonesCommandsRepository>(MockBehavior.Strict);
        var validationPolicyMock = new Mock<IValidationPolicy<MobilePhone>>(MockBehavior.Strict);

        var validationResult = new ValidationResult();
        var sequence = new MockSequence();

        validationPolicyMock.InSequence(sequence)
            .Setup(policy => policy.Validate(It.IsAny<MobilePhone>()))
            .ReturnsAsync(validationResult);

        mobilePhonesRepoMock.InSequence(sequence)
            .Setup(repo => repo.GetById(mobilePhoneId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingMobilePhone);

        validationPolicyMock.InSequence(sequence)
            .Setup(policy => policy.Validate(It.IsAny<MobilePhone>()))
            .ReturnsAsync(validationResult);

        mobilePhonesRepoMock.InSequence(sequence)
            .Setup(repo => repo.Update(It.IsAny<MobilePhone>()));

        mobilePhonesRepoMock.InSequence(sequence)
          .Setup(r => r.WriteHistory(It.IsAny<MobilePhonesHistory>()));

        mobilePhonesRepoMock.InSequence(sequence)
            .Setup(repo => repo.SaveChanges(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new UpdateMobilePhoneCommandHandler(
            mobilePhonesRepoMock.Object,
            validationPolicyMock.Object,
            new UpdateMobilePhoneCommandFlowDescribtor());

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        validationPolicyMock.Verify(policy => policy.Validate(It.IsAny<MobilePhone>()), Times.Exactly(2));
        mobilePhonesRepoMock.Verify(
            repo => repo.GetById(mobilePhoneId, It.IsAny<CancellationToken>()),
            Times.Once);
        mobilePhonesRepoMock.Verify(
            repo => repo.Update(It.Is<MobilePhone>(phone =>
                phone.CommonDescription.Name == command.MobilePhone.CommonDescription.Name &&
                phone.CommonDescription.Description == command.MobilePhone.CommonDescription.Description &&
                phone.CommonDescription.MainPhoto == command.MobilePhone.CommonDescription.MainPhoto &&
                phone.ElectronicDetails.CPU == command.MobilePhone.ElectronicDetails.CPU &&
                phone.ElectronicDetails.GPU == command.MobilePhone.ElectronicDetails.GPU &&
                phone.ElectronicDetails.Ram == command.MobilePhone.ElectronicDetails.Ram &&
                phone.ElectronicDetails.Storage == command.MobilePhone.ElectronicDetails.Storage &&
                phone.ElectronicDetails.DisplayType == command.MobilePhone.ElectronicDetails.DisplayType &&
                phone.ElectronicDetails.RefreshRateHz == command.MobilePhone.ElectronicDetails.RefreshRateHz &&
                phone.ElectronicDetails.ScreenSizeInches == command.MobilePhone.ElectronicDetails.ScreenSizeInches &&
                phone.ElectronicDetails.Width == command.MobilePhone.ElectronicDetails.Width &&
                phone.ElectronicDetails.Height == command.MobilePhone.ElectronicDetails.Height &&
                phone.ElectronicDetails.BatteryType == command.MobilePhone.ElectronicDetails.BatteryType &&
                phone.ElectronicDetails.BatteryCapacity == command.MobilePhone.ElectronicDetails.BatteryCapacity &&
                phone.Connectivity.Has5G == command.MobilePhone.Connectivity.Has5G &&
                phone.Connectivity.WiFi == command.MobilePhone.Connectivity.WiFi &&
                phone.Connectivity.NFC == command.MobilePhone.Connectivity.NFC &&
                phone.Connectivity.Bluetooth == command.MobilePhone.Connectivity.Bluetooth &&
                phone.SatelliteNavigationSystems.GPS == command.MobilePhone.SatelliteNavigationSystems.GPS &&
                phone.SatelliteNavigationSystems.AGPS == command.MobilePhone.SatelliteNavigationSystems.AGPS &&
                phone.SatelliteNavigationSystems.Galileo == command.MobilePhone.SatelliteNavigationSystems.Galileo &&
                phone.SatelliteNavigationSystems.GLONASS == command.MobilePhone.SatelliteNavigationSystems.GLONASS &&
                phone.SatelliteNavigationSystems.QZSS == command.MobilePhone.SatelliteNavigationSystems.QZSS &&
                phone.Sensors.Accelerometer == command.MobilePhone.Sensors.Accelerometer &&
                phone.Sensors.Gyroscope == command.MobilePhone.Sensors.Gyroscope &&
                phone.Sensors.Proximity == command.MobilePhone.Sensors.Proximity &&
                phone.Sensors.Compass == command.MobilePhone.Sensors.Compass &&
                phone.Sensors.Barometer == command.MobilePhone.Sensors.Barometer &&
                phone.Sensors.Halla == command.MobilePhone.Sensors.Halla &&
                phone.Sensors.AmbientLight == command.MobilePhone.Sensors.AmbientLight &&
                phone.FingerPrint == command.MobilePhone.FingerPrint &&
                phone.FaceId == command.MobilePhone.FaceId &&
                phone.CategoryId == command.MobilePhone.CategoryId &&
                phone.Price.Amount == command.MobilePhone.Price.Amount &&
                phone.Price.Currency == command.MobilePhone.Price.Currency.ToUpperInvariant())),
            Times.Once);
        mobilePhonesRepoMock.Verify(
            repo => repo.SaveChanges(It.IsAny<CancellationToken>()),
            Times.Once);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(existingMobilePhone.Id);
        result.CommonDescription.Name.ShouldBe(command.MobilePhone.CommonDescription.Name);
        result.CommonDescription.Description.ShouldBe(command.MobilePhone.CommonDescription.Description);
        result.CommonDescription.MainPhoto.ShouldBe(command.MobilePhone.CommonDescription.MainPhoto);
        result.CommonDescription.OtherPhotos.ShouldBe(command.MobilePhone.CommonDescription.OtherPhotos);
        result.ElectronicDetails.CPU.ShouldBe(command.MobilePhone.ElectronicDetails.CPU);
        result.ElectronicDetails.GPU.ShouldBe(command.MobilePhone.ElectronicDetails.GPU);
        result.ElectronicDetails.Ram.ShouldBe(command.MobilePhone.ElectronicDetails.Ram);
        result.ElectronicDetails.Storage.ShouldBe(command.MobilePhone.ElectronicDetails.Storage);
        result.ElectronicDetails.DisplayType.ShouldBe(command.MobilePhone.ElectronicDetails.DisplayType);
        result.ElectronicDetails.RefreshRateHz.ShouldBe(command.MobilePhone.ElectronicDetails.RefreshRateHz);
        result.ElectronicDetails.ScreenSizeInches.ShouldBe(command.MobilePhone.ElectronicDetails.ScreenSizeInches);
        result.ElectronicDetails.Width.ShouldBe(command.MobilePhone.ElectronicDetails.Width);
        result.ElectronicDetails.Height.ShouldBe(command.MobilePhone.ElectronicDetails.Height);
        result.ElectronicDetails.BatteryType.ShouldBe(command.MobilePhone.ElectronicDetails.BatteryType);
        result.ElectronicDetails.BatteryCapacity.ShouldBe(command.MobilePhone.ElectronicDetails.BatteryCapacity);
        result.Connectivity.Has5G.ShouldBe(command.MobilePhone.Connectivity.Has5G);
        result.Connectivity.WiFi.ShouldBe(command.MobilePhone.Connectivity.WiFi);
        result.Connectivity.NFC.ShouldBe(command.MobilePhone.Connectivity.NFC);
        result.Connectivity.Bluetooth.ShouldBe(command.MobilePhone.Connectivity.Bluetooth);
        result.SatelliteNavigationSystems.GPS.ShouldBe(command.MobilePhone.SatelliteNavigationSystems.GPS);
        result.SatelliteNavigationSystems.AGPS.ShouldBe(command.MobilePhone.SatelliteNavigationSystems.AGPS);
        result.SatelliteNavigationSystems.Galileo.ShouldBe(command.MobilePhone.SatelliteNavigationSystems.Galileo);
        result.SatelliteNavigationSystems.GLONASS.ShouldBe(command.MobilePhone.SatelliteNavigationSystems.GLONASS);
        result.SatelliteNavigationSystems.QZSS.ShouldBe(command.MobilePhone.SatelliteNavigationSystems.QZSS);
        result.Sensors.Accelerometer.ShouldBe(command.MobilePhone.Sensors.Accelerometer);
        result.Sensors.Gyroscope.ShouldBe(command.MobilePhone.Sensors.Gyroscope);
        result.Sensors.Proximity.ShouldBe(command.MobilePhone.Sensors.Proximity);
        result.Sensors.Compass.ShouldBe(command.MobilePhone.Sensors.Compass);
        result.Sensors.Barometer.ShouldBe(command.MobilePhone.Sensors.Barometer);
        result.Sensors.Halla.ShouldBe(command.MobilePhone.Sensors.Halla);
        result.Sensors.AmbientLight.ShouldBe(command.MobilePhone.Sensors.AmbientLight);
        result.FingerPrint.ShouldBe(command.MobilePhone.FingerPrint);
        result.FaceId.ShouldBe(command.MobilePhone.FaceId);
        result.CategoryId.ShouldBe(command.MobilePhone.CategoryId);
        result.Price.Amount.ShouldBe(command.MobilePhone.Price.Amount);
        result.Price.Currency.ShouldBe(command.MobilePhone.Price.Currency.ToUpperInvariant());
        result.IsActive.ShouldBeTrue();
    }

    [Fact]
    public async Task Handle_WhenValidationFails_ShouldThrowValidationException()
    {
        // Arrange
        var mobilePhoneId = Guid.NewGuid();
        var command = new UpdateMobilePhoneCommand(
            mobilePhoneId,
            new UpdateMobilePhoneExternalDto(
                new CommonDescriptionExtrernalDto(
                    "Phone Updated",
                    "Updated description",
                    "updated-main-photo",
                    new List<string> { "updated-photo" }),
                new UpdateElectronicDetailsExternalDto(
                    "Updated CPU",
                    "Updated GPU",
                    "12GB",
                    "256GB",
                    "OLED",
                    144,
                    6.9m,
                    75,
                    160,
                    "Li-Poly",
                    5500),
                new UpdateConnectivityExternalDto(true, false, true, false),
                new UpdateSatelliteNavigationSystemExternalDto(true, false, true, true, false),
                new UpdateSensorsExternalDto(true, false, true, true, false, true, false),
                true,
                true,
                Guid.NewGuid(),
                new UpdateMoneyExternalDto(999.99m, "eur")));

        var mobilePhonesRepoMock = new Mock<IMobilePhonesCommandsRepository>(MockBehavior.Strict);
        var validationPolicyMock = new Mock<IValidationPolicy<MobilePhone>>(MockBehavior.Strict);

        var invalidResult = new ValidationResult();
        invalidResult.AddValidationError(new ValidationError
        {
            Entity = nameof(MobilePhone),
            Name = nameof(MobilePhone.CommonDescription),
            Message = "Invalid description"
        });

        validationPolicyMock
            .Setup(policy => policy.Validate(It.IsAny<MobilePhone>()))
            .ReturnsAsync(invalidResult);

        var handler = new UpdateMobilePhoneCommandHandler(
            mobilePhonesRepoMock.Object,
            validationPolicyMock.Object,
            new UpdateMobilePhoneCommandFlowDescribtor());

        // Act & Assert
        await Should.ThrowAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));

        mobilePhonesRepoMock.Verify(
            repo => repo.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
        mobilePhonesRepoMock.Verify(repo => repo.Update(It.IsAny<MobilePhone>()), Times.Never);
        mobilePhonesRepoMock.Verify(repo => repo.SaveChanges(It.IsAny<CancellationToken>()), Times.Never);
    }
}
