using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ExpenseApi.Models
{
    public class BankAccountRequestModel
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }

        public string Name { get; set; }
        /// <summary>
        /// Valores possíveis "Corrente", "Conta salário"
        /// </summary>
        public string Type { get; set; }
        public decimal AccountValue { get; set; }

        public bool IsMain { get; set; }
    }
}
