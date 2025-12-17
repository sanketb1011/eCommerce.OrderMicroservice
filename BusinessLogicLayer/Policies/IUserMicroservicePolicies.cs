using Polly;

namespace BusinessLogicLayer.Policies
{
    public interface IUserMicroservicePolicies
    {
        IAsyncPolicy<HttpResponseMessage> GetRetryPolicy();
        IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy();
        IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy();
        IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy();
    }
}
