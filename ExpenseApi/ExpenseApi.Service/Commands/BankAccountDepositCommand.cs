﻿using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Domain.Interfaces.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApi.Service.Commands
{
    public class BankAccountDepositCommand : ICommand
    {
        private readonly IBankAccountService _service;
        private readonly decimal _amount;
        private readonly string _userId;

        public BankAccountDepositCommand(IBankAccountService service, string userId, decimal amount)
        {
            _service = service;
            _amount = amount;
            _userId = userId;
        }

        public async Task ExecuteAsync()
        {
            var bankAccounts = await _service.GetAllAsync(_userId);
            var bankAccount = bankAccounts.Data.FirstOrDefault(x => x.IsMain);

            if (bankAccount == null)
                bankAccount = bankAccounts.Data.FirstOrDefault();
            if (bankAccount != null)
            {
                await _service.DepositAsync(bankAccount.Id.ToString(), _amount);
            }
        }

        public async Task UndoAsync()
        {
            var bankAccounts = await _service.GetAllAsync(_userId);
            var bankAccount = bankAccounts.Data.FirstOrDefault(x => x.IsMain);

            if (bankAccount == null)
                bankAccount = bankAccounts.Data.FirstOrDefault(x => !x.IsMain);
            if (bankAccount != null)
            {
                await _service.WithDrawAsync(bankAccount.Id.ToString(), _amount);
            }
        }
    }
}
