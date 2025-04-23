using Ambev.DeveloperEvaluation.Application.Products.GetProducts;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using AutoMapper;
using FluentAssertions;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

public class GetProductsHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock = new();
    private readonly IMapper _mapper;

    public GetProductsHandlerTests()
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Product, ProductDto>();
        });
        _mapper = mapperConfig.CreateMapper();
    }

    [Fact]
    public async Task Handle_ShouldReturnPagedProducts_WhenValidQuery()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = 1, Title = "Product A", Price = 10, Description = "Desc A", Category = "Cat A", Image = "img1", Rate = 4.5, Count = 10 },
            new() { Id = 2, Title = "Product B", Price = 20, Description = "Desc B", Category = "Cat B", Image = "img2", Rate = 3.8, Count = 8 },
            new() { Id = 3, Title = "Another", Price = 15, Description = "Another Desc", Category = "Cat C", Image = "img3", Rate = 5.0, Count = 12 },
        };

        _productRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        var handler = new GetProductsHandler(_productRepositoryMock.Object, _mapper);
        var query = new GetProductsQuery(Page: 1, Size: 2, Order: "title");

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().HaveCount(2);
        result.TotalItems.Should().Be(3);
        result.CurrentPage.Should().Be(1);
        result.TotalPages.Should().Be(2);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoProductsFound()
    {
        // Arrange
        _productRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Product>());

        var handler = new GetProductsHandler(_productRepositoryMock.Object, _mapper);
        var query = new GetProductsQuery(1, 10, "title");

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().BeEmpty();
        result.TotalItems.Should().Be(0);
        result.CurrentPage.Should().Be(1);
        result.TotalPages.Should().Be(0);
    }

    [Fact]
    public async Task Handle_ShouldApplySortingCorrectly()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = 2, Title = "Zeta" },
            new() { Id = 3, Title = "Alpha" },
            new() { Id = 1, Title = "Gamma" },
        };

        _productRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        var handler = new GetProductsHandler(_productRepositoryMock.Object, _mapper);
        var query = new GetProductsQuery(Page: 1, Size: 3, Order: "title");

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.Data.Should().HaveCount(3);
        result.Data.First().Title.Should().Be("Alpha");
        result.Data.Last().Title.Should().Be("Zeta");
    }
}
