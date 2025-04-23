using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using AutoMapper;
using FluentAssertions;
using Moq;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Products.GetProducts;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products
{
    public class GetProductByIdHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock = new();
        private readonly IMapper _mapper;

        public GetProductByIdHandlerTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDto>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Handle_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Title = "Energy Drink",
                Price = 10.5m,
                Description = "Boost your day",
                Category = "Drinks",
                Image = "energy.jpg",
                Rate = 4.7,
                Count = 300
            };

            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(product.Id, default))
                .ReturnsAsync(product);

            var handler = new GetProductByIdHandler(_productRepositoryMock.Object, _mapper);

            // Act
            var result = await handler.Handle(new GetProductByIdQuery(product.Id), default);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            result.Title.Should().Be("Energy Drink");
            result.Price.Should().Be(10.5m);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>(), default))
                .ReturnsAsync((Product?)null);

            var handler = new GetProductByIdHandler(_productRepositoryMock.Object, _mapper);

            // Act
            var result = await handler.Handle(new GetProductByIdQuery(999), default);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldMapCorrectly()
        {
            // Arrange
            var product = new Product
            {
                Id = 2,
                Title = "Snack",
                Price = 5.75m,
                Description = "Crunchy snack",
                Category = "Food",
                Image = "snack.jpg",
                Rate = 4.1,
                Count = 87
            };

            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(product.Id, default))
                .ReturnsAsync(product);

            var handler = new GetProductByIdHandler(_productRepositoryMock.Object, _mapper);

            // Act
            var result = await handler.Handle(new GetProductByIdQuery(product.Id), default);

            // Assert
            result.Should().NotBeNull();
            result!.Category.Should().Be("Food");
            result.Image.Should().Be("snack.jpg");
            result.Rate.Should().Be(4.1);
            result.Count.Should().Be(87);
        }

        [Fact]
        public async Task Handle_ShouldCallRepositoryOnce()
        {
            // Arrange
            var handler = new GetProductByIdHandler(_productRepositoryMock.Object, _mapper);

            // Act
            await handler.Handle(new GetProductByIdQuery(3), default);

            // Assert
            _productRepositoryMock.Verify(r => r.GetByIdAsync(3, default), Times.Once);
        }
    }
}
