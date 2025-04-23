using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products
{
    public class DeleteProductHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock = new();

        [Fact]
        public async Task Handle_ShouldDeleteProduct_WhenProductExists()
        {
            // Arrange
            const int productId = 1;
            _productRepositoryMock.Setup(r => r.DeleteByIdAsync(productId, default))
                                   .ReturnsAsync(true);

            var handler = new DeleteProductHandler(_productRepositoryMock.Object);

            // Act
            var result = await handler.Handle(new DeleteProductCommand(productId), default);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            // Arrange
            const int productId = 99;
            _productRepositoryMock.Setup(r => r.DeleteByIdAsync(productId, default))
                                   .ReturnsAsync(false);

            var handler = new DeleteProductHandler(_productRepositoryMock.Object);

            // Act
            var result = await handler.Handle(new DeleteProductCommand(productId), default);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_ShouldCallRepository_WithCorrectProductId()
        {
            // Arrange
            const int productId = 123;
            var called = false;

            _productRepositoryMock.Setup(r => r.DeleteByIdAsync(productId, default))
                                   .Callback(() => called = true)
                                   .ReturnsAsync(true);

            var handler = new DeleteProductHandler(_productRepositoryMock.Object);

            // Act
            await handler.Handle(new DeleteProductCommand(productId), default);

            // Assert
            called.Should().BeTrue();
        }
    }
}
