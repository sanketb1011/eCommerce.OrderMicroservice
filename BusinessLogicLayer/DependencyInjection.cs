using BusinessLogicLayer.ServiceContracts;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.Validators;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;



namespace BusinessLogicLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IOrdersService, OrdersService>();
            services.AddValidatorsFromAssemblyContaining<OrderAddRequestValidator>();
            return services;
        }
    }
}
