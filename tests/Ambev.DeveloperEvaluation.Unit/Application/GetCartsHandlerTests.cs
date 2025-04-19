using Ambev.DeveloperEvaluation.Application.Carts.GetCarts;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using AutoMapper;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class GetCartsHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsCartsCorrectly()
    {
        // Arrange
        var carts = new List<Cart>
        {
            new() { Id = 1, UserId = 101, Date = DateTime.UtcNow },
            new() { Id = 2, UserId = 102, Date = DateTime.UtcNow }
        };

        var repoMock = new Mock<ICartRepository>();
        repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(carts);

        var mapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<Cart, CartDto>());
        var mapper = mapperConfig.CreateMapper();

        var handler = new GetCartsHandler(repoMock.Object, mapper);

        // Act
        var result = await handler.Handle(new GetCartsQuery(), default);

        // Assert
        Assert.Equal(2, result.Carts.Count());
        Assert.Equal(2, result.TotalCount);
    }
}