using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUsers;

public sealed record GetUserByIdQuery(Guid Id) : IRequest<UserDto?>;
