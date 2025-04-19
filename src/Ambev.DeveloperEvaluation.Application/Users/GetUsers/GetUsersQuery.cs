using MediatR;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUsers;

/// <summary>
/// Query para recuperar todos os usuários
/// </summary>
public sealed record GetUsersQuery(int Page = 1, int Size = 10, string? Order = null) : IRequest<GetUsersResult>;