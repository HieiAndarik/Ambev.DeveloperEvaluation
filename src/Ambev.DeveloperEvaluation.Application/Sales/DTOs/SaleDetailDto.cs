using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.DTOs
{
    public class SaleDetailDto
    {
        public SaleDetailDto()
        {
            CustomerName = string.Empty;
            BranchName = string.Empty;
            Items = new List<SaleItemDto>();
            SaleDate = DateTime.UtcNow;
        }
        public Guid Id { get; set; }
        public int SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public string CustomerName { get; set; }
        public string BranchName { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsCancelled { get; set; }
        public List<SaleItemDto> Items { get; set; } = new List<SaleItemDto>();
    }
}