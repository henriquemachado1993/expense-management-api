using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ExpenseApi.Models
{
    public class TransactionCategoryRequestModel
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        /// <summary>
        /// Icone da categoria
        /// </summary>
        public string? Icon { get; set; }
    }
}
