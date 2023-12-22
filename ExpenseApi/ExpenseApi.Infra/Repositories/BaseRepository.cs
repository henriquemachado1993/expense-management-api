using MongoDB.Bson;
using MongoDB.Driver;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Infra.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApi.Infra.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T>
    {
        private readonly IMongoCollection<T> _collection;

        public BaseRepository(MongoDBContext mongoDBContext)
        {
            _collection = mongoDBContext.Database.GetCollection<T>(typeof(T).Name);
        }

        public async Task<List<T>> GetAllAsync()
        {
            var result = await _collection.Find(_ => true).ToListAsync();
            return result;
        }

        public async Task<T> GetByIdAsync(ObjectId id)
        {
            Expression<Func<T, bool>> filterExpression = entity => ((IBaseEntity)entity).Id == id;
            var result = await _collection.Find(filterExpression).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> filterExpression)
        {
            var result = await _collection.Find(filterExpression).ToListAsync();
            return result;
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            // Assumindo que a entidade tem um campo chamado "Id"
            var id = (ObjectId)entity.GetType().GetProperty("Id").GetValue(entity, null);
            await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", id), entity);

            return entity;
        }

        public async Task DeleteAsync(ObjectId id)
        {
            await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", id));
        }
    }
}
