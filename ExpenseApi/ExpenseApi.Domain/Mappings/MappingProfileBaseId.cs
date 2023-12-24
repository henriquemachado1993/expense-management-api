using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApi.Domain.Mappings
{
    public class MappingProfileBaseId : Profile
    {
        public MappingProfileBaseId()
        {
            CreateMap<Guid?, Guid>().ConvertUsing(src => src ?? Guid.Empty);
        }
    }
}
