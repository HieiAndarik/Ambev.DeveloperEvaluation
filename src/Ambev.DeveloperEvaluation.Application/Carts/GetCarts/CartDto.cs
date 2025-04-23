using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

public sealed class CartDto
{
    public int Id { get; init; }
    public Guid CustomerId { get; init; }
    public DateTime Date { get; init; }
    public List<CartItemDto> Items { get; init; } = new();
}
