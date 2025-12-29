using Moq;
using ProductCatalog.Application.Features.Currencies.Commands.UpdateCurrency;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.Common;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.History;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using Shouldly;
using System.Reflection;

namespace ProductsCatalog.Application.UnitTests.Features.Currencies.Commands
{
    public class UpdateCurrencyCommandHandlerTests
    {
        static UpdateCurrencyCommandHandlerTests()
        {
            MappingConfig.RegisterMappings();
        }

        [Fact]
        public async Task Handle_ShouldUpdateAndReturnDto()
        {
            // Arrange
            var currencyId = Guid.NewGuid();
            var command = new UpdateCurrencyCommand(
                currencyId,
                new UpdateCurrencyExternalDto("eur", "Euro v2"));

            var existing = new Currency("usd", "US Dollar");
            typeof(AggregateRoot)
                .GetProperty(nameof(AggregateRoot.Id), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
                .SetValue(existing, currencyId);

            var repoMock = new Mock<ICurrenciesCommandsRepository>(MockBehavior.Strict);
            var validationPolicyMock = new Mock<IValidationPolicy<Currency>>(MockBehavior.Strict);
            var sequence = new MockSequence();

            var incomingValidationResult = new ValidationResult();
            var existingValidationResult = new ValidationResult();

            validationPolicyMock.InSequence(sequence)
                .Setup(v => v.Validate(It.IsAny<Currency>()))
                .ReturnsAsync(incomingValidationResult);

            repoMock.InSequence(sequence)
                .Setup(r => r.GetCurrencyById(currencyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existing);

            validationPolicyMock.InSequence(sequence)
                .Setup(v => v.Validate(existing))
                .ReturnsAsync(existingValidationResult);

            repoMock.InSequence(sequence)
                .Setup(r => r.Update(existing));

            repoMock.InSequence(sequence)
                .Setup(r => r.WriteHistory(It.Is<CurrenciesHistory>(h =>
                    h.Operation == Operation.Updated &&
                    h.CurrencyId == currencyId &&
                    h.Code == existing.Code &&
                    h.Description == command.currency.Description)));

            repoMock.InSequence(sequence)
                .Setup(r => r.SaveChanges(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new UpdateCurrencyCommandHandler(
                repoMock.Object,
                validationPolicyMock.Object,
                new UpdateCurrencyCommandFlowDescribtor());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            validationPolicyMock.Verify(v => v.Validate(It.IsAny<Currency>()), Times.Exactly(2));
            repoMock.Verify(r => r.Update(It.Is<Currency>(c =>
                c.Id == currencyId &&
                c.Code == existing.Code &&
                c.Description == command.currency.Description)), Times.Once);

            repoMock.Verify(r => r.WriteHistory(It.IsAny<CurrenciesHistory>()), Times.Once);
            repoMock.Verify(r => r.SaveChanges(It.IsAny<CancellationToken>()), Times.Once);

            result.ShouldNotBeNull();
            result.Id.ShouldBe(currencyId);
            result.Code.ShouldBe(existing.Code);
            result.Description.ShouldBe(command.currency.Description);
            result.IsActive.ShouldBeTrue();
        }

        [Fact]
        public async Task Handle_WhenIncomingValidationFails_ShouldThrowValidationException()
        {
            // Arrange
            var currencyId = Guid.NewGuid();
            var command = new UpdateCurrencyCommand(
                currencyId,
                new UpdateCurrencyExternalDto("eur", "Euro v2"));

            var repoMock = new Mock<ICurrenciesCommandsRepository>(MockBehavior.Strict);
            var validationPolicyMock = new Mock<IValidationPolicy<Currency>>(MockBehavior.Strict);

            var invalid = new ValidationResult();
            invalid.AddValidationError(new ValidationError
            {
                Entity = nameof(Currency),
                Name = nameof(Currency.Code),
                Message = "Invalid code"
            });

            validationPolicyMock
                .Setup(v => v.Validate(It.IsAny<Currency>()))
                .ReturnsAsync(invalid);

            var handler = new UpdateCurrencyCommandHandler(
                repoMock.Object,
                validationPolicyMock.Object,
                new UpdateCurrencyCommandFlowDescribtor());

            // Act & Assert
            await Should.ThrowAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));

            repoMock.Verify(r => r.GetCurrencyById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            repoMock.Verify(r => r.Update(It.IsAny<Currency>()), Times.Never);
            repoMock.Verify(r => r.WriteHistory(It.IsAny<CurrenciesHistory>()), Times.Never);
            repoMock.Verify(r => r.SaveChanges(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
