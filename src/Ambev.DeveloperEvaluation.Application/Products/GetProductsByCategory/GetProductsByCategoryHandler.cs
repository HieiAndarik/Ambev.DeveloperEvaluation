using Ambev.DeveloperEvaluation.Common.Utils;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

public sealed class GetProductsByCategoryHandler : IRequestHandler<GetProductsByCategoryQuery, ProductsListResponse<ProductDto>>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public GetProductsByCategoryHandler(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProductsListResponse<ProductDto>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var filtered = await _repository.GetByCategoryAsync(request.Category, cancellationToken);

        var ordered = filtered.ApplyOrdering(request.Order);
        var totalItems = ordered.Count();
        var paged = ordered.ApplyPagination(request.Page, request.Size);
        var mapped = _mapper.Map<IEnumerable<ProductDto>>(paged);

        return new ProductsListResponse<ProductDto>
        {
            Data = mapped,
            TotalItems = totalItems,
            CurrentPage = request.Page,
            TotalPages = (int)Math.Ceiling((double)totalItems / request.Size)
        };
    }
}
