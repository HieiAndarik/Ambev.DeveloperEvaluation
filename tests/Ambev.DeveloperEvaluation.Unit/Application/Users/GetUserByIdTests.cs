using Ambev.DeveloperEvaluation.Application.Users;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using FluentAssertions;
using Moq;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users
{
    public class GetUserByIdTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new();

        [Fact]
        public async Task GetUserById_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var expectedUser = new User
            {
                Id = userId,
                FirstName = "Arthur",
                LastName = "Leywin",
                Email = "arthur@dicathen.com"
            };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(expectedUser);

            // Act
            var result = await _userRepositoryMock.Object.GetByIdAsync(userId, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(userId);
            result.Email.Should().Be("arthur@dicathen.com");
        }

        [Fact]
        public async Task GetUserById_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                               .ReturnsAsync((User?)null);

            // Act
            var result = await _userRepositoryMock.Object.GetByIdAsync(userId);

            // Assert
            result.Should().BeNull();
        }

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
                Email = "old@email.com"
            };

            var command = new UpdateUserCommand(
                userId,
                "New First Name",
                "New Last Name",
                "newusername",
                "new@email.com",
                "newpassword",
                UserRole.Customer,
                "999999999",
                UserStatus.Active
                );

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, default)).ReturnsAsync(existingUser);
            _userRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<User>())).ReturnsAsync(true);

            var handler = new UpdateUserHandler(_userRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().BeTrue();
            existingUser.FirstName.Should().Be("New First Name");
            existingUser.Email.Should().Be("new@email.com");
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new UpdateUserCommand(
                userId,
                "New First Name",
                "New Last Name",
                "newusername",
                "new@email.com",
                "newpassword",
                UserRole.Customer,
                "999999999",
                UserStatus.Active
                );

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, default)).ReturnsAsync((User?)null);

            var handler = new UpdateUserHandler(_userRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().BeFalse();
        }
    }
}