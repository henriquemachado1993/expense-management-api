using ExpenseApi.Models.Shared;

namespace ExpenseApi.Models
{
    public class FilterTransactionRequestModel : BasePage
    {
        public string? Name { get; set; }
        /// <summary>
        /// Valores possíveis "Expense" ou "Income"
        /// </summary>
        public string? TransactionType { get; set; }
    }
}
