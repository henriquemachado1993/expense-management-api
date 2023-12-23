using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Domain.Extensions;

namespace ExpenseApi.Domain.Entities
{
    public class Transaction : IBaseEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string AmountText
        {
            get
            {
                return Amount.ConvertToBrazilianReal();
            }
        }
        public bool IsPaid { get; set; }
        public bool IsMonthlyRecurrence { get; set; }
        public DateTime ExpenseDate { get; set; }

        /// <summary>
        /// Valores possíveis "Expense" ou "Income"
        /// </summary>
        public string TransactionType { get; set; }
        public TransactionCategory Category { get; set; }
    }
}
