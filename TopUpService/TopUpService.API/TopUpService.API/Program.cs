using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TopUpService.API.Middleware;
using TopUpService.Data;
using TopUpService.Data.Repository;
using TopUpService.Database;
using TopUpService.DTO.Request;
using TopUpService.DTO.RequestValidators;
using TopUpService.Logic;
using TopUpService.Logic.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Added In Memory Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("DatabaseName")));
builder.Services.AddScoped<ApplicationDbContext>();

//Add Validators
builder.Services.AddScoped<IValidator<AddBeneficiaryRequest>, AddBeneficiaryRequestValidator>();
builder.Services.AddScoped<IValidator<TopUpRequest>, TopUpRequestValidator>();

//Add Dependencies
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBeneficiaryRepository, BeneficiaryRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUserTopUpService, UserTopUpService>();
builder.Services.AddScoped<IUserTransactionHttpService, UserTransactionHttpService>();


builder.Services.AddTransient<ErrorHandlingMiddleware>();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<UserTransactionHttpService>();


builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "Top Up Service API", Version = "v1" }));
builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

builder.Logging.ClearProviders();
builder.Logging.AddConsole();



var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("MyPolicy");

app.UseMiddleware<ErrorHandlingMiddleware>();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
