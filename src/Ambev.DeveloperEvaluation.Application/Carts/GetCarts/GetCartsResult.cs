using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

public sealed class GetCartsResult
{
    public IEnumerable<CartDto> Carts { get; init; } = new List<CartDto>();
    public int TotalCount { get; init; }
}