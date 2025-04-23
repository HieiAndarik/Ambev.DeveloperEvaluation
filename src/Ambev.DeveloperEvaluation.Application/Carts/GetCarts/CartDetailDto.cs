using Ambev.DeveloperEvaluation.Application.Carts.GetCarts;
using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCartById;

public sealed class CartDetailDto
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public DateTime Date { get; init; }
    public List<CartItemDto> Items { get; init; } = new();
}