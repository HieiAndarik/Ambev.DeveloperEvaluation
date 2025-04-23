using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings
{
    public sealed class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<CartItem, CartItemDto>();

            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.UserId));
        }
    }
}
