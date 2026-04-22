using Microsoft.EntityFrameworkCore;

namespace BudgetService.Data
{
    public class BudgetDbContext: DbContext
    {
        public BudgetDbContext(DbContextOptions<BudgetDbContext> options) : base(options)
        {
        }
        public DbSet<Models.Expense> Expenses { get; set; }
    }
}
