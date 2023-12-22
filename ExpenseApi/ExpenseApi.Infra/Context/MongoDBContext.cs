using MongoDB.Driver;
using ExpenseApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
