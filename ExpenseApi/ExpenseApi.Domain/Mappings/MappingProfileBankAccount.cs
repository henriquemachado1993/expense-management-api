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
            CreateMap<BankAccount, BankAccountRequestModel>()
                .ForMember(dest => dest.Id ?? Guid.Empty, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId ?? Guid.Empty, opt => opt.MapFrom(src => src.UserId))
                .ReverseMap();
        }
    }
}
