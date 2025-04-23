using Ambev.DeveloperEvaluation.Application.Users.GetUsers;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.WebApi.Mappings;
using AutoMapper;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users
{
    public class GetUsersHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly IMapper _mapper;

        public GetUsersHandlerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(GetUsersHandler).Assembly);
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task Handle_ShouldReturnPagedUsers_WithDefaultOrder()
        {
            // Arrange
            var users = new List<User>
            {
                new() { Id = Guid.NewGuid(), FirstName = "Arthur", Email = "arthur@dicathen.com" }
            };

            _userRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(users);

            var query = new GetUsersQuery(1, 10, "email");

            var handler = new GetUsersHandler(_userRepositoryMock.Object, _mapper);

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().HaveCount(1);
            result.TotalItems.Should().Be(1);
        }

        [Fact]
        public async Task Handle_ShouldReturnUserDto_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                FirstName = "Arthur",
                LastName = "Leywin",
                Email = "arthur@dicathen.com",
                Phone = "999999999",
                Role = UserRole.Customer,
                Status = UserStatus.Active
            };

            var query = new GetUserByIdQuery(userId);

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.GetByIdAsync(userId, default))
                .ReturnsAsync(user);

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(GetUsersHandler).Assembly);
            });

            var mapper = mapperConfig.CreateMapper();

            var handler = new GetUserByIdHandler(userRepositoryMock.Object, mapper);

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(userId);
            result.Email.Should().Be("arthur@dicathen.com");
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var query = new GetUserByIdQuery(userId);

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.GetByIdAsync(userId, default))
                .ReturnsAsync((User?)null);

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(GetUsersHandler).Assembly);
            });

            var mapper = mapperConfig.CreateMapper();

            var handler = new GetUserByIdHandler(userRepositoryMock.Object, mapper);

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Should().BeNull();
        }
    }
}
