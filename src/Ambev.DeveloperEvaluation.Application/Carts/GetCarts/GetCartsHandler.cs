using Ambev.DeveloperEvaluation.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

public sealed class GetCartsHandler : IRequestHandler<GetCartsQuery, GetCartsResult>
{
    private readonly ICartRepository _repository;
    private readonly IMapper _mapper;

    public GetCartsHandler(ICartRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GetCartsResult> Handle(GetCartsQuery request, CancellationToken cancellationToken)
    {
        var allCarts = await _repository.GetAllAsync(cancellationToken);

        var ordered = request.Order?.ToLower() switch
        {
            "userid" => allCarts.OrderBy(c => c.UserId),
            "date" => allCarts.OrderBy(c => c.Date),
            _ => allCarts
        };

        var paged = ordered
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .ToList();

        return new GetCartsResult
        {
            Carts = _mapper.Map<IEnumerable<CartDto>>(paged),
            TotalCount = allCarts.Count()
        };
    }
}