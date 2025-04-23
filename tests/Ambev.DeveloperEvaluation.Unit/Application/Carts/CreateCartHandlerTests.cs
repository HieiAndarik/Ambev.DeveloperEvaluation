using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts
{
    public class CreateCartHandlerTests
    {
        private readonly Mock<ICartRepository> _cartRepositoryMock = new();

        [Fact]
        public async Task Handle_ShouldCreateCart_WhenValid()
        {
            // Arrange
            var command = new CreateCartCommand
            {
                UserId = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Items = new List<CreateCartItemDto>
                {
                    new() { ProductId = 1, Quantity = 2 }
                }
            };

            _cartRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
                .Callback<Cart, CancellationToken>((cart, _) => cart.Id = 1)
                .Returns(Task.CompletedTask);


            var handler = new CreateCartHandler(_cartRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Handle_ShouldAssignCorrectUserIdAndDate()
        {
            // Arrange
            var command = new CreateCartCommand
            {
                UserId = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Items = new List<CreateCartItemDto>()
            };

            Cart? createdCart = null;
            _cartRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
                .Callback<Cart, CancellationToken>((cart, _) => createdCart = cart)
                .Returns(Task.CompletedTask);

            var handler = new CreateCartHandler(_cartRepositoryMock.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            createdCart.Should().NotBeNull();
            createdCart!.UserId.Should().Be(command.UserId);
            createdCart.Date.Should().Be(command.Date);
        }

        [Fact]
        public async Task Handle_ShouldMapItemsCorrectly()
        {
            // Arrange
            var command = new CreateCartCommand
            {
                UserId = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Items = new List<CreateCartItemDto>
                {
                    new() { ProductId = 5, Quantity = 10 }
                }
            };

            Cart? createdCart = null;
            _cartRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
                .Callback<Cart, CancellationToken>((cart, _) => createdCart = cart)
                .Returns(Task.CompletedTask);

            var handler = new CreateCartHandler(_cartRepositoryMock.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            createdCart.Should().NotBeNull();
            createdCart!.Items.Should().HaveCount(1);
            createdCart.Items[0].ProductId.Should().Be(5);
            createdCart.Items[0].Quantity.Should().Be(10);
        }

        [Fact]
        public async Task Handle_ShouldThrowIfRepositoryFailsSilently()
        {
            // Arrange
            var command = new CreateCartCommand
            {
                UserId = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Items = new List<CreateCartItemDto>
                {
                    new() { ProductId = 1, Quantity = 1 }
                }
            };

            _cartRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("DB Error"));

            var handler = new CreateCartHandler(_cartRepositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}