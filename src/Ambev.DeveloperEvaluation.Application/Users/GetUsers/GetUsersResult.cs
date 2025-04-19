using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUsers;

/// <summary>
/// Resultado da consulta de usuários
/// </summary>
public sealed class GetUsersResult
{
    public IEnumerable<UserDto> Users { get; init; } = new List<UserDto>();
    public int TotalCount { get; init; }
}