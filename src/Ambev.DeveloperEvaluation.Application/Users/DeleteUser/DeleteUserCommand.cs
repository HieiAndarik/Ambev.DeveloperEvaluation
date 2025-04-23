using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.DeleteUser;

public sealed record DeleteUserCommand(Guid Id) : IRequest<bool>;
