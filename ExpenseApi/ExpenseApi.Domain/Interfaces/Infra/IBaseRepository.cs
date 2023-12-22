using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApi.Domain.Interfaces
{
    public interface IBaseRepository<T>

    {
        /// <summary>
        /// Obtém todos os registros.
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetAllAsync();
        /// <summary>
        /// Obtém por Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetByIdAsync(ObjectId id);
        /// <summary>
        /// Obtém filtrado.
        /// </summary>
        /// <param name="filterExpression"></param>
        /// <returns></returns>
        Task<List<T>> FindAsync(Expression<Func<T, bool>> filterExpression);
        /// <summary>
        /// Cria um novo registro.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> CreateAsync(T entity);
        /// <summary>
        /// Altera um registro.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> UpdateAsync(T entity);
        /// <summary>
        /// Delete um registro.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(ObjectId id);
    }
}
