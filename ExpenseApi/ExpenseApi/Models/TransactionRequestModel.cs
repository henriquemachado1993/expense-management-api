namespace ExpenseApi.Models
{
    public class TransactionRequestModel
    {
        public string? Id { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public bool IsMonthlyRecurrence { get; set; }
        public DateTime ExpenseDate { get; set; }
        /// <summary>
        /// Valores possíveis "Expense" ou "Income"
        /// </summary>
        public string TransactionType { get; set; }
        public ExpenseCategoryModel Category { get; set; }
    }

    public class ExpenseCategoryModel
    {
        
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Icone da categoria
        /// </summary>
        public string? Icon { get; set; }
    }
}
