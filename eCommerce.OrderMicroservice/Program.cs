
using FluentValidation.AspNetCore;
using BusinessLogicLayer;
using DataAccessLayer;
using eCommerce.OrderMicroservice;

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
