
using BeireMKit.Domain.BaseModels;

namespace ExpenseApi.Domain.Interfaces
{
    /// <summary>
    /// Esse serviço tem a finalidade apenas de testar Resiliência em aplicações .Net com Polly
    /// </summary>
    public interface IHttpBinService
    {
        Task<BaseResult<object>> GetAsync(int code);
    }
}
