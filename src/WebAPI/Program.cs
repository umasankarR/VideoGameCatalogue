using System.Reflection;
using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using WebAPI.Extensions;
using WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configure Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Infrastructure")));

// Register MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyReference).Assembly));

// Register Repositories and Unit of Work
builder.Services.AddScoped<IVideoGameRepository, VideoGameRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register Global Exception Handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Register Endpoints (Minimal API)
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

// Configure OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS (for frontend integration)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Use Global Exception Handler (must be early in pipeline)
app.UseExceptionHandler();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Video Game Catalogue API v1");
        options.RoutePrefix = "swagger"; // Access Swagger UI at /swagger
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

// Map all endpoints
app.MapEndpoints();

app.Run();
