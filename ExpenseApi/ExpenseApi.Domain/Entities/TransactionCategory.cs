using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ExpenseApi.Domain.Interfaces;

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
    }
}
