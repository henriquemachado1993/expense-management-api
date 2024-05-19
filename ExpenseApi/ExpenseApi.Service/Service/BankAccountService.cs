using BeireMKit.Data.Interfaces.MongoDB;
using BeireMKit.Domain.BaseModels;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Extensions;
using ExpenseApi.Domain.Interfaces;

namespace ExpenseApi.Service.Service
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IMongoRepository<BankAccount> _repository;

        public BankAccountService(IMongoRepository<BankAccount> repository)
        {
            _repository = repository;
        }

        public async Task<BaseResult<BankAccount>> GetByIdAsync(Guid userId, Guid id)
        {
            var result = await _repository.FindAsync(x => x.UserId == userId && x.Id == id);
            var entity = result?.FirstOrDefault();
            if (entity == null)
                return BaseResult<BankAccount>.CreateInvalidResult(message: "Registro não encontrado.");

            return BaseResult<BankAccount>.CreateValidResult(entity);
        }

        public async Task<BaseResult<List<BankAccount>>> GetAllAsync(Guid userId)
        {
            return BaseResult<List<BankAccount>>.CreateValidResult(await _repository.FindAsync(x => x.UserId == userId));
        }

        public async Task<BaseResult<BankAccount>> CreateAsync(BankAccount bank)
        {
            bank.Id = Guid.NewGuid();
            return BaseResult<BankAccount>.CreateValidResult(await _repository.CreateAsync(bank));
        }

        public async Task<BaseResult<BankAccount>> UpdateAsync(BankAccount bank)
        {
            var result = await _repository.FindAsync(x => x.UserId == bank.UserId && x.Id == bank.Id);
            var entity = result?.FirstOrDefault();

            if (entity == null)
                return BaseResult<BankAccount>.CreateInvalidResult(message: $"Registro não encontrado");

            entity.Name = bank.Name;
            entity.UserId = bank.UserId;
            entity.IsMain = bank.IsMain;
            entity.Type = bank.Type;

            return BaseResult<BankAccount>.CreateValidResult(await _repository.UpdateAsync(entity));
        }

        public async Task<BaseResult<bool>> WithDrawAsync(Guid userId, Guid id, decimal amount)
        {
            var result = await _repository.FindAsync(x => x.UserId == userId && x.Id == id);
            var entity = result?.FirstOrDefault();

            if (entity == null)
                return BaseResult<bool>.CreateInvalidResult(message: $"Não foi possível debitar o valor: {amount.ConvertToBrazilianReal()}");
            entity.WithDraw(amount);

            await _repository.UpdateAsync(entity);

            return BaseResult<bool>.CreateValidResult(true);
        }

        public async Task<BaseResult<bool>> DepositAsync(Guid userId, Guid id, decimal amount)
        {
            var result = await _repository.FindAsync(x => x.UserId == userId && x.Id == id);
            var entity = result?.FirstOrDefault();
            if (entity == null)
                return BaseResult<bool>.CreateInvalidResult(message: $"Não foi possível incluir o valor: {amount.ConvertToBrazilianReal()}");

            entity.Deposit(amount);

            await _repository.UpdateAsync(entity);

            return BaseResult<bool>.CreateValidResult(true);
        }

        public async Task<BaseResult<bool>> DeleteAsync(Guid userId, Guid id)
        {
            var result = await _repository.FindAsync(x => x.UserId == userId && x.Id == id);
            var entity = result?.FirstOrDefault();
            if (entity == null)
                return BaseResult<bool>.CreateInvalidResult(message: $"Não foi possível excluir conta");

            await _repository.DeleteAsync(entity.Id);

            return BaseResult<bool>.CreateValidResult(true);
        }
    }
}
