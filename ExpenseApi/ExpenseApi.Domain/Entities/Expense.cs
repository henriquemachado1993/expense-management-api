using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ExpenseApi.Domain.Interfaces;

namespace ExpenseApi.Domain.Entities
{
    public class Expense : IBaseEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId UserId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int IsPaid { get; set; }
        public int IsMonthlyRecurrence { get; set; }
        public DateTime ExpenseDate { get; set; }

        public ExpenseCategory Category { get; set; }
    }
}
