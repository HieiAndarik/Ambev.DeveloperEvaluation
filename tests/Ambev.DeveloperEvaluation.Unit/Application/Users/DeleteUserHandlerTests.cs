using Ambev.DeveloperEvaluation.Application.Users;
using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users
{
    public class DeleteUserHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly DeleteUserHandler _handler;

        public DeleteUserHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new DeleteUserHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteUser_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepositoryMock.Setup(r => r.DeleteByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var command = new DeleteUserCommand(userId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _userRepositoryMock.Verify(r => r.DeleteByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepositoryMock.Setup(r => r.DeleteByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var command = new DeleteUserCommand(userId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenRepositoryThrows()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepositoryMock.Setup(r => r.DeleteByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            var command = new DeleteUserCommand(userId);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
        }

        [Fact]
        public async Task Handle_ShouldCallRepository_WithCorrectUserId()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var command = new DeleteUserCommand(userId);

            _userRepositoryMock.Setup(r => r.DeleteByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _userRepositoryMock.Verify(r => r.DeleteByIdAsync(It.Is<Guid>(id => id == userId), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
