using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale : BaseEntity
    {
        public Sale()
        {
            CustomerName = string.Empty;
            BranchName = string.Empty;
            Items = new List<SaleItem>();
            SaleDate = DateTime.UtcNow;
            CustomerId = string.Empty;
            BranchId = string.Empty;
        }

        public int SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsCancelled { get; set; }
        public virtual ICollection<SaleItem> Items { get; set; } 
    }
}