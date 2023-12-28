using ExpenseApi.Models.Shared;

namespace ExpenseApi.Domain.Models.Transaction
{
    public class FilterTransactionRequestModel : BasePage
    {
        public string? Name { get; set; }
        /// <summary>
        /// Valores possíveis "Expense" ou "Income"
        /// </summary>
        public string? TransactionType { get; set; }
        public Guid? CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}
