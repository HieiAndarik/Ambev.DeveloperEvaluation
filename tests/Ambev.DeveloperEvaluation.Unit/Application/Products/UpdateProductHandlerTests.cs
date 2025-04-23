using System;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

public class UpdateProductHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock = new();

    [Fact]
    public async Task Handle_ShouldUpdateProduct_WhenProductExists()
    {
        // Arrange
        var command = new UpdateProductCommand
        {
            Id = 1,
            Title = "Updated",
            Description = "Updated Desc",
            Price = 11.99m,
            Category = "Updated Cat",
            Image = "updated.jpg",
            Rate = 4.8,
            Count = 88
        };

        var existing = new Product
        {
            Id = 1,
            Title = "Old",
            Description = "Old Desc",
            Price = 9.99m,
            Category = "Old Cat",
            Image = "old.jpg",
            Rate = 4.0,
            Count = 10
        };

        _productRepositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        _productRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()));

        var handler = new UpdateProductHandler(_productRepositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        existing.Title.Should().Be("Updated");
        existing.Price.Should().Be(11.99m);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenProductDoesNotExist()
    {
        // Arrange
        var command = new UpdateProductCommand { Id = 999 };

        _productRepositoryMock.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        var handler = new UpdateProductHandler(_productRepositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldUpdateAllFields()
    {
        // Arrange
        var command = new UpdateProductCommand
        {
            Id = 3,
            Title = "Title",
            Description = "Desc",
            Price = 30,
            Category = "Cat",
            Image = "img.jpg",
            Rate = 3.5,
            Count = 33
        };

        var existing = new Product { Id = 3 };

        _productRepositoryMock.Setup(r => r.GetByIdAsync(3, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        var handler = new UpdateProductHandler(_productRepositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        existing.Title.Should().Be("Title");
        existing.Description.Should().Be("Desc");
        existing.Price.Should().Be(30);
        existing.Category.Should().Be("Cat");
        existing.Image.Should().Be("img.jpg");
        existing.Rate.Should().Be(3.5);
        existing.Count.Should().Be(33);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryUpdate()
    {
        // Arrange
        var product = new Product { Id = 2 };
        var command = new UpdateProductCommand { Id = 2, Title = "Check" };

        _productRepositoryMock.Setup(r => r.GetByIdAsync(2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var handler = new UpdateProductHandler(_productRepositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        _productRepositoryMock.Verify(r => r.UpdateAsync(product, It.IsAny<CancellationToken>()), Times.Once);
    }
}
