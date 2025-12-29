using Moq;
using ProductCatalog.Application.Features.Currencies.Commands.CreateCurrency;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.History;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using Shouldly;

namespace ProductsCatalog.Application.UnitTests.Features.Currencies.Commands
{
    public class CreateCurrencyCommandHandlerTests
    {
        static CreateCurrencyCommandHandlerTests()
        {
            MappingConfig.RegisterMappings();
        }

        [Fact]
        public async Task Handle_ShouldInvokeDependenciesInOrderAndReturnDto()
        {
            // Arrange
            var command = new CreateCurrencyCommand(
                new CreateCurrencyExternalDto("usd", "US Dollar"));

            var repoMock = new Mock<ICurrenciesCommandsRepository>(MockBehavior.Strict);
            var validationPolicyMock = new Mock<IValidationPolicy<Currency>>(MockBehavior.Strict);
            var sequence = new MockSequence();

            var validationResult = new ValidationResult();

            validationPolicyMock.InSequence(sequence)
                .Setup(v => v.Validate(It.IsAny<Currency>()))
                .ReturnsAsync(validationResult);

            repoMock.InSequence(sequence)
                .Setup(r => r.Add(It.IsAny<Currency>()));

            repoMock.InSequence(sequence)
                .Setup(r => r.WriteHistory(It.IsAny<CurrenciesHistory>()));

            repoMock.InSequence(sequence)
                .Setup(r => r.SaveChanges(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new CreateCurrencyCommandHandler(
                repoMock.Object,
                validationPolicyMock.Object,
                new CreateCurrencyCommandFlowDescribtor());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            validationPolicyMock.Verify(v => v.Validate(It.IsAny<Currency>()), Times.Once);

            repoMock.Verify(r => r.Add(It.IsAny<Currency>()), Times.Once);

            repoMock.Verify(r => r.WriteHistory(It.IsAny<CurrenciesHistory>()), Times.Once);

            repoMock.Verify(r => r.SaveChanges(It.IsAny<CancellationToken>()), Times.Once);

            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(Guid.Empty);
            result.Code.ShouldBe(command.currency.Code);
            result.Description.ShouldBe(command.currency.Description);
            result.IsActive.ShouldBeTrue();
        }

        [Fact]
        public async Task Handle_WhenValidationFails_ShouldThrowValidationException()
        {
            // Arrange
            var command = new CreateCurrencyCommand(
                new CreateCurrencyExternalDto("usd", "US Dollar"));

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

            var handler = new CreateCurrencyCommandHandler(
                repoMock.Object,
                validationPolicyMock.Object,
                new CreateCurrencyCommandFlowDescribtor());

            // Act & Assert
            await Should.ThrowAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));

            repoMock.Verify(r => r.Add(It.IsAny<Currency>()), Times.Never);
            repoMock.Verify(r => r.WriteHistory(It.IsAny<CurrenciesHistory>()), Times.Never);
            repoMock.Verify(r => r.SaveChanges(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}