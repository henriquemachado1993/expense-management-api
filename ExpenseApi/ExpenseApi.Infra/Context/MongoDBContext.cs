using ExpenseApi.Domain.Entities;
using MongoDB.Driver;

namespace ExpenseApi.Infra.Context
{
    public class MongoDBContext
    {
        public readonly IMongoDatabase Database;

        public MongoDBContext(MongoDBConfig config)
        {
            var client = new MongoClient(config.ConnectionString);
            Database = client.GetDatabase(config.DatabaseName);
        }
    }
}
