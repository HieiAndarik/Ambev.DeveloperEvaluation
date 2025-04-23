using Ambev.DeveloperEvaluation.Application.Users;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users
{
    public class UpdateUserHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new();

        [Fact]
        public async Task Handle_ShouldUpdateUser_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var existingUser = new User
            {
                Id = userId,
                FirstName = "Old",
                LastName = "Name",
                Username = "olduser",
                Email = "old@email.com",
                Phone = "123456789",
                Password = "oldpass",
                Role = UserRole.Customer,
                Status = UserStatus.Active
            };

            var command = new UpdateUserCommand(
                userId,
                "arthur",
                "leywin",
                "arthurleywin",
                "new@email.com",
                "newpassword",
                UserRole.Admin,
                "999999999",
                UserStatus.Inactive
            );

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, default)).ReturnsAsync(existingUser);
            _userRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<User>())).ReturnsAsync(true);

            var handler = new UpdateUserHandler(_userRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().BeTrue();
            existingUser.Username.Should().Be("arthurleywin");
            existingUser.Email.Should().Be("new@email.com");
            existingUser.Password.Should().Be("newpassword");
            existingUser.Role.Should().Be(UserRole.Admin);
            existingUser.Phone.Should().Be("999999999");
            existingUser.Status.Should().Be(UserStatus.Inactive);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, default)).ReturnsAsync((User?)null);

            var command = new UpdateUserCommand(
                userId,
                "arthur",
                "leywin",
                "arthurleywin",
                "new@email.com",
                "newpassword",
                UserRole.Admin,
                "999999999",
                UserStatus.Inactive
            );

            var handler = new UpdateUserHandler(_userRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_ShouldNotChangeUser_WhenDataIsTheSame()
        {
            var userId = Guid.NewGuid();
            var existingUser = new User
            {
                Id = userId,
                Username = "SameUser",
                Email = "same@email.com",
                Password = "123456",
                Phone = "999999999",
                Role = UserRole.Customer,
                Status = UserStatus.Active
            };

            var command = new UpdateUserCommand(
                userId,
                "arthur",
                "leywin",
                "arthurleywin",
                "new@email.com",
                "newpassword",
                UserRole.Admin,
                "999999999",
                UserStatus.Active
            );

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.GetByIdAsync(userId, default)).ReturnsAsync(existingUser);
            userRepositoryMock.Setup(r => r.UpdateAsync(existingUser)).ReturnsAsync(true);

            var handler = new UpdateUserHandler(userRepositoryMock.Object);

            var result = await handler.Handle(command, default);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenUpdateFails()
        {
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };

            var command = new UpdateUserCommand(
                userId,
                "arthur",
                "leywin",
                "arthurleywin",
                "new@email.com",
                "newpassword",
                UserRole.Admin,
                "999999999",
                UserStatus.Inactive
            );

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.GetByIdAsync(userId, default)).ReturnsAsync(user);
            userRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<User>())).ReturnsAsync(false);

            var handler = new UpdateUserHandler(userRepositoryMock.Object);

            var result = await handler.Handle(command, default);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_ShouldUpdateAllFieldsCorrectly()
        {
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Username = "OldUser",
                Email = "old@email.com",
                Password = "OldPass",
                Role = UserRole.Customer,
                Phone = "123456789",
                Status = UserStatus.Inactive
            };

            var command = new UpdateUserCommand(
                userId,
                "arthur",
                "leywin",
                "arthurleywin",
                "new@email.com",
                "newpassword",
                UserRole.Admin,
                "999999999",
                UserStatus.Active
            );

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.GetByIdAsync(userId, default)).ReturnsAsync(user);
            userRepositoryMock.Setup(r => r.UpdateAsync(user)).ReturnsAsync(true);

            var handler = new UpdateUserHandler(userRepositoryMock.Object);
            await handler.Handle(command, default);

            user.Username.Should().Be("arthurleywin");
            user.Email.Should().Be("new@email.com");
            user.Password.Should().Be("newpassword");
            user.Role.Should().Be(UserRole.Admin);
            user.Phone.Should().Be("999999999");
            user.Status.Should().Be(UserStatus.Active);
        }

    }
}
