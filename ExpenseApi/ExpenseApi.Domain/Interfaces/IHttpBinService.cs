using ExpenseApi.Domain.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApi.Domain.Interfaces
{
    /// <summary>
    /// Esse serviço tem a finalidade apenas de testar Resiliência em aplicações .Net com Polly
    /// </summary>
    public interface IHttpBinService
    {
        Task<ServiceResult<object>> GetAsync(int code);
    }
}
