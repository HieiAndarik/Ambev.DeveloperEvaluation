using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

public sealed class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<Cart, CartDto>();
    }
}