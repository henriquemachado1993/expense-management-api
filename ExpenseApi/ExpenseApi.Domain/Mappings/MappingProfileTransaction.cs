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
            CreateMap<Transaction, TransactionRequestModel>().ReverseMap();
            CreateMap<TransactionCategory, TransactionCategoryRequestModel>().ReverseMap();
        }
    }
}
