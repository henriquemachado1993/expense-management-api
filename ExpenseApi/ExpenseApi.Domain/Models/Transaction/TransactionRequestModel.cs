namespace ExpenseApi.Domain.Models.Transaction
{
    public class TransactionRequestModel
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public bool IsMonthlyRecurrence { get; set; }
        public DateTime ExpenseDate { get; set; }
        /// <summary>
        /// Valores possíveis "Expense" ou "Income"
        /// </summary>
        public string TransactionType { get; set; }
        public TransactionCategoryRequestModel Category { get; set; }
    }
}
