using Ambev.DeveloperEvaluation.Domain.Common;
using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale : BaseEntity
    {
        public Sale()
        {
            Items = new List<SaleItem>();
        }

        public int SaleNumber { get; private set; }
        public DateTime SaleDate { get; private set; }
        public int CustomerId { get; private set; }
        public string CustomerName { get; private set; } = string.Empty;
        public int BranchId { get; private set; }
        public string BranchName { get; private set; } = string.Empty;
        public decimal TotalAmount { get; private set; }
        public bool IsCancelled { get; private set; }
        public virtual ICollection<SaleItem> Items { get; private set; }

        public static Sale Create(int saleNumber, int customerId, string customerName,
            int branchId, string branchName)
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

        public void AddItem(int productId, string productName, int quantity,
            decimal unitPrice)
        {
            if (quantity > 20)
                throw new DomainException("Cannot sell above 20 identical items");

            var discount = CalculateDiscount(quantity);
            var item = SaleItem.Create(productId, productName, quantity, unitPrice, discount);
            Items.Add(item);
            RecalculateTotalAmount();
        }

        public void RemoveItem(Guid itemId)
        {
            var item = Items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
                throw new DomainException("Item not found");

            item.Cancel();
            RecalculateTotalAmount();
        }

        public void Cancel()
        {
            if (IsCancelled)
                throw new DomainException("Sale is already cancelled");

            IsCancelled = true;
            foreach (var item in Items)
            {
                item.Cancel();
            }

            // Here we would publish a SaleCancelled event
        }

        private decimal CalculateDiscount(int quantity)
        {
            if (quantity < 4)
                return 0;

            if (quantity >= 10 && quantity <= 20)
                return 0.2m; 

            return 0.1m; 
        }

        private void RecalculateTotalAmount()
        {
            TotalAmount = Items.Where(i => !i.IsCancelled)
                              .Sum(i => i.TotalAmount);
        }
    }
}