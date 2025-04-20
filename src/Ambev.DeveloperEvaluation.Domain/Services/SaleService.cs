using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Services
{
    public class SaleService : ISaleService
    {
        public Sale CreateSale(int saleNumber, string customerId, string customerName, string branchId, string branchName)
        {
            return new Sale
            {
                SaleNumber = saleNumber,
                SaleDate = DateTime.UtcNow,
                CustomerId = customerId,
                CustomerName = customerName,
                BranchId = branchId,
                BranchName = branchName,
                IsCancelled = false
            };
        }

        public void AddItem(Sale sale, string productId, string productName, int quantity, decimal unitPrice)
        {
            ValidateItemQuantity(quantity);

            var discount = CalculateDiscount(quantity);

            var item = new SaleItem
            {
                ProductId = productId,
                ProductName = productName,
                Quantity = quantity,
                UnitPrice = unitPrice,
                Discount = discount,
                IsCancelled = false,
                SaleId = sale.Id,
            };

            CalculateItemTotalAmount(item);
            sale.Items.Add(item);
            CalculateSaleTotalAmount(sale);
        }

        public void CancelSale(Sale sale)
        {
            if (sale.IsCancelled)
                throw new DomainException("Sale is already cancelled");

            sale.IsCancelled = true;
            foreach (var item in sale.Items)
            {
                item.IsCancelled = true;
            }
            CalculateSaleTotalAmount(sale);
        }

        public void CancelItem(Sale sale, string itemId)
        {
            var item = sale.Items.FirstOrDefault(i => i.Id.ToString() == itemId);
            if (item == null)
                throw new DomainException("Item not found");

            item.IsCancelled = true;
            CalculateSaleTotalAmount(sale);
        }

        public decimal CalculateDiscount(int quantity)
        {
            if (quantity < 4)
                return 0;

            if (quantity >= 10 && quantity <= 20)
                return 0.2m; // 20% discount

            return 0.1m; // 10% discount
        }

        private void ValidateItemQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("Quantity must be greater than zero");

            if (quantity > 20)
                throw new DomainException("Cannot sell above 20 identical items");
        }

        private void CalculateItemTotalAmount(SaleItem item)
        {
            decimal subtotal = item.Quantity * item.UnitPrice;
            decimal discountAmount = subtotal * item.Discount;
            item.TotalAmount = subtotal - discountAmount;
        }

        private void CalculateSaleTotalAmount(Sale sale)
        {
            sale.TotalAmount = sale.Items
                .Where(i => !i.IsCancelled)
                .Sum(i => i.TotalAmount);
        }
    }
}