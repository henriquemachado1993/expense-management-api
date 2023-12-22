using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Infra.Auth;
using ExpenseApi.Infra.Context;
using ExpenseApi.Infra.Repositories;
using ExpenseApi.Service;
using ExpenseApi.Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApi.Infra.Dependencies
{
    public class DependenciesInjector
    {
        public static void Register(IServiceCollection svcCollection)
        {
            // Context
            svcCollection.AddScoped<MongoDBContext>();
            
            // Repositories
            svcCollection.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            // Services
            svcCollection.AddScoped<ITransactionService, TransactionService>();
            svcCollection.AddScoped<IBankAccountService, BankAccountService>();
            svcCollection.AddScoped<IUserService, UserService>();

            // Auth
            svcCollection.AddScoped<IAuthService, AuthService>();
            svcCollection.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
        }
    }
}
