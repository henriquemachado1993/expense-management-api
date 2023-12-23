using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Domain.ValueObjects;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ExpenseApi.Service.Service
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBaseRepository<BankAccount> _repository;

        public BankAccountService(IBaseRepository<BankAccount> repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<BankAccount>> GetByIdAsync(string userId, string id)
        {
            var result = await _repository.FindAsync(x => x.UserId == new ObjectId(userId) && x.Id == new ObjectId(id));
            var entity = result?.FirstOrDefault();
            if (entity == null)
                return ServiceResult<BankAccount>.CreateInvalidResult("Registro não encontrado.");

            return ServiceResult<BankAccount>.CreateValidResult(entity);
        }

        public async Task<ServiceResult<List<BankAccount>>> GetAllAsync(string userId)
        {
            return ServiceResult<List<BankAccount>>.CreateValidResult(await _repository.FindAsync(x => x.UserId == new ObjectId(userId)));
        }

        public async Task<ServiceResult<BankAccount>> CreateAsync(BankAccount bank)
        {
            bank.Id = ObjectId.GenerateNewId();
            return ServiceResult<BankAccount>.CreateValidResult(await _repository.CreateAsync(bank));
        }

        public async Task<ServiceResult<BankAccount>> UpdateAsync(BankAccount bank)
        {
            var result = await _repository.FindAsync(x => x.UserId == bank.UserId && x.Id == bank.Id);
            var entity = result?.FirstOrDefault();

            if (entity == null)
                return ServiceResult<BankAccount>.CreateInvalidResult($"Registro não encontrado");

            entity.Name = bank.Name;
            entity.UserId = bank.UserId;
            entity.IsMain = bank.IsMain;
            entity.Type = bank.Type;

            return ServiceResult<BankAccount>.CreateValidResult(await _repository.UpdateAsync(entity));
        }

        public async Task<ServiceResult<bool>> WithDrawAsync(string userId, string id, decimal amount)
        {
            var result = await _repository.FindAsync(x => x.UserId == new ObjectId(userId) && x.Id == new ObjectId(id));
            var entity = result?.FirstOrDefault();

            if (entity == null)
                return ServiceResult<bool>.CreateInvalidResult($"Não foi possível debitar o valor {amount}"); // TODO: formatar o valor.

            entity.WithDraw(amount);

            await _repository.UpdateAsync(entity);

            return ServiceResult<bool>.CreateValidResult(true);
        }

        public async Task<ServiceResult<bool>> DepositAsync(string userId, string id, decimal amount)
        {
            var result = await _repository.FindAsync(x => x.UserId == new ObjectId(userId) && x.Id == new ObjectId(id));
            var entity = result?.FirstOrDefault();
            if (entity == null)
                return ServiceResult<bool>.CreateInvalidResult($"Não foi possível incluir o valor {amount}"); // TODO: formatar o valor.

            entity.Deposit(amount);

            await _repository.UpdateAsync(entity);

            return ServiceResult<bool>.CreateValidResult(true);
        }

        public async Task<ServiceResult<bool>> DeleteAsync(string userId, string id)
        {
            var result = await _repository.FindAsync(x => x.UserId == new ObjectId(userId) && x.Id == new ObjectId(id));
            var entity = result?.FirstOrDefault();
            if (entity == null)
                return ServiceResult<bool>.CreateInvalidResult($"Não foi possível excluir conta");

            await _repository.DeleteAsync(entity.Id);

            return ServiceResult<bool>.CreateValidResult(true);
        }
    }
}
