using Ambev.DeveloperEvaluation.Application.Carts.GetCarts;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts
{
    public class GetCartsHandler : IRequestHandler<GetCartsQuery, GetCartsResult>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public GetCartsHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<GetCartsResult> Handle(GetCartsQuery request, CancellationToken cancellationToken)
        {
            var carts = await _cartRepository.GetAllAsync(cancellationToken);
            var totalItems = carts.Count();
            var pagedCarts = carts
                .Skip((request.Page - 1) * request.Size)
                .Take(request.Size)
                .ToList();

            var result = new GetCartsResult
            {
                Items = _mapper.Map<List<CartDto>>(pagedCarts),
                TotalItems = totalItems,
                CurrentPage = request.Page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)request.Size)
            };

            return result;
        }
    }
}
