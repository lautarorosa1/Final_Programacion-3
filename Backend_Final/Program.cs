using Backend_Final.Application.Services.Interfaces;
using Backend_Final.Application.Services;
using Backend_Final.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Backend_Final.Infrastructure.Repositories.Interfaces;
using Backend_Final.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Controllers + JSON config
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Services
builder.Services.AddScoped<IClienteService, ClienteService>();

// Repositories
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

var app = builder.Build();

// Middleware
app.UseHttpsRedirection();

app.UseCors("PermitirTodo");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();