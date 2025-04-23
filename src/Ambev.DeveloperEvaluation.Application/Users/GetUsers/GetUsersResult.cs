using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUsers;

public sealed class GetUsersResult
{
    public IEnumerable<UserDto> Data { get; set; } = [];
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}