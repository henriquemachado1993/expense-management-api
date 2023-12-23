using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Domain.Interfaces.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApi.Service.Commands
{
    public class BankAccountWithDrawCommand : ICommand
    {
        private readonly IBankAccountService _service;
        private readonly decimal _amount;
        private readonly Guid _userId;

        public BankAccountWithDrawCommand(IBankAccountService service, Guid userId, decimal amount)
        {
            _service = service;
            _amount = amount;
            _userId = userId;
        }

        public async Task ExecuteAsync()
        {
            var bankAccounts = await _service.GetAllAsync(_userId);
            var bankAccount = bankAccounts.Data.FirstOrDefault(x => x.IsMain);

            if(bankAccount == null)
                bankAccount = bankAccounts.Data.FirstOrDefault(x => !x.IsMain);
            if(bankAccount != null)
            {
                await _service.WithDrawAsync(_userId, bankAccount.Id, _amount);
            }
        }

        public async Task UndoAsync()
        {
            var bankAccounts = await _service.GetAllAsync(_userId);
            var bankAccount = bankAccounts.Data.FirstOrDefault(x => x.IsMain);

            if (bankAccount == null)
                bankAccount = bankAccounts.Data.FirstOrDefault();
            if (bankAccount != null)
            {
                await _service.DepositAsync(_userId, bankAccount.Id, _amount);
            }
        }
    }
}
