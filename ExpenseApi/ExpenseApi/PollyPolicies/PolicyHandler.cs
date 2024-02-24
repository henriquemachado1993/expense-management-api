using Polly;
using Polly.Extensions.Http;

namespace ExpenseApi.PollyPolicies
{
    public static class PolicyHandler
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount) => HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));


        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(int exceptionsAllowedBeforeBreaking,
            int durationOfBreakInSeconds) => HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(exceptionsAllowedBeforeBreaking, TimeSpan.FromSeconds(durationOfBreakInSeconds));
    }
}
