using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetService.Data;
using BudgetService.Models;

namespace BudgetService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpensesController : ControllerBase
    {
        private readonly BudgetDbContext _context;

        public ExpensesController(BudgetDbContext context)
        {
            _context = context;
        }

        // 1. ENDPOINT: Lista wszystkich wydatków (GET)
        [HttpGet]
        public async Task<IActionResult> GetExpenses()
        {
            var expenses = await _context.Expenses.ToListAsync();
            return Ok(expenses);
        }

        // 2. ENDPOINT: Podsumowanie analityczne budżetu (GET)
        [HttpGet("summary")]
        public async Task<IActionResult> GetBudgetSummary()
        {
            var expenses = await _context.Expenses.ToListAsync();

            var totalCost = expenses.Sum(e => e.Amount);

            var totalPaid = expenses.Sum(e => e.IsPaid ? e.Amount : e.AdvanceAmount);

            var remainingToPay = totalCost - totalPaid;

            return Ok(new
            {
                TotalCost = totalCost,
                TotalPaid = totalPaid,
                RemainingToPay = remainingToPay,
                ItemsCount = expenses.Count
            });
        }

        // 3. ENDPOINT: Dodanie nowego wydatku (POST)
        [HttpPost]
        public async Task<IActionResult> AddExpense([FromBody] Expense newExpense)
        {
            if (newExpense.AdvanceAmount >= newExpense.Amount)
            {
                newExpense.IsPaid = true;
            }

            _context.Expenses.Add(newExpense);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetExpenses), new { id = newExpense.Id }, newExpense);
        }

        // 4. ENDPOINT: Edycja/Aktualizacja wydatku (PUT)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] Expense updatedExpense)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null) return NotFound("Nie znaleziono wydatku.");

            expense.Name = updatedExpense.Name;
            expense.Amount = updatedExpense.Amount;
            expense.Category = updatedExpense.Category;
            expense.AdvanceAmount = updatedExpense.AdvanceAmount;
            expense.Notes = updatedExpense.Notes;
            expense.PaymentDate = updatedExpense.PaymentDate;

            expense.IsPaid = updatedExpense.IsPaid || expense.AdvanceAmount >= expense.Amount;

            await _context.SaveChangesAsync();
            return Ok(expense);
        }

        // 5. ENDPOINT: Usuwanie wydatku (DELETE)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null) return NotFound("Nie znaleziono wydatku.");

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return Ok("Wydatek usunięty.");
        }
    }
}