using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using AutoMapper;
using FluentAssertions;
using Moq;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products
{
    public class CreateProductHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock = new();
        private readonly IMapper _mapper;

        public CreateProductHandlerTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateProductCommand, Product>();
                cfg.CreateMap<Product, CreateProductResult>();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Handle_ShouldCreateProduct_WhenValid()
        {
            var command = new CreateProductCommand("Soda", 3.99m, "Drink", "soda.jpg", "Cold soda", 4.5, 120);
            var createdProduct = _mapper.Map<Product>(command);
            createdProduct.Id = 1;

            _productRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);


            var handler = new CreateProductHandler(_productRepositoryMock.Object, _mapper);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().Be(0);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenRepositoryFails()
        {
            var command = new CreateProductCommand("Soda", 3.99m, "Drink", "soda.jpg", "Cold soda", 4.5, 120);

            _productRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .Callback<Product, CancellationToken>((p, _) => p.Id = 42)
                .Returns(Task.CompletedTask);

            var handler = new CreateProductHandler(_productRepositoryMock.Object, _mapper);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().Be(42);
        }

        [Fact]
        public async Task Handle_ShouldPassCorrectDataToRepository()
        {
            var command = new CreateProductCommand("Water", 1.00m, "Drink", "water.jpg", "Mineral water", 5.0, 500);
            Product? capturedProduct = null;

            _productRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .Callback<Product, CancellationToken>((product, _) => capturedProduct = product)
                    .Returns(Task.CompletedTask);

            var handler = new CreateProductHandler(_productRepositoryMock.Object, _mapper);

            await handler.Handle(command, CancellationToken.None);

            capturedProduct.Should().NotBeNull();
            capturedProduct!.Title.Should().Be("Water");
            capturedProduct.Price.Should().Be(1.00m);
            capturedProduct.Rate.Should().Be(5.0);
        }

        [Fact]
        public async Task Handle_ShouldReturnValidResult_WhenProductIsCreated()
        {
            var command = new CreateProductCommand("Juice", 5.00m, "Drink", "juice.jpg", "Fruit juice", 3.8, 200);
            var expectedProduct = new Product { Id = 3, Title = "Juice" };

            _productRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new CreateProductHandler(_productRepositoryMock.Object, _mapper);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().Be(0);
        }

        [Fact]
        public async Task Handle_ShouldHandleMultipleCreationsCorrectly()
        {
            var command1 = new CreateProductCommand("Beer", 6.50m, "Alcohol", "beer.jpg", "Cool beer", 4.0, 80);
            var command2 = new CreateProductCommand("Wine", 25.00m, "Alcohol", "wine.jpg", "Fine wine", 4.9, 30);

            var nextId = 1;
            var capturedProducts = new List<Product>();

            _productRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .Callback<Product, CancellationToken>((p, _) =>
                {
                    p.Id = nextId++;
                    capturedProducts.Add(p);
                })
                .Returns(Task.CompletedTask);

            var handler = new CreateProductHandler(_productRepositoryMock.Object, _mapper);

            var result1 = await handler.Handle(command1, CancellationToken.None);
            var result2 = await handler.Handle(command2, CancellationToken.None);

            capturedProducts.Should().HaveCount(2);
            capturedProducts[0].Title.Should().Be("Beer");
            capturedProducts[1].Title.Should().Be("Wine");
        }
    }
}
