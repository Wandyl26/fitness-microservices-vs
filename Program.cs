using Microsoft.EntityFrameworkCore;
using UserService.Application.UseCase;
using UserService.Domain.Repository;
using UserService.Infrastructure.Persistence;
using UserService.Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Configuración de MySQL y Pool de conexiones
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Inyección de dependencias explícita
builder.Services.AddScoped<IUserRepository, UserRepositoryAdapter>();
builder.Services.AddScoped<RegisterUserUseCase>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run("http://localhost:8081");