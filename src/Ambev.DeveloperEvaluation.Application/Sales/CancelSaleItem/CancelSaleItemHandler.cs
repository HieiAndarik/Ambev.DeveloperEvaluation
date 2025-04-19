using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Services;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem
{
    public class CancelSaleItemHandler : IRequestHandler<CancelSaleItemCommand, bool>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ISaleService _saleService;
        private readonly IMediator _mediator;

        public CancelSaleItemHandler(
            ISaleRepository saleRepository,
            ISaleService saleService,
            IMediator mediator)
        {
            _saleRepository = saleRepository;
            _saleService = saleService;
            _mediator = mediator;
        }

        public async Task<bool> Handle(CancelSaleItemCommand request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByIdAsync(Guid.Parse(request.SaleId));
            if (sale == null)
                throw new ValidationException("Sale not found");

            _saleService.CancelItem(sale, request.ItemId);
            await _saleRepository.UpdateAsync(sale);

            // Publish event
            await _mediator.Publish(new ItemCancelledEvent(sale.Id, Guid.Parse(request.ItemId)), cancellationToken);

            return true;
        }
    }
}