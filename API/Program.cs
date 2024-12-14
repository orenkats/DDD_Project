using Domain.Interfaces;
using Infrastructure.Messaging;
using Infrastructure.Persistence;
using Application.Services;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Register AppDbContext for persistence
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 39)) // Replace with your MySQL version
    ));

// Register repositories (Infrastructure Layer)
builder.Services.AddScoped<ITraderRepository, TraderRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Register services (Application Layer)
builder.Services.AddScoped<ITraderService, TraderService>();

// Register RabbitMQ services
builder.Services.AddSingleton<IMessagingPublisher>(sp =>
    new RabbitMqPublisher(
        hostname: "localhost",
        username: "guest",
        password: "guest"
    ));

builder.Services.AddSingleton<IMessagingConsumer>(sp =>
    new RabbitMqConsumer(
        hostname: "localhost",
        username: "guest",
        password: "guest"
    ));

var app = builder.Build();

// Map controllers
app.MapControllers();
app.MapGet("/", () => "Welcome to the RabbitMQ API!");

app.Run();
