namespace Ambev.DeveloperEvaluation.Application.Sales.DTOs
{
    public class SaleDto
    {
        public SaleDto()
        {
            CustomerName = string.Empty;
            BranchName = string.Empty;
            SaleDate = DateTime.UtcNow;
        }
        public Guid Id { get; set; }
        public int SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public string CustomerName { get; set; }
        public string BranchName { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsCancelled { get; set; }
    }
}