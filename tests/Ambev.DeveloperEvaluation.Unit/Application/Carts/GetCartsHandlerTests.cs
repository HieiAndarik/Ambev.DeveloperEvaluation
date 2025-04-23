using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using AutoMapper;
using FluentAssertions;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts
{
    public class GetCartsHandlerTests
    {
        private readonly Mock<ICartRepository> _cartRepositoryMock = new();
        private readonly IMapper _mapper;

        public GetCartsHandlerTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CartItem, CartItemDto>();
                cfg.CreateMap<Cart, CartDto>();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Handle_ShouldReturnPagedCarts_WithCorrectData()
        {
            // Arrange
            var carts = new List<Cart>
            {
                new()
                {
                    Id = 1,
                    UserId = Guid.NewGuid(),
                    Date = DateTime.UtcNow,
                    Items = new List<CartItem>
                    {
                        new() { Id = 1, ProductId = 100, Quantity = 2 },
                        new() { Id = 2, ProductId = 101, Quantity = 3 }
                    }
                },
                new()
                {
                    Id = 2,
                    UserId = Guid.NewGuid(),
                    Date = DateTime.UtcNow,
                    Items = new List<CartItem>
                    {
                        new() { Id = 3, ProductId = 102, Quantity = 1 }
                    }
                }
            };

            _cartRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(carts);

            var handler = new GetCartsHandler(_cartRepositoryMock.Object, _mapper);
            var query = new GetCartsQuery();

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.Items.First().Id.Should().Be(1);
            result.Items.Last().Id.Should().Be(2);
            result.TotalItems.Should().Be(2);
        }

        [Fact]
        public async Task Handle_ShouldReturnCart_WhenExists()
        {
            var cart = new Cart { Id = 1, UserId = Guid.NewGuid(), Date = DateTime.UtcNow };
            _cartRepositoryMock.Setup(r => r.GetByIdAsync(cart.Id, default)).ReturnsAsync(cart);

            var handler = new GetCartByIdHandler(_cartRepositoryMock.Object, _mapper);
            var result = await handler.Handle(new GetCartByIdQuery(cart.Id), default);

            result.Should().NotBeNull();
            result.Id.Should().Be(cart.Id);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenCartDoesNotExist()
        {
            _cartRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), default))
                .ReturnsAsync((Cart?)null);

            var handler = new GetCartByIdHandler(_cartRepositoryMock.Object, _mapper);
            var result = await handler.Handle(new GetCartByIdQuery(999), default);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldMapCartItemsCorrectly()
        {
            var cart = new Cart
            {
                Id = 1,
                UserId = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Items = new List<CartItem>
                {
                    new() { Id = 1, ProductId = 100, Quantity = 3 },
                    new() { Id = 2, ProductId = 200, Quantity = 1 }
                }
            };

            _cartRepositoryMock.Setup(r => r.GetByIdAsync(cart.Id, default)).ReturnsAsync(cart);

            var handler = new GetCartByIdHandler(_cartRepositoryMock.Object, _mapper);
            var result = await handler.Handle(new GetCartByIdQuery(cart.Id), default);

            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.Items.First().ProductId.Should().Be(100);
        }

    }
}
