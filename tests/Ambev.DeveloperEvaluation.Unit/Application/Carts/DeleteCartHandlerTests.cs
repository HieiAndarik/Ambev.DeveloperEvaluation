using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts
{
    public class DeleteCartHandlerTests
    {
        private readonly Mock<ICartRepository> _cartRepositoryMock = new();

        [Fact]
        public async Task Handle_ShouldDeleteCart_WhenExists()
        {
            // Arrange
            _cartRepositoryMock.Setup(r => r.DeleteByIdAsync(1, default)).ReturnsAsync(true);
            var handler = new DeleteCartHandler(_cartRepositoryMock.Object);
            var command = new DeleteCartCommand(1);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenNotExists()
        {
            _cartRepositoryMock.Setup(r => r.DeleteByIdAsync(99, default)).ReturnsAsync(false);
            var handler = new DeleteCartHandler(_cartRepositoryMock.Object);
            var command = new DeleteCartCommand(99);

            var result = await handler.Handle(command, default);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenCartIsDeleted()
        {
            // Arrange
            var cartId = 1;
            _cartRepositoryMock.Setup(r => r.DeleteByIdAsync(cartId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var handler = new DeleteCartHandler(_cartRepositoryMock.Object);
            var command = new DeleteCartCommand(cartId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenCartDoesNotExist()
        {
            // Arrange
            var cartId = 999;
            _cartRepositoryMock.Setup(r => r.DeleteByIdAsync(cartId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var handler = new DeleteCartHandler(_cartRepositoryMock.Object);
            var command = new DeleteCartCommand(cartId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_ShouldCallRepositoryOnce()
        {
            // Arrange
            var cartId = 1;
            var command = new DeleteCartCommand(cartId);
            var handler = new DeleteCartHandler(_cartRepositoryMock.Object);

            _cartRepositoryMock.Setup(r => r.DeleteByIdAsync(cartId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            _cartRepositoryMock.Verify(r => r.DeleteByIdAsync(cartId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}