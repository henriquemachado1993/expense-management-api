using BeireMKit.Data.Interfaces.MongoDB;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Infra.Context;
using ExpenseApi.Infra.PollyPolicies;
using ExpenseApi.Service;
using ExpenseApi.Service.Service;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseApi.Infra.Dependencies
{
    public class DependenciesInjector
    {
        public static void Register(IServiceCollection svcCollection)
        {
            // Poly configurations
            svcCollection.AddHttpClient<IHttpBinService, HttpBinService>()
                .AddPolicyHandler(PolicyHandler.GetRetryPolicy(retryCount: 3))
                .AddPolicyHandler(PolicyHandler.GetCircuitBreakerPolicy(exceptionsAllowedBeforeBreaking: 5, durationOfBreakInSeconds: 30));

            // Services
            svcCollection.AddScoped<ITransactionService, TransactionService>();
            svcCollection.AddScoped<ITransactionCategoryService, TransactionCategoryService>();
            svcCollection.AddScoped<IBankAccountService, BankAccountService>();
            svcCollection.AddScoped<IUserService, UserService>();

            // Auth
            svcCollection.AddScoped<IAuthService, AuthService>();
        }
    }
}
