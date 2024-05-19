using BeireMKit.Data.Interfaces.MongoDB;
using BeireMKit.Data.Models;
using MongoDB.Driver;

namespace ExpenseApi.Infra.Context
{
    public class MongoDBContext : IBaseMongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(MongoDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoDatabase Database => _database;
    }
}
