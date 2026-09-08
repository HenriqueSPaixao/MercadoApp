using Market.Domain.Implementations;
using Market.Domain.Interfaces;
using Market.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// 1. Busque a string de conexão ANTES e guarde em uma variável
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// (Opcional, mas recomendado) Uma trava de segurança para avisar se esquecemos de colocar no appsettings.json
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("A string de conexão 'DefaultConnection' não foi encontrada no appsettings.json!");
}

// 2. Agora passe a variável 'connectionString' em vez de chamar o 'builder' lá dentro
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddScoped<ICartServices, CartServices>();
builder.Services.AddScoped<IProductServices, ProductServices>();
builder.Services.AddScoped<ISectorServices, SectorServices>(); 
builder.Services.AddScoped<IBatchServices, BatchServices>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
