using Moq;
using ProductCatalog.Application.Common.Dtos.Common;
using ProductCatalog.Application.Features.Common;
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using Shouldly;

namespace ProductsCatalog.Application.UnitTests.Features.MobilePhones.Commands;

public class CreateMobilePhoneCommandHandlerTests
{
    static CreateMobilePhoneCommandHandlerTests()
    {
        MappingConfig.RegisterMappings();
    }

    [Fact]
    public async Task Handle_ShouldInvokeDependenciesInOrderAndReturnDto()
    {
        // Arrange
        var command = new CreateMobilePhoneCommand(
            new CreateMobilePhoneExternalDto(
                new CommonDescriptionExtrernalDto(
                    "Phone",
                    "brand",
                    "Good phone",
                    "main-photo",
                    new List<string> { "photo1" }),
                new CreateElectronicDetailsExternalDto(
                    "CPU",
                    "GPU",
                    "8GB",
                    "128GB",
                    "AMOLED",
                    120,
                    6.7m,
                    70,
                    150,
                    "Li-Ion",
                    5000),
                new CreateConnectivityExternalDto(true, true, true, true),
                new CreateSatelliteNavigationSystemExternalDto(true, true, true, false, true),
                new CreateSensorsExternalDto(true, true, true, false, true, false, true),
                "48 MP",
                true,
                false,
                Guid.NewGuid(),
                new CreateMoneyExternalDto(250.5m, "usd"),
                "desc2",
                "desc3"));

        var mobilePhonesRepoMock = new Mock<IMobilePhonesCommandsRepository>(MockBehavior.Strict);
        var validationPolicyMock = new Mock<IValidationPolicy<MobilePhone>>(MockBehavior.Strict);

        var validationResult = new ValidationResult();
        var sequence = new MockSequence();

        validationPolicyMock.InSequence(sequence)
            .Setup(policy => policy.Validate(It.IsAny<MobilePhone>()))
            .ReturnsAsync(validationResult);

        mobilePhonesRepoMock.InSequence(sequence)
            .Setup(repo => repo.Add(It.IsAny<MobilePhone>()));

        mobilePhonesRepoMock.InSequence(sequence)
            .Setup(r => r.WriteHistory(It.IsAny<MobilePhonesHistory>()));

        mobilePhonesRepoMock.InSequence(sequence)
            .Setup(repo => repo.SaveChanges(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new CreateMobilePhoneCommandHandler(
            mobilePhonesRepoMock.Object,
            validationPolicyMock.Object,
            new CreateMobilePhoneCommandFlowDescribtor());

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        validationPolicyMock.Verify(policy => policy.Validate(It.IsAny<MobilePhone>()), Times.Once);

        mobilePhonesRepoMock.Verify(
            repo => repo.Add(It.Is<MobilePhone>(phone =>
                phone.CommonDescription.Name == command.mobilePhoneExternalDto.CommonDescription.Name &&
                phone.CommonDescription.Description == command.mobilePhoneExternalDto.CommonDescription.Description &&
                phone.CommonDescription.MainPhoto == command.mobilePhoneExternalDto.CommonDescription.MainPhoto &&
                phone.ElectronicDetails.CPU == command.mobilePhoneExternalDto.ElectronicDetails.CPU &&
                phone.ElectronicDetails.GPU == command.mobilePhoneExternalDto.ElectronicDetails.GPU &&
                phone.ElectronicDetails.Ram == command.mobilePhoneExternalDto.ElectronicDetails.Ram &&
                phone.ElectronicDetails.Storage == command.mobilePhoneExternalDto.ElectronicDetails.Storage &&
                phone.ElectronicDetails.DisplayType == command.mobilePhoneExternalDto.ElectronicDetails.DisplayType &&
                phone.ElectronicDetails.RefreshRateHz == command.mobilePhoneExternalDto.ElectronicDetails.RefreshRateHz &&
                phone.ElectronicDetails.ScreenSizeInches == command.mobilePhoneExternalDto.ElectronicDetails.ScreenSizeInches &&
                phone.ElectronicDetails.Width == command.mobilePhoneExternalDto.ElectronicDetails.Width &&
                phone.ElectronicDetails.Height == command.mobilePhoneExternalDto.ElectronicDetails.Height &&
                phone.ElectronicDetails.BatteryType == command.mobilePhoneExternalDto.ElectronicDetails.BatteryType &&
                phone.ElectronicDetails.BatteryCapacity == command.mobilePhoneExternalDto.ElectronicDetails.BatteryCapacity &&
                phone.Connectivity.Has5G == command.mobilePhoneExternalDto.Connectivity.Has5G &&
                phone.Connectivity.WiFi == command.mobilePhoneExternalDto.Connectivity.WiFi &&
                phone.Connectivity.NFC == command.mobilePhoneExternalDto.Connectivity.NFC &&
                phone.Connectivity.Bluetooth == command.mobilePhoneExternalDto.Connectivity.Bluetooth &&
                phone.SatelliteNavigationSystems.GPS == command.mobilePhoneExternalDto.SatelliteNavigationSystems.GPS &&
                phone.SatelliteNavigationSystems.AGPS == command.mobilePhoneExternalDto.SatelliteNavigationSystems.AGPS &&
                phone.SatelliteNavigationSystems.Galileo == command.mobilePhoneExternalDto.SatelliteNavigationSystems.Galileo &&
                phone.SatelliteNavigationSystems.GLONASS == command.mobilePhoneExternalDto.SatelliteNavigationSystems.GLONASS &&
                phone.SatelliteNavigationSystems.QZSS == command.mobilePhoneExternalDto.SatelliteNavigationSystems.QZSS &&
                phone.Sensors.Accelerometer == command.mobilePhoneExternalDto.Sensors.Accelerometer &&
                phone.Sensors.Gyroscope == command.mobilePhoneExternalDto.Sensors.Gyroscope &&
                phone.Sensors.Proximity == command.mobilePhoneExternalDto.Sensors.Proximity &&
                phone.Sensors.Compass == command.mobilePhoneExternalDto.Sensors.Compass &&
                phone.Sensors.Barometer == command.mobilePhoneExternalDto.Sensors.Barometer &&
                phone.Sensors.Halla == command.mobilePhoneExternalDto.Sensors.Halla &&
                phone.Sensors.AmbientLight == command.mobilePhoneExternalDto.Sensors.AmbientLight &&
                phone.Camera == command.mobilePhoneExternalDto.Camera &&
                phone.FingerPrint == command.mobilePhoneExternalDto.FingerPrint &&
                phone.FaceId == command.mobilePhoneExternalDto.FaceId &&
                phone.CategoryId == command.mobilePhoneExternalDto.CategoryId &&
                phone.Price.Amount == command.mobilePhoneExternalDto.Price.Amount &&
                phone.Price.Currency == command.mobilePhoneExternalDto.Price.Currency.ToUpperInvariant())),
            Times.Once);

        mobilePhonesRepoMock.Verify(
            repo => repo.SaveChanges(It.IsAny<CancellationToken>()),
            Times.Once);

        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
        result.CommonDescription.Name.ShouldBe(command.mobilePhoneExternalDto.CommonDescription.Name);
        result.CommonDescription.Description.ShouldBe(command.mobilePhoneExternalDto.CommonDescription.Description);
        result.CommonDescription.MainPhoto.ShouldBe(command.mobilePhoneExternalDto.CommonDescription.MainPhoto);
        result.CommonDescription.OtherPhotos.ShouldBe(command.mobilePhoneExternalDto.CommonDescription.OtherPhotos);
        result.ElectronicDetails.CPU.ShouldBe(command.mobilePhoneExternalDto.ElectronicDetails.CPU);
        result.ElectronicDetails.GPU.ShouldBe(command.mobilePhoneExternalDto.ElectronicDetails.GPU);
        result.ElectronicDetails.Ram.ShouldBe(command.mobilePhoneExternalDto.ElectronicDetails.Ram);
        result.ElectronicDetails.Storage.ShouldBe(command.mobilePhoneExternalDto.ElectronicDetails.Storage);
        result.ElectronicDetails.DisplayType.ShouldBe(command.mobilePhoneExternalDto.ElectronicDetails.DisplayType);
        result.ElectronicDetails.RefreshRateHz.ShouldBe(command.mobilePhoneExternalDto.ElectronicDetails.RefreshRateHz);
        result.ElectronicDetails.ScreenSizeInches.ShouldBe(command.mobilePhoneExternalDto.ElectronicDetails.ScreenSizeInches);
        result.ElectronicDetails.Width.ShouldBe(command.mobilePhoneExternalDto.ElectronicDetails.Width);
        result.ElectronicDetails.Height.ShouldBe(command.mobilePhoneExternalDto.ElectronicDetails.Height);
        result.ElectronicDetails.BatteryType.ShouldBe(command.mobilePhoneExternalDto.ElectronicDetails.BatteryType);
        result.ElectronicDetails.BatteryCapacity.ShouldBe(command.mobilePhoneExternalDto.ElectronicDetails.BatteryCapacity);
        result.Connectivity.Has5G.ShouldBe(command.mobilePhoneExternalDto.Connectivity.Has5G);
        result.Connectivity.WiFi.ShouldBe(command.mobilePhoneExternalDto.Connectivity.WiFi);
        result.Connectivity.NFC.ShouldBe(command.mobilePhoneExternalDto.Connectivity.NFC);
        result.Connectivity.Bluetooth.ShouldBe(command.mobilePhoneExternalDto.Connectivity.Bluetooth);
        result.SatelliteNavigationSystems.GPS.ShouldBe(command.mobilePhoneExternalDto.SatelliteNavigationSystems.GPS);
        result.SatelliteNavigationSystems.AGPS.ShouldBe(command.mobilePhoneExternalDto.SatelliteNavigationSystems.AGPS);
        result.SatelliteNavigationSystems.Galileo.ShouldBe(command.mobilePhoneExternalDto.SatelliteNavigationSystems.Galileo);
        result.SatelliteNavigationSystems.GLONASS.ShouldBe(command.mobilePhoneExternalDto.SatelliteNavigationSystems.GLONASS);
        result.SatelliteNavigationSystems.QZSS.ShouldBe(command.mobilePhoneExternalDto.SatelliteNavigationSystems.QZSS);
        result.Sensors.Accelerometer.ShouldBe(command.mobilePhoneExternalDto.Sensors.Accelerometer);
        result.Sensors.Gyroscope.ShouldBe(command.mobilePhoneExternalDto.Sensors.Gyroscope);
        result.Sensors.Proximity.ShouldBe(command.mobilePhoneExternalDto.Sensors.Proximity);
        result.Sensors.Compass.ShouldBe(command.mobilePhoneExternalDto.Sensors.Compass);
        result.Sensors.Barometer.ShouldBe(command.mobilePhoneExternalDto.Sensors.Barometer);
        result.Sensors.Halla.ShouldBe(command.mobilePhoneExternalDto.Sensors.Halla);
        result.Sensors.AmbientLight.ShouldBe(command.mobilePhoneExternalDto.Sensors.AmbientLight);
        result.Camera.ShouldBe(command.mobilePhoneExternalDto.Camera);
        result.FingerPrint.ShouldBe(command.mobilePhoneExternalDto.FingerPrint);
        result.FaceId.ShouldBe(command.mobilePhoneExternalDto.FaceId);
        result.CategoryId.ShouldBe(command.mobilePhoneExternalDto.CategoryId);
        result.Price.Amount.ShouldBe(command.mobilePhoneExternalDto.Price.Amount);
        result.Price.Currency.ShouldBe(command.mobilePhoneExternalDto.Price.Currency.ToUpperInvariant());
        result.IsActive.ShouldBeTrue();
    }

    [Fact]
    public async Task Handle_WhenValidationFails_ShouldThrowValidationException()
    {
        // Arrange
        var command = new CreateMobilePhoneCommand(
            new CreateMobilePhoneExternalDto(
                new CommonDescriptionExtrernalDto(
                    "Phone",
                    "brand",
                    "Good phone",
                    "main-photo",
                    new List<string> { "photo1" }),
                new CreateElectronicDetailsExternalDto(
                    "CPU",
                    "GPU",
                    "8GB",
                    "128GB",
                    "AMOLED",
                    120,
                    6.7m,
                    70,
                    150,
                    "Li-Ion",
                    5000),
                new CreateConnectivityExternalDto(true, true, true, true),
                new CreateSatelliteNavigationSystemExternalDto(true, true, true, false, true),
                new CreateSensorsExternalDto(true, true, true, false, true, false, true),
                "48 MP",
                true,
                false,
                Guid.NewGuid(),
                new CreateMoneyExternalDto(250.5m, "usd"),
                "desc2",
                "desc3"));

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

        var handler = new CreateMobilePhoneCommandHandler(
            mobilePhonesRepoMock.Object,
            validationPolicyMock.Object,
            new CreateMobilePhoneCommandFlowDescribtor());

        // Act & Assert
        await Should.ThrowAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));

        mobilePhonesRepoMock.Verify(repo => repo.Add(It.IsAny<MobilePhone>()), Times.Never);
        mobilePhonesRepoMock.Verify(repo => repo.SaveChanges(It.IsAny<CancellationToken>()), Times.Never);
    }
}
