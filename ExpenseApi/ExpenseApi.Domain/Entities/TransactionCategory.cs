using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Domain.Enums;

namespace ExpenseApi.Domain.Entities
{
    public class TransactionCategory : IBaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Icone da categoria
        /// </summary>
        public string? Icon { get; set; }
        /// <summary>
        /// Possíveis valores "Expense","Income"
        /// </summary>
        public TransactionType Type { get; set; }
    }
}
