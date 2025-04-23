//using Ambev.DeveloperEvaluation.Domain.Entities;
//using Ambev.DeveloperEvaluation.Domain.Interfaces;
//using AutoMapper;
//using FluentAssertions;
//using Moq;
//using Xunit;
//using System;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Ambev.DeveloperEvaluation.Unit.Application.Carts
//{
//    public class CreateCartHandlerTests
//    {
//        private readonly Mock<ICartRepository> _cartRepositoryMock = new();
//        private readonly IMapper _mapper;

//        public CreateCartHandlerTests()
//        {
//            var mapperConfig = new MapperConfiguration(cfg =>
//            {
//                cfg.CreateMap<CreateCartCommand, Cart>();
//                cfg.CreateMap<Cart, CreateCartResult>();
//            });
//            _mapper = mapperConfig.CreateMapper();
//        }

//        [Fact]
//        public async Task Handle_ShouldCreateCartSuccessfully()
//        {
//            // Arrange
//            var customerId = Guid.NewGuid();

//            var command = new CreateCartCommand { CustomerId = customerId };

//            var expectedCart = new Cart { Id = Guid.NewGuid(), CustomerId = customerId };

//            _cartRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
//                .ReturnsAsync(expectedCart);

//            var handler = new CreateCartHandler(_cartRepositoryMock.Object, _mapper);

//            // Act
//            var result = await handler.Handle(command, CancellationToken.None);

//            // Assert
//            result.Should().NotBeNull();
//            result.Id.Should().Be(expectedCart.Id);
//            result.CustomerId.Should().Be(customerId);
//        }

//        [Fact]
//        public async Task Handle_ShouldThrow_WhenRepositoryReturnsNull()
//        {
//            // Arrange
//            var command = new CreateCartCommand { CustomerId = Guid.NewGuid() };

//            _cartRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
//                .ReturnsAsync((Cart?)null);

//            var handler = new CreateCartHandler(_cartRepositoryMock.Object, _mapper);

//            // Act
//            var result = await handler.Handle(command, CancellationToken.None);

//            // Assert
//            result.Should().BeNull();
//        }

//        [Fact]
//        public async Task Handle_ShouldPassCorrectCartToRepository()
//        {
//            // Arrange
//            var customerId = Guid.NewGuid();
//            Cart? capturedCart = null;

//            var command = new CreateCartCommand { CustomerId = customerId };

//            _cartRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
//                .Callback<Cart, CancellationToken>((cart, _) => capturedCart = cart)
//                .ReturnsAsync(new Cart { Id = Guid.NewGuid(), CustomerId = customerId });

//            var handler = new CreateCartHandler(_cartRepositoryMock.Object, _mapper);

//            // Act
//            await handler.Handle(command, CancellationToken.None);

//            // Assert
//            capturedCart.Should().NotBeNull();
//            capturedCart!.CustomerId.Should().Be(customerId);
//        }

//        [Fact]
//        public async Task Handle_ReturnsCartsCorrectly()
//        {
//            // Arrange
//            var carts = new List<Cart>
//        {
//            new() { Id = 1, UserId = 101, Date = DateTime.UtcNow },
//            new() { Id = 2, UserId = 102, Date = DateTime.UtcNow }
//        };

//            var repoMock = new Mock<ICartRepository>();
//            repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(carts);

//            var mapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<Cart, CartDto>());
//            var mapper = mapperConfig.CreateMapper();

//            var handler = new GetCartsHandler(repoMock.Object, mapper);

//            // Act
//            var result = await handler.Handle(new GetCartsQuery(), default);

//            // Assert
//            Assert.Equal(2, result.Carts.Count());
//            Assert.Equal(2, result.TotalCount);
//        }
//    }
//}
