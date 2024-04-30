using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TransactionService.API.Middleware;
using TransactionService.Data;
using TransactionService.Database;
using TransactionService.DTO.Request;
using TransactionService.DTO.RequestValidators;
using TransactionService.Logic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Added In Memory Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("DatabaseName")));
builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddScoped<IValidator<UserTransactionRequest>, UserTransactionRequestValidator>();

//Add Dependencies
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserTransactionService, UserTransactionService>();
builder.Services.AddTransient<ErrorHandlingMiddleware>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "Transaction Service API", Version = "v1" }));
builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

builder.Logging.ClearProviders();
builder.Logging.AddConsole();


var app = builder.Build();


app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseCors("MyPolicy");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
