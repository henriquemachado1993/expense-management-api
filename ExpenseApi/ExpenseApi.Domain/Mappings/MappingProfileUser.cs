using AutoMapper;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApi.Domain.Mappings
{
    public class MappingProfileUser : Profile
    {
        public MappingProfileUser()
        {
                CreateMap<User, UserRequestModel>().ReverseMap();
                CreateMap<Address, AddressModel>().ReverseMap();
        }
    }
}
