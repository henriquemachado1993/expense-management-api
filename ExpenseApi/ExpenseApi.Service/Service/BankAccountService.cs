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

        public async Task<ServiceResult<BankAccount>> GetByIdAsync(Guid userId, Guid id)
        {
            var result = await _repository.FindAsync(x => x.UserId == userId && x.Id == id);
            var entity = result?.FirstOrDefault();
            if (entity == null)
                return ServiceResult<BankAccount>.CreateInvalidResult("Registro não encontrado.");

            return ServiceResult<BankAccount>.CreateValidResult(entity);
        }

        public async Task<ServiceResult<List<BankAccount>>> GetAllAsync(Guid userId)
        {
            return ServiceResult<List<BankAccount>>.CreateValidResult(await _repository.FindAsync(x => x.UserId == userId));
        }

        public async Task<ServiceResult<BankAccount>> CreateAsync(BankAccount bank)
        {
            bank.Id = Guid.NewGuid();
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

        public async Task<ServiceResult<bool>> WithDrawAsync(Guid userId, Guid id, decimal amount)
        {
            var result = await _repository.FindAsync(x => x.UserId == userId && x.Id == id);
            var entity = result?.FirstOrDefault();

            if (entity == null)
                return ServiceResult<bool>.CreateInvalidResult($"Não foi possível debitar o valor {amount}"); // TODO: formatar o valor.

            entity.WithDraw(amount);

            await _repository.UpdateAsync(entity);

            return ServiceResult<bool>.CreateValidResult(true);
        }

        public async Task<ServiceResult<bool>> DepositAsync(Guid userId, Guid id, decimal amount)
        {
            var result = await _repository.FindAsync(x => x.UserId == userId && x.Id == id);
            var entity = result?.FirstOrDefault();
            if (entity == null)
                return ServiceResult<bool>.CreateInvalidResult($"Não foi possível incluir o valor {amount}"); // TODO: formatar o valor.

            entity.Deposit(amount);

            await _repository.UpdateAsync(entity);

            return ServiceResult<bool>.CreateValidResult(true);
        }

        public async Task<ServiceResult<bool>> DeleteAsync(Guid userId, Guid id)
        {
            var result = await _repository.FindAsync(x => x.UserId == userId && x.Id == id);
            var entity = result?.FirstOrDefault();
            if (entity == null)
                return ServiceResult<bool>.CreateInvalidResult($"Não foi possível excluir conta");

            await _repository.DeleteAsync(entity.Id);

            return ServiceResult<bool>.CreateValidResult(true);
        }
    }
}
