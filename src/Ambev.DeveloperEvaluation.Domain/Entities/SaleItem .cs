using Ambev.DeveloperEvaluation.Domain.Common;
using System;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem : BaseEntity
    {
        public SaleItem()
        {
            ProductName = string.Empty;
            ProductId = string.Empty;
            Sale = null!;
        }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsCancelled { get; set; }
        public Guid SaleId { get; set; }
        public virtual Sale Sale { get; set; }
    }
}