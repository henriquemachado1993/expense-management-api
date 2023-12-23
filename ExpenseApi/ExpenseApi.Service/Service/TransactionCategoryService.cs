using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Enums;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Domain.ValueObjects;
using ExpenseApi.Service.Commands;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ExpenseApi.Service.Service
{
    public class TransactionCategoryService : ITransactionCategoryService
    {
        private readonly IBaseRepository<TransactionCategory> _repository;

        public TransactionCategoryService(IBaseRepository<TransactionCategory> repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<TransactionCategory>> CreateAsync(TransactionCategory category)
        {
            category.Id = ObjectId.GenerateNewId();

            var tranResult = await _repository.CreateAsync(category);

            return ServiceResult<TransactionCategory>.CreateValidResult(tranResult);
        }

        public async Task<ServiceResult<TransactionCategory>> UpdateAsync(TransactionCategory category)
        {
            var entity = await _repository.GetByIdAsync(category.Id);
            if (entity == null)
                return ServiceResult<TransactionCategory>.CreateInvalidResult("Registro não encontrado.");

            entity.Description = category.Description;
            entity.Name = category.Name;
            entity.Icon = category.Icon;

            var resultUpdate = await _repository.UpdateAsync(entity);

            return ServiceResult<TransactionCategory>.CreateValidResult(resultUpdate);
        }

        public async Task<ServiceResult<bool>> DeleteAsync(string id)
        {
            var entity = await _repository.GetByIdAsync(ObjectId.Parse(id));

            if (entity != null)
            {
                await _repository.DeleteAsync(new ObjectId(id));

                return ServiceResult<bool>.CreateValidResult(true);
            }
            else
            {
                return ServiceResult<bool>.CreateInvalidResult("Registro não encontrado.");
            }
        }

        public async Task<ServiceResult<List<TransactionCategory>>> GetAllAsync()
        {
            return ServiceResult<List<TransactionCategory>>.CreateValidResult(await _repository.FindAsync(_ => true));
        }

        public async Task<ServiceResult<TransactionCategory>> GetByIdAsync(string id)
        {
            var entity = await _repository.GetByIdAsync(ObjectId.Parse(id));
            if (entity == null)
                return ServiceResult<TransactionCategory>.CreateInvalidResult("Registro não encontrado.");

            return ServiceResult<TransactionCategory>.CreateValidResult(entity);
        }

    }
}
