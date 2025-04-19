using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem : BaseEntity
    {
        public int SaleId { get; private set; }
        public int ProductId { get; private set; }
        public string ProductName { get; private set; } = string.Empty;
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalAmount { get; private set; }
        public bool IsCancelled { get; private set; }
        public virtual Sale Sale { get; private set; } = new Sale();

        public static SaleItem Create(int productId, string productName, int quantity,
            decimal unitPrice, decimal discount)
        {
            var item = new SaleItem
            {
                ProductId = productId,
                ProductName = productName,
                Quantity = quantity,
                UnitPrice = unitPrice,
                Discount = discount,
                IsCancelled = false
            };

            item.CalculateTotalAmount();
            return item;
        }

        public void Cancel()
        {
            IsCancelled = true;
            // Here we would publish an ItemCancelled event
        }

        private void CalculateTotalAmount()
        {
            decimal subtotal = Quantity * UnitPrice;
            decimal discountAmount = subtotal * Discount;
            TotalAmount = subtotal - discountAmount;
        }
    }
}