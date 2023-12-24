using AutoMapper;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Models.BankAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApi.Domain.Mappings
{
    public class MappingProfileBankAccount : Profile
    {
        public MappingProfileBankAccount()
        {
            CreateMap<BankAccount, BankAccountRequestModel>().ReverseMap();
        }
    }
}
