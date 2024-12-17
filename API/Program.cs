using Application.Interfaces;
using Application.Services;
using Application.Validations;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Persistence;
using Infrastructure.Services;
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
builder.Services.AddScoped<ITraderManagement, TraderManagementService>();
builder.Services.AddScoped<ITraderActions, TraderActionsService>();
builder.Services.AddScoped<IFileService, FileService>();

// Register S3 services
builder.Services.AddSingleton<IFileStorageService, S3FileStorageService>();

// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<TraderValidator>();

var app = builder.Build();

// Configure HTTP request pipeline
app.MapControllers();
app.Run();
