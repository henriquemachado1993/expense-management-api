using BeireMKit.Data.Interfaces.MongoDB;
using BeireMKit.Domain.BaseModels;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;

namespace ExpenseApi.Service.Service
{
    public class TransactionCategoryService : ITransactionCategoryService
    {
        private readonly IMongoRepository<TransactionCategory> _repository;

        public TransactionCategoryService(IMongoRepository<TransactionCategory> repository)
        {
            _repository = repository;
        }

        public async Task<BaseResult<TransactionCategory>> CreateAsync(TransactionCategory category)
        {
            category.Id = Guid.NewGuid();

            var tranResult = await _repository.CreateAsync(category);

            return BaseResult<TransactionCategory>.CreateValidResult(tranResult);
        }

        public async Task<BaseResult<TransactionCategory>> UpdateAsync(TransactionCategory category)
        {
            var entity = await _repository.GetByIdAsync(category.Id);
            if (entity == null)
                return BaseResult<TransactionCategory>.CreateInvalidResult(message: "Registro não encontrado.");

            entity.Description = category.Description;
            entity.Name = category.Name;
            entity.Icon = category.Icon;

            var resultUpdate = await _repository.UpdateAsync(entity);

            return BaseResult<TransactionCategory>.CreateValidResult(resultUpdate);
        }

        public async Task<BaseResult<bool>> DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity != null)
            {
                await _repository.DeleteAsync(id);

                return BaseResult<bool>.CreateValidResult(true);
            }
            else
            {
                return BaseResult<bool>.CreateInvalidResult(message: "Registro não encontrado.");
            }
        }

        public async Task<BaseResult<List<TransactionCategory>>> GetAllAsync()
        {
            return BaseResult<List<TransactionCategory>>.CreateValidResult(await _repository.FindAsync(_ => true));
        }

        public async Task<BaseResult<TransactionCategory>> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return BaseResult<TransactionCategory>.CreateInvalidResult(message: "Registro não encontrado.");

            return BaseResult<TransactionCategory>.CreateValidResult(entity);
        }

    }
}
