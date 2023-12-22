namespace ExpenseApi.Models
{
    public class ExpenseRequestModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int IsPaid { get; set; }
        public int IsMonthlyRecurrence { get; set; }
        public DateTime ExpenseDate { get; set; }

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
