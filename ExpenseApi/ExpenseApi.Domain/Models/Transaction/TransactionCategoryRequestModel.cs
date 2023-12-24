using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ExpenseApi.Domain.Models.Transaction
{
    public class TransactionCategoryRequestModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        /// <summary>
        /// Icone da categoria
        /// </summary>
        public string? Icon { get; set; }
    }
}
