using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users
{
    public record UpdateUserCommand(
        Guid Id,
        string Firstname,
        string Lastname,
        string Username,
        string Email,
        string Password,
        UserRole Role,
        string Phone,
        UserStatus Status
    ) : IRequest<bool>;
}
