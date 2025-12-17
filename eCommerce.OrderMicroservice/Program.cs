using BusinessLogicLayer;
using BusinessLogicLayer.HttpClients;
using BusinessLogicLayer.Policies;
using DataAccessLayer;
using eCommerce.OrderMicroservice;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBusinessLogicLayer(builder.Configuration);
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddTransient<IUserMicroservicePolicies, UserMicroservicePolicies>();
builder.Services.AddHttpClient<UsersMicroserviceClient>(client =>
{
    client.BaseAddress = new Uri($"http://localhost:5000");
})
    //.AddPolicyHandler(
    //    builder.Services.BuildServiceProvider()
    //                .GetRequiredService<IUserMicroservicePolicies>()
    //                .GetRetryPolicy())

    //.AddPolicyHandler(
    //    builder.Services.BuildServiceProvider()
    //                .GetRequiredService<IUserMicroservicePolicies>()
    //                .GetCircuitBreakerPolicy())
    //.AddPolicyHandler(
    //    builder.Services.BuildServiceProvider()
    //                .GetRequiredService<IUserMicroservicePolicies>()
    //                .GetTimeoutPolicy()
    //)
    .AddPolicyHandler(
    builder.Services.BuildServiceProvider()
   .GetRequiredService<IUserMicroservicePolicies>()
   .GetCombinedPolicy());

builder.Services.AddTransient<IProductsMicroservicePolicies, ProductsMicroservicePolicies>();
builder.Services.AddHttpClient<ProductsMicroserviceClient>(client => {
    client.BaseAddress = new Uri($"http://localhost:5000");
})
    //.AddPolicyHandler(
    //builder.Services.BuildServiceProvider()
    //                .GetRequiredService<IProductsMicroservicePolicies>()
    //                .GetFallbackPolicy()
    //)
    //.AddPolicyHandler(
    //builder.Services.BuildServiceProvider()
    //                .GetRequiredService<IProductsMicroservicePolicies>()
    //                .GetBulkheadIsolationPolicy()
    //);
    .AddPolicyHandler(
    builder.Services.BuildServiceProvider()
   .GetRequiredService<IProductsMicroservicePolicies>()
   .GetCombinedPolicy());

var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UseRouting();
app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
