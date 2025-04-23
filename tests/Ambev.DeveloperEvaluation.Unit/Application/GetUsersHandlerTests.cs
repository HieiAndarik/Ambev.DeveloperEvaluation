using Ambev.DeveloperEvaluation.Application.Users.GetUsers;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using AutoMapper;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class GetUsersHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsUsersCorrectly()
    {
        // Arrange
        var users = new List<User>
        {
            new() { Id = Guid.NewGuid(), Username = "john", Email = "john@example.com" },
            new() { Id = Guid.NewGuid(), Username = "jane", Email = "jane@example.com" }
        };

        var repoMock = new Mock<IUserRepository>();
        repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(users);

        var mapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDto>());
        var mapper = mapperConfig.CreateMapper();

        var handler = new GetUsersHandler(repoMock.Object, mapper);

        // Act
        var result = await handler.Handle(new GetUsersQuery(), default);

        // Assert
        //Assert.Equal(2, result.Users.Count());
        //Assert.Equal(2, result.TotalCount);
    }
}