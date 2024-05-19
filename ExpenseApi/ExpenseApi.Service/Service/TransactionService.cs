using BeireMKit.Data.Interfaces.MongoDB;
using BeireMKit.Domain.BaseModels;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Enums;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Service.Commands;
using ExpenseApi.Service.Commands.Manager;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseApi.Service
{
    public class TransactionService : ITransactionService
    {

        private readonly IMongoRepository<Transaction> _repository;
        private readonly IServiceProvider _serviceProvider;

        public TransactionService(IMongoRepository<Transaction> repository, IServiceProvider serviceProvider)
        {
            _repository = repository;
            _serviceProvider = serviceProvider;
        }

        public async Task<BaseResult<Transaction>> CreateAsync(Transaction transaction)
        {
            transaction.Id = Guid.NewGuid();

            var tranResult = await _repository.CreateAsync(transaction);

            // Se for uma entrada, nós adicionamos à alguma conta.
            if (tranResult != null && tranResult.TransactionType == TransactionType.Income)
            {
                var transactionManager = new TransactionManager();
                var commandDesposit = new BankAccountDepositCommand(_serviceProvider.GetRequiredService<IBankAccountService>(), tranResult.UserId, tranResult.Amount);
               
                try
                {
                    await transactionManager.ExecuteCommand(commandDesposit);
                }
                catch (Exception ex)
                {
                    await transactionManager.UndoLastCommand();
                    return BaseResult<Transaction>.CreateInvalidResult(exception: ex, message: "A transação foi criada, porém houve um problema ao atualizar os valores em suas contas!");
                }
            }

            return BaseResult<Transaction>.CreateValidResult(tranResult ?? transaction);
        }

        public async Task<BaseResult<Transaction>> UpdateAsync(Transaction transaction)
        {
            var entity = await _repository.GetByIdAsync(transaction.Id);
            if(entity == null)
                return BaseResult<Transaction>.CreateInvalidResult(message:"Registro não encontrado.");

            entity.Description = transaction.Description;
            entity.IsMonthlyRecurrence = transaction.IsMonthlyRecurrence;
            entity.IsPaid = transaction.IsPaid;
            entity.UserId = transaction.UserId;
            entity.Category = transaction.Category;
            entity.ExpenseDate = transaction.ExpenseDate;

            var resultUpdate = await _repository.UpdateAsync(entity);

            return BaseResult<Transaction>.CreateValidResult(resultUpdate);
        }

        public async Task<BaseResult<bool>> DeleteAsync(Guid userId, Guid id)
        {
            var entity = (await _repository.FindAsync(x => x.UserId == userId && x.Id == id))?.FirstOrDefault();

            if (entity != null)
            {
                var transactionManager = new TransactionManager();
                var command = new BankAccountWithDrawCommand(_serviceProvider.GetRequiredService<IBankAccountService>(), entity.UserId, entity.Amount);

                try
                {
                    await _repository.DeleteAsync(id);

                    // Se for uma entrada, nós debitamos de alguma conta.
                    if (entity.TransactionType == TransactionType.Income)
                        await transactionManager.ExecuteCommand(command);
                }
                catch
                {
                    await transactionManager.UndoLastCommand();
                    return BaseResult<bool>.CreateInvalidResult(message: "Não foi possível debitar o valor de alguma conta.");
                }

                return BaseResult<bool>.CreateValidResult(true);
            }
            else
            {
                return BaseResult<bool>.CreateInvalidResult(message: "Registro não encontrado.");
            }
        }

        public async Task<BaseResult<List<Transaction>>> GetAllAsync(Guid userId)
        {
            return BaseResult<List<Transaction>>.CreateValidResult(await _repository.FindAsync(x => x.UserId == userId));
        }

        public async Task<PagingResult<List<Transaction>>> GetPagedAsync(QueryCriteria<Transaction> queryCriteria)
        {
            return await _repository.FindPagedAsync(queryCriteria);
        }

        public async Task<BaseResult<Transaction>> GetByIdAsync(Guid userId, Guid id)
        {
            var result = await _repository.FindAsync(x => x.UserId == userId && x.Id == id);
            var entity = result?.FirstOrDefault();
            if(entity == null)
                return BaseResult<Transaction>.CreateInvalidResult(message: "Registro não encontrado.");

            return BaseResult<Transaction>.CreateValidResult(entity);
        }
    }
}
