using MongoDB.Bson;
using Newtonsoft.Json;
using ExpenseApi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApi.Domain.Entities.Audit
{
    public class AuditLog : IBaseEntity
    {
        public ObjectId Id { get; set; }
        /// <summary>
        /// Possiveis valores "error", "information", "warning", "success"
        /// </summary>
        public string Type { get; set; }
        public string Message { get; set; }
        public BsonDocument Data { get; set; }

        public void SetDataFromObject(object obj)
        {
            try
            {
                // Tenta converter o objeto para um BsonDocument
                Data = BsonDocument.Parse(JsonConvert.SerializeObject(obj));
            }
            catch (JsonException)
            {
                // Se falhar, trata exceções específicas ou fornece uma resposta padrão
                Data = new BsonDocument
                {
                    { "error", "Unable to convert object to BsonDocument" }
                };
            }
            catch (Exception ex)
            {
                Data = new BsonDocument
                {
                    { "error", $"Error converting object to BsonDocument: {ex.Message}" }
                };
            }
        }
    }
}
