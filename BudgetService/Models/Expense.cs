namespace BudgetService.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public bool IsPaid { get; set; }

        public DateTime? PaymentDate { get; set; }
        public string Notes { get; set; }
        public decimal AdvanceAmount { get; set; }
    }
}
