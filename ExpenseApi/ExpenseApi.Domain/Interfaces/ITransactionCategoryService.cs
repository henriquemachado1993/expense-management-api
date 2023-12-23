using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApi.Domain.Interfaces
{
    public interface ITransactionCategoryService
    {
        public Task<ServiceResult<List<TransactionCategory>>> GetAllAsync();
        public Task<ServiceResult<TransactionCategory>> GetByIdAsync(string id);
        public Task<ServiceResult<TransactionCategory>> CreateAsync(TransactionCategory category);
        public Task<ServiceResult<TransactionCategory>> UpdateAsync(TransactionCategory category);
        public Task<ServiceResult<bool>> DeleteAsync(string id);
    }
}
