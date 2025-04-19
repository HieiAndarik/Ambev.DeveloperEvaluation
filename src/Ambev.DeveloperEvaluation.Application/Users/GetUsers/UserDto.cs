namespace Ambev.DeveloperEvaluation.Application.Users.GetUsers;

/// <summary>
/// Representa um usuário para exibição pública
/// </summary>
public sealed class UserDto
{
    public Guid Id { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}