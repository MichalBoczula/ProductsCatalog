using Moq;
using ProductCatalog.Application.Features.MobilePhones.Commands.DeleteMobilePhone;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.Common;
using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ValueObjects;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using Shouldly;
using System.Reflection;

namespace ProductsCatalog.Application.UnitTests.Features.MobilePhones.Commands;

public class DeleteMobilePhoneCommandHandlerTests
{
    static DeleteMobilePhoneCommandHandlerTests()
    {
        MappingConfig.RegisterMappings();
    }

    [Fact]
    public async Task Handle_ShouldDeactivateAndReturnDto()
    {
        // Arrange
        var mobilePhoneId = Guid.NewGuid();
        var command = new DeleteMobilePhoneCommand(mobilePhoneId);

        var mobilePhone = new MobilePhone(
            new CommonDescription(
                "Original Phone",
                "brand",
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
            "12 MP",
            false,
            false,
            Guid.NewGuid(),
            new Money(199.99m, "usd"),
            "desc2",
            "desc3");

        typeof(AggregateRoot)
            .GetProperty(nameof(AggregateRoot.Id), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
            .SetValue(mobilePhone, mobilePhoneId);

        var mobilePhonesRepoMock = new Mock<IMobilePhonesCommandsRepository>(MockBehavior.Strict);
        var validationPolicyMock = new Mock<IValidationPolicy<MobilePhone>>(MockBehavior.Strict);
        var sequence = new MockSequence();

        var validationResult = new ValidationResult();

        mobilePhonesRepoMock.InSequence(sequence)
            .Setup(repo => repo.GetById(mobilePhoneId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mobilePhone);

        validationPolicyMock.InSequence(sequence)
            .Setup(policy => policy.Validate(mobilePhone))
            .ReturnsAsync(validationResult);

        mobilePhonesRepoMock.InSequence(sequence)
            .Setup(repo => repo.Update(It.Is<MobilePhone>(phone => !phone.IsActive)));

        mobilePhonesRepoMock.InSequence(sequence)
          .Setup(r => r.WriteHistory(It.IsAny<MobilePhonesHistory>()));

        mobilePhonesRepoMock.InSequence(sequence)
            .Setup(repo => repo.SaveChanges(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new DeleteMobilePhoneCommandHandler(
            mobilePhonesRepoMock.Object,
            validationPolicyMock.Object,
            new DeleteMobilePhoneCommandFlowDescribtor());

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        validationPolicyMock.Verify(policy => policy.Validate(mobilePhone), Times.Once);
        mobilePhonesRepoMock.Verify(
            repo => repo.GetById(mobilePhoneId, It.IsAny<CancellationToken>()),
            Times.Once);
        mobilePhonesRepoMock.Verify(
            repo => repo.Update(It.Is<MobilePhone>(phone => !phone.IsActive)),
            Times.Once);
        mobilePhonesRepoMock.Verify(
            repo => repo.SaveChanges(It.IsAny<CancellationToken>()),
            Times.Once);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(mobilePhoneId);
        result.IsActive.ShouldBeFalse();
    }

    [Fact]
    public async Task Handle_WhenValidationFails_ShouldThrowValidationException()
    {
        // Arrange
        var mobilePhoneId = Guid.NewGuid();
        var command = new DeleteMobilePhoneCommand(mobilePhoneId);

        var mobilePhone = new MobilePhone(
            new CommonDescription(
                "Original Phone",
                "brand",
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
            "12 MP",
            false,
            false,
            Guid.NewGuid(),
            new Money(199.99m, "usd"),
            "desc2",
            "desc3");

        var mobilePhonesRepoMock = new Mock<IMobilePhonesCommandsRepository>(MockBehavior.Strict);
        var validationPolicyMock = new Mock<IValidationPolicy<MobilePhone>>(MockBehavior.Strict);

        mobilePhonesRepoMock
            .Setup(repo => repo.GetById(mobilePhoneId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mobilePhone);

        var invalidResult = new ValidationResult();
        invalidResult.AddValidationError(new ValidationError
        {
            Entity = nameof(MobilePhone),
            Name = nameof(MobilePhone.CommonDescription),
            Message = "Invalid description"
        });

        validationPolicyMock
            .Setup(policy => policy.Validate(mobilePhone))
            .ReturnsAsync(invalidResult);

        var handler = new DeleteMobilePhoneCommandHandler(
            mobilePhonesRepoMock.Object,
            validationPolicyMock.Object,
            new DeleteMobilePhoneCommandFlowDescribtor());

        // Act & Assert
        await Should.ThrowAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));

        mobilePhonesRepoMock.Verify(
            repo => repo.Update(It.IsAny<MobilePhone>()),
            Times.Never);
        mobilePhonesRepoMock.Verify(
            repo => repo.SaveChanges(It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
