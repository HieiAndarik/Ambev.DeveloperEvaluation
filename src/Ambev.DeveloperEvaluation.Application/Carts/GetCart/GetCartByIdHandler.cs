using Ambev.DeveloperEvaluation.Domain.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

public sealed class GetCartByIdHandler : IRequestHandler<GetCartByIdQuery, CartDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    public GetCartByIdHandler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    public async Task<CartDto> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByIdAsync(request.Id, cancellationToken);

        return cart is null ? null! : _mapper.Map<CartDto>(cart);
    }
}