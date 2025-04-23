using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts
{
    public class UpdateCartHandlerTests
    {
        private readonly Mock<ICartRepository> _cartRepositoryMock = new();

        [Fact]
        public async Task Handle_ShouldUpdateCart_WhenValid()
        {
            // Arrange
            var cartId = 1;
            var existingCart = new Cart
            {
                Id = cartId,
                UserId = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Items = new List<CartItem> { new() { ProductId = 1, Quantity = 2 } }
            };

            var command = new UpdateCartCommand
            {
                Id = cartId,
                UserId = Guid.NewGuid(),
                Date = DateTime.UtcNow.AddDays(1),
                Items = new List<UpdateCartItemDto>
                {
                    new() { ProductId = 2, Quantity = 3 }
                }
            };

            _cartRepositoryMock.Setup(r => r.GetByIdAsync(cartId, default)).ReturnsAsync(existingCart);
            _cartRepositoryMock.Setup(r => r.UpdateAsync(existingCart, default)).Returns(Task.CompletedTask);

            var handler = new UpdateCartHandler(_cartRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().BeTrue();
            existingCart.Items.Should().ContainSingle(i => i.ProductId == 2 && i.Quantity == 3);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenCartNotFound()
        {
            // Arrange
            var command = new UpdateCartCommand
            {
                Id = 999,
                UserId = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Items = new List<UpdateCartItemDto>()
            };

            _cartRepositoryMock.Setup(r => r.GetByIdAsync(command.Id, default)).ReturnsAsync((Cart?)null);

            var handler = new UpdateCartHandler(_cartRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_ShouldUpdateAllFieldsProperly()
        {
            // Arrange
            var cartId = 3;
            var originalDate = DateTime.UtcNow;
            var updatedDate = originalDate.AddHours(3);

            var existingCart = new Cart
            {
                Id = cartId,
                UserId = Guid.NewGuid(),
                Date = originalDate,
                Items = new List<CartItem>()
            };

            var newUserId = Guid.NewGuid();

            var command = new UpdateCartCommand
            {
                Id = cartId,
                UserId = newUserId,
                Date = updatedDate,
                Items = new List<UpdateCartItemDto>()
            };

            _cartRepositoryMock.Setup(r => r.GetByIdAsync(cartId, default)).ReturnsAsync(existingCart);

            var handler = new UpdateCartHandler(_cartRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().BeTrue();
            existingCart.UserId.Should().Be(newUserId);
            existingCart.Date.Should().Be(updatedDate);
        }

        [Fact]
        public async Task Handle_ShouldPreserveCartId()
        {
            // Arrange
            var cartId = 4;
            var existingCart = new Cart
            {
                Id = cartId,
                UserId = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Items = new List<CartItem>()
            };

            var command = new UpdateCartCommand
            {
                Id = cartId,
                UserId = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Items = new List<UpdateCartItemDto>
                {
                    new() { ProductId = 5, Quantity = 1 }
                }
            };

            _cartRepositoryMock.Setup(r => r.GetByIdAsync(cartId, default)).ReturnsAsync(existingCart);

            var handler = new UpdateCartHandler(_cartRepositoryMock.Object);

            // Act
            await handler.Handle(command, default);

            // Assert
            existingCart.Id.Should().Be(cartId);
        }
    }
}