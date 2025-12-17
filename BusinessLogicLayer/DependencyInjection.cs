using BusinessLogicLayer.Mappers;
using BusinessLogicLayer.ServiceContracts;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.Validators;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using BusinessLogicLayer.RabbitMQ;



namespace BusinessLogicLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IOrdersService, OrdersService>();
            services.AddTransient<IRabbitMQProductNameUpdateConsumer,RabbitMQProductNameUpdateConsumer>();
            services.AddHostedService<RabbitMQProductNameUpdateHostedService>();

            services.AddValidatorsFromAssemblyContaining<OrderAddRequestValidator>();
     
            services.AddAutoMapper(cfg => {
                cfg.AddProfile<OrderAddRequestToOrderMappingProfile>();
            });
            services.AddAutoMapper(cfg => {
                cfg.AddProfile<OrderItemAddRequestToOrderItemMappingProfile>();
            });
            services.AddAutoMapper(cfg => {
                cfg.AddProfile<OrderItemToOrderItemResponseMappingProfile>();
            });
            services.AddAutoMapper(cfg => {
                cfg.AddProfile<OrderItemUpdateRequestToOrderItemMappingProfile>();
            });
            services.AddAutoMapper(cfg => {
                cfg.AddProfile<OrderUpdateRequestToOrderMappingProfile>();
            });
            services.AddAutoMapper(cfg => {
                cfg.AddProfile<UserDTOToOrderResponseMappingProfile>();
            });
            services.AddAutoMapper(cfg => {
                cfg.AddProfile<OrderToOrderResponseMappingProfile>();
            });
            services.AddAutoMapper(cfg => {
                cfg.AddProfile<ProductDTOToOrderItemResponseMappingProfile>();
            });

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379"; 
                options.InstanceName = "MyAppCache_";     
            });

            return services;
        }
    }
}
