using Moq;
using ProductCatalog.Application.Features.Currencies.Commands.DeleteCurrency;
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
    public class DeleteCurrencyCommandHandlerTests
    {
        static DeleteCurrencyCommandHandlerTests()
        {
            MappingConfig.RegisterMappings();
        }

        [Fact]
        public async Task Handle_ShouldDeactivateAndReturnDto()
        {
            // Arrange
            var currencyId = Guid.NewGuid();
            var command = new DeleteCurrencyCommand(currencyId);

            var currency = new Currency("usd", "US Dollar");
            typeof(AggregateRoot)
                .GetProperty(nameof(AggregateRoot.Id), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
                .SetValue(currency, currencyId);

            var repoMock = new Mock<ICurrenciesCommandsRepository>(MockBehavior.Strict);
            var validationPolicyMock = new Mock<IValidationPolicy<Currency>>(MockBehavior.Strict);
            var sequence = new MockSequence();

            var validationResult = new ValidationResult();

            repoMock.InSequence(sequence)
                .Setup(r => r.GetCurrencyById(currencyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(currency);

            validationPolicyMock.InSequence(sequence)
                .Setup(v => v.Validate(currency))
                .ReturnsAsync(validationResult);

            repoMock.InSequence(sequence)
                .Setup(r => r.Update(currency));

            repoMock.InSequence(sequence)
                .Setup(r => r.WriteHistory(It.Is<CurrenciesHistory>(h =>
                    h.Operation == Operation.Deleted &&
                    h.CurrencyId == currencyId &&
                    h.Code == currency.Code &&
                    h.Description == currency.Description)));

            repoMock.InSequence(sequence)
                .Setup(r => r.SaveChanges(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new DeleteCurrencyCommandHandler(
                repoMock.Object,
                validationPolicyMock.Object,
                new DeleteCurrencyCommandFlowDescribtor());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            validationPolicyMock.Verify(v => v.Validate(currency), Times.Once);
            repoMock.Verify(r => r.Update(currency), Times.Once);
            repoMock.Verify(r => r.WriteHistory(It.IsAny<CurrenciesHistory>()), Times.Once);
            repoMock.Verify(r => r.SaveChanges(It.IsAny<CancellationToken>()), Times.Once);

            result.ShouldNotBeNull();
            result.Id.ShouldBe(currencyId);
            result.IsActive.ShouldBeFalse();
        }

        [Fact]
        public async Task Handle_WhenValidationFails_ShouldThrowValidationException()
        {
            // Arrange
            var currencyId = Guid.NewGuid();
            var command = new DeleteCurrencyCommand(currencyId);

            var currency = new Currency("usd", "US Dollar");
            typeof(AggregateRoot)
                .GetProperty(nameof(AggregateRoot.Id), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
                .SetValue(currency, currencyId);

            var repoMock = new Mock<ICurrenciesCommandsRepository>(MockBehavior.Strict);
            var validationPolicyMock = new Mock<IValidationPolicy<Currency>>(MockBehavior.Strict);

            repoMock
                .Setup(r => r.GetCurrencyById(currencyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(currency);

            var invalid = new ValidationResult();
            invalid.AddValidationError(new ValidationError
            {
                Entity = nameof(Currency),
                Name = nameof(Currency.Code),
                Message = "Invalid code"
            });

            validationPolicyMock
                .Setup(v => v.Validate(currency))
                .ReturnsAsync(invalid);

            var handler = new DeleteCurrencyCommandHandler(
                repoMock.Object,
                validationPolicyMock.Object,
                new DeleteCurrencyCommandFlowDescribtor());

            // Act & Assert
            await Should.ThrowAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));

            repoMock.Verify(r => r.Update(It.IsAny<Currency>()), Times.Never);
            repoMock.Verify(r => r.WriteHistory(It.IsAny<CurrenciesHistory>()), Times.Never);
            repoMock.Verify(r => r.SaveChanges(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
