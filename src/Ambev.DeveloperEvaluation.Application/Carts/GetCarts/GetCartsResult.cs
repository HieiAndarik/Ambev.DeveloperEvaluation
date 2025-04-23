using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

public sealed class GetCartsResult
{
    public IEnumerable<CartDto> Items { get; init; } = new List<CartDto>();
    public int TotalItems { get; init; }
    public int CurrentPage { get; init; }
    public int TotalPages { get; init; }
}