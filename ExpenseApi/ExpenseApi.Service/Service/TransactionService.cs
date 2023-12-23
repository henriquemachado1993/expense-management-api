using MongoDB.Bson;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Domain.ValueObjects;
using ExpenseApi.Service.Commands;
using ExpenseApi.Service.Commands.Manager;
using ExpenseApi.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace ExpenseApi.Service
{
    public class TransactionService : ITransactionService
    {

        private readonly IBaseRepository<Transaction> _repository;
        private readonly IServiceProvider _serviceProvider;

        public TransactionService(IBaseRepository<Transaction> repository, IServiceProvider serviceProvider)
        {
            _repository = repository;
            _serviceProvider = serviceProvider;
        }

        public async Task<ServiceResult<Transaction>> CreateAsync(Transaction transaction)
        {
            transaction.Id = ObjectId.GenerateNewId();

            var tranResult = await _repository.CreateAsync(transaction);

            // Se for uma entrada, nós adicionamos à alguma conta.
            if (tranResult != null && tranResult.TransactionType == TransactionType.Income.ToString())
            {
                var transactionManager = new TransactionManager();
                var commandDesposit = new BankAccountDepositCommand(_serviceProvider.GetRequiredService<IBankAccountService>(), tranResult.UserId.ToString(), tranResult.Amount);
               
                try
                {
                    await transactionManager.ExecuteCommand(commandDesposit);
                }
                catch (Exception ex)
                {
                    await transactionManager.UndoLastCommand();
                    return ServiceResult<Transaction>.CreateInvalidResult(ex, "A transação foi criada, porém houve um problema ao atualizar os valores em suas contas!");
                }
            }

            return ServiceResult<Transaction>.CreateValidResult(tranResult);
        }

        public async Task<ServiceResult<Transaction>> UpdateAsync(Transaction transaction)
        {
            var entity = await _repository.GetByIdAsync(transaction.Id);
            if(entity == null)
                return ServiceResult<Transaction>.CreateInvalidResult("Registro não encontrado.");

            entity.Description = transaction.Description;
            entity.IsMonthlyRecurrence = transaction.IsMonthlyRecurrence;
            entity.IsPaid = transaction.IsPaid;
            entity.UserId = transaction.UserId;
            entity.Category = transaction.Category;
            entity.ExpenseDate = transaction.ExpenseDate;

            var resultUpdate = await _repository.UpdateAsync(entity);

            return ServiceResult<Transaction>.CreateValidResult(resultUpdate);
        }

        public async Task<ServiceResult<bool>> DeleteAsync(string userId, string id)
        {
            var entity = (await _repository.FindAsync(x => x.UserId == new ObjectId(userId) && x.Id == new ObjectId(id)))?.FirstOrDefault();

            if (entity != null)
            {
                var transactionManager = new TransactionManager();
                var command = new BankAccountWithDrawCommand(_serviceProvider.GetRequiredService<IBankAccountService>(), entity.UserId.ToString(), entity.Amount);

                try
                {
                    await _repository.DeleteAsync(new ObjectId(id));

                    // Se for uma entrada, nós debitamos de alguma conta.
                    if (entity.TransactionType == TransactionType.Income.ToString())
                        await transactionManager.ExecuteCommand(command);
                }
                catch
                {
                    await transactionManager.UndoLastCommand();
                    return ServiceResult<bool>.CreateInvalidResult("Não foi possível debitar o valor de alguma conta.");
                }

                return ServiceResult<bool>.CreateValidResult(true);
            }
            else
            {
                return ServiceResult<bool>.CreateInvalidResult("Registro não encontrado.");
            }
        }

        public async Task<ServiceResult<List<Transaction>>> GetAllAsync(string userId)
        {
            return ServiceResult<List<Transaction>>.CreateValidResult(await _repository.FindAsync(x => x.UserId == new ObjectId(userId)));
        }

        public async Task<ServiceResult<Transaction>> GetByIdAsync(string userId, string id)
        {
            var result = await _repository.FindAsync(x => x.UserId == new ObjectId(userId) && x.Id == new ObjectId(id));
            var entity = result?.FirstOrDefault();
            if(entity == null)
                return ServiceResult<Transaction>.CreateInvalidResult("Registro não encontrado.");

            return ServiceResult<Transaction>.CreateValidResult(entity);
        }
    }
}
