namespace ExpenseApi.Domain.Models.BankAccount
{
    public class BankAccountBalanceRequestModel
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
    }
}
