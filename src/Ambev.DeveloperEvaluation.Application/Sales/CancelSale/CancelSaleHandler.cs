using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Services;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, bool>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ISaleService _saleService;
        private readonly IMediator _mediator;

        public CancelSaleHandler(
            ISaleRepository saleRepository,
            ISaleService saleService,
            IMediator mediator)
        {
            _saleRepository = saleRepository;
            _saleService = saleService;
            _mediator = mediator;
        }

        public async Task<bool> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByIdAsync(request.Id);
            if (sale == null)
                throw new ValidationException("Sale not found");

            _saleService.CancelSale(sale);
            await _saleRepository.UpdateAsync(sale);

            // Publish event
            await _mediator.Publish(new SaleCancelledEvent(sale.Id), cancellationToken);

            return true;
        }
    }
}