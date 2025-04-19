using Ambev.DeveloperEvaluation.Domain.Entities;
public interface ISaleService
{
    Sale CreateSale(int saleNumber, string customerId, string customerName, string branchId, string branchName);
    void AddItem(Sale sale, string productId, string productName, int quantity, decimal unitPrice);
    void CancelSale(Sale sale);
    void CancelItem(Sale sale, string itemId);
    decimal CalculateDiscount(int quantity);
}