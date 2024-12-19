using Application.Interfaces;
using Infrastructure.Messaging;
using Application.Services;
using Application.Validations;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Amazon.S3;
using Amazon.Extensions.NETCore.Setup;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Register AppDbContext for persistence
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 39)) // Replace with your MySQL version
    ));

// Register services (Application Layer)
builder.Services.AddScoped<ITraderManagement, TraderService>();
builder.Services.AddScoped<ITraderActions, TraderActionsService>();
builder.Services.AddScoped<IFileService, FileService>();

// Register repositories (Infrastructure Layer)
builder.Services.AddScoped<ITraderRepository, TraderRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Register RabbitMQ publisher
builder.Services.AddSingleton<IMessagingPublisher>(sp =>
    new RabbitMqPublisher(
        hostname: "localhost",
        username: "guest",
        password: "guest"
    ));

// Register RabbitMQ consumer for notifications
builder.Services.AddSingleton<IMessagingConsumer>(sp =>
    new RabbitMqConsumer(
        hostname: "localhost",
        username: "guest",
        password: "guest"
    ));

// Register S3 services
builder.Services.AddSingleton<IFileStorageService, S3FileStorageService>();

// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<TraderValidator>();

builder.Services.Configure<AWSOptions>(builder.Configuration.GetSection("AWS"));
builder.Services.AddAWSService<IAmazonS3>();

var app = builder.Build();

// Configure HTTP request pipeline
app.MapControllers();
app.MapGet("/", () => "Welcome to the RabbitMQ API!");
app.Run();
