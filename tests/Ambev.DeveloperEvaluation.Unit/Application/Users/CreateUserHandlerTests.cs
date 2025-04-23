using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Application.Users.GetUsers;
using Ambev.DeveloperEvaluation.Common.Security;
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
    public class CreateUserHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
        private readonly IMapper _mapper;

        public CreateUserHandlerTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(GetUsersHandler).Assembly);
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Handle_ShouldCreateUserSuccessfully()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Username = "arthurleywin",
                Email = "arthur@dicathen.com",
                Password = "StrongPassword123!",
                Phone = "999999999",
                Role = UserRole.Customer,
                Status = UserStatus.Active
            };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateUserCommand, User>()
                   .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()));
                cfg.CreateMap<User, CreateUserResult>();
            });

            var mapper = config.CreateMapper();

            var createdUser = mapper.Map<User>(command);

            _userRepositoryMock
                .Setup(r => r.GetByEmailAsync(command.Email, default))
                .ReturnsAsync((User?)null);

            _userRepositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<User>(), default))
                .ReturnsAsync(createdUser);

            _passwordHasherMock
                .Setup(h => h.HashPassword(command.Password))
                .Returns("hashed_password");

            var handler = new CreateUserHandler(
                _userRepositoryMock.Object,
                mapper,
                _passwordHasherMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().NotBe(Guid.Empty);
            result.Email.Should().Be(command.Email);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenEmailAlreadyExists()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Username = "arthurleywin",
                Email = "arthur@dicathen.com",
                Password = "StrongPassword123!",
                Phone = "999999999",
                Role = UserRole.Customer,
                Status = UserStatus.Active
            };

            var existingUser = new User { Id = Guid.NewGuid(), Email = command.Email };

            _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, default))
                .ReturnsAsync(existingUser);

            var handler = new CreateUserHandler(
                _userRepositoryMock.Object,
                _mapper,
                _passwordHasherMock.Object);

            // Act
            Func<Task> act = async () => await handler.Handle(command, default);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"User with email {command.Email} already exists");
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenEmailAlreadyExists()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Username = "arthurleywin",
                Email = "arthur@dicathen.com",
                Password = "StrongPassword123!",
                Phone = "999999999",
                Role = UserRole.Customer,
                Status = UserStatus.Active
            };

            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                Email = command.Email
            };

            _userRepositoryMock
                .Setup(r => r.GetByEmailAsync(command.Email, default))
                .ReturnsAsync(existingUser);

            var handler = new CreateUserHandler(
                _userRepositoryMock.Object,
                _mapper,
                _passwordHasherMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_ShouldHashPassword_Correctly()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Username = "arthurleywin",
                Email = "arthur@dicathen.com",
                Password = "StrongPassword123!",
                Phone = "999999999",
                Role = UserRole.Customer,
                Status = UserStatus.Active
            };

            _userRepositoryMock
                .Setup(r => r.GetByEmailAsync(command.Email, default))
                .ReturnsAsync((User?)null);

            _passwordHasherMock
                .Setup(h => h.HashPassword(command.Password))
                .Returns("hashed_password");

            _userRepositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<User>(), default))
                .ReturnsAsync((User u, CancellationToken _) => u);

            var handler = new CreateUserHandler(
                _userRepositoryMock.Object,
                _mapper,
                _passwordHasherMock.Object);

            // Act
            await handler.Handle(command, default);

            // Assert
            _passwordHasherMock.Verify(h => h.HashPassword("StrongPassword123!"), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldMapCommandToUserCorrectly()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Username = "arthurleywin",
                Email = "arthur@dicathen.com",
                Password = "StrongPassword123!",
                Phone = "999999999",
                Role = UserRole.Customer,
                Status = UserStatus.Active
            };

            _userRepositoryMock
                .Setup(r => r.GetByEmailAsync(command.Email, default))
                .ReturnsAsync((User?)null);

            _passwordHasherMock
                .Setup(h => h.HashPassword(It.IsAny<string>()))
                .Returns("hashed_password");

            User? capturedUser = null;

            _userRepositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<User>(), default))
                .Callback<User, CancellationToken>((u, _) => capturedUser = u)
                .ReturnsAsync((User u, CancellationToken _) => u);

            var handler = new CreateUserHandler(
                _userRepositoryMock.Object,
                _mapper,
                _passwordHasherMock.Object);

            // Act
            await handler.Handle(command, default);

            // Assert
            capturedUser.Should().NotBeNull();
            capturedUser!.Username.Should().Be(command.Username);
            capturedUser.Email.Should().Be(command.Email);
            capturedUser.Phone.Should().Be(command.Phone);
            capturedUser.Role.Should().Be(command.Role);
            capturedUser.Status.Should().Be(command.Status);
            capturedUser.Password.Should().Be("hashed_password");
        }

        [Fact]
        public async Task Handle_ShouldSetCorrectRoleAndStatus()
        {
            var command = new CreateUserCommand
            {
                Username = "arthur",
                Email = "arthur@dicathen.com",
                Password = "password123",
                Phone = "999999999",
                Role = UserRole.Admin,
                Status = UserStatus.Inactive
            };

            _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, default)).ReturnsAsync((User?)null);
            _passwordHasherMock.Setup(h => h.HashPassword(command.Password)).Returns("hashed_password");

            User? createdUser = null;
            _userRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<User>(), default))
                .Callback<User, CancellationToken>((u, _) => createdUser = u)
                .ReturnsAsync((User u, CancellationToken _) => u);

            var handler = new CreateUserHandler(
                _userRepositoryMock.Object,
                _mapper,
                _passwordHasherMock.Object);

            var result = await handler.Handle(command, default);

            createdUser.Should().NotBeNull();
            createdUser!.Role.Should().Be(UserRole.Admin);
            createdUser.Status.Should().Be(UserStatus.Inactive);
        }

        [Fact]
        public void Validate_ShouldFail_WhenEmailIsInvalid()
        {
            var validator = new CreateUserCommandValidator();
            var command = new CreateUserCommand
            {
                Username = "arthur",
                Email = "invalid-email",
                Password = "StrongPass123!",
                Phone = "999999999",
                Role = UserRole.Customer,
                Status = UserStatus.Active
            };

            var result = validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Email");
        }

        [Fact]
        public void Validate_ShouldFail_WhenPasswordIsTooWeak()
        {
            var validator = new CreateUserCommandValidator();
            var command = new CreateUserCommand
            {
                Username = "arthur",
                Email = "arthur@dicathen.com",
                Password = "123",
                Phone = "999999999",
                Role = UserRole.Customer,
                Status = UserStatus.Active
            };

            var result = validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Password");
        }

        [Fact]
        public async Task Handle_ShouldNotReturnPasswordInResult()
        {
            var command = new CreateUserCommand
            {
                Username = "arthur",
                Email = "arthur@dicathen.com",
                Password = "StrongPassword123!",
                Phone = "999999999",
                Role = UserRole.Customer,
                Status = UserStatus.Active
            };

            _userRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email, default))
                .ReturnsAsync((User?)null);
            _passwordHasherMock.Setup(h => h.HashPassword(command.Password)).Returns("hashed_password");
            _userRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<User>(), default))
                .ReturnsAsync((User u, CancellationToken _) => u);

            var handler = new CreateUserHandler(
                _userRepositoryMock.Object,
                _mapper,
                _passwordHasherMock.Object);

            var result = await handler.Handle(command, default);

            result.Should().NotBeNull();
            result.Should().BeOfType<CreateUserResult>();
            typeof(CreateUserResult).GetProperty("Password").Should().BeNull();
        }
    }
}
