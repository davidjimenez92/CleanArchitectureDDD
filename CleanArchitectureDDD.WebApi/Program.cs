using CleanArchitectureDDD.Application.Configuration;
using CleanArchitectureDDD.Infrastructure.Configuration;
using CleanArchitectureDDD.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ApplyDatabaseMigrations();
app.SeedData();
app.UseCustomExceptionHandler();
app.MapControllers();

app.Run();
