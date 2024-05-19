using BeireMKit.Domain.BaseModels;
using ExpenseApi.Domain.Entities;

namespace ExpenseApi.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<BaseResult<User>> AuthenticateAsync(string email, string password);
    }
}
