using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Application.Sales.DTOs;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales
{
    public class GetSalesHandler : IRequestHandler<GetSalesQuery, PagedResult<SaleDto>>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public GetSalesHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<SaleDto>> Handle(GetSalesQuery request, CancellationToken cancellationToken)
        {
            var sales = await _saleRepository.GetAllAsync(request.Page, request.Size, request.Order);
            var totalCount = await _saleRepository.GetTotalCountAsync();

            var salesDto = _mapper.Map<List<SaleDto>>(sales);

            return new PagedResult<SaleDto>(
                salesDto,
                totalCount,
                request.Page,
                request.Size);
        }
    }
}