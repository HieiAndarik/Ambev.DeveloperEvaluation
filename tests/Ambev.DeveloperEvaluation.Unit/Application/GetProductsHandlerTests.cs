using Ambev.DeveloperEvaluation.Application.Products.GetProducts;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using AutoMapper;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class GetProductsHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsProductsCorrectly()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = 1, Title = "Product A", Price = 10, Description = "Test A" },
            new() { Id = 2, Title = "Product B", Price = 20, Description = "Test B" }
        };

        var repoMock = new Mock<IProductRepository>();
        repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(products);

        var mapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<Product, ProductDto>());
        var mapper = mapperConfig.CreateMapper();

        var handler = new GetProductsHandler(repoMock.Object, mapper);

        // Act
        var result = await handler.Handle(new GetProductsQuery(), default);

        // Assert
        Assert.Equal(2, result.Products.Count());
        Assert.Equal(2, result.TotalCount);
    }
}