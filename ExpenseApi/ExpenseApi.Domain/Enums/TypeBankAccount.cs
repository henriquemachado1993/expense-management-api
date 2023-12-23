using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApi.Domain.Enums
{
    public enum TypeBankAccount
    {
        [Display(Name = "Conta corrente")]
        CurrentAccount,
        [Display(Name = "Conta salário")]
        SalaryAccount,
        [Display(Name = "Conta poupança")]
        SavingsAccount, 
        [Display(Name = "Conta de Investimentos")]
        InvestimentsAccount,
    }
}
