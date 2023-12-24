using AutoMapper;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Models.Transaction;
using ExpenseApi.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApi.Domain.Mappings
{
    public class MappingProfileTransaction : Profile
    {
        public MappingProfileTransaction()
        {
            CreateMap<Transaction, TransactionRequestModel>()
                .ForMember(dest => dest.Id ?? Guid.Empty, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId ?? Guid.Empty, opt => opt.MapFrom(src => src.UserId))
                .ReverseMap();
        }
    }
}
