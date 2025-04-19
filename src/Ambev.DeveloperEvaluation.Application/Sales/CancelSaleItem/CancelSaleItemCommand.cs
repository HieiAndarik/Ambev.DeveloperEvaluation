using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem
{
    public class CancelSaleItemCommand : IRequest<bool>
    {
        public CancelSaleItemCommand(string saleId, string itemId)
        {
            SaleId = saleId;
            ItemId = itemId;
        }
        public string SaleId { get; set; }
        public string ItemId { get; set; }
    }
}