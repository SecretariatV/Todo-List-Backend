using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using TodoBackend.Data;
using TodoBackend.Middlewares;
using TodoBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Add Swagger
builder.Services.AddSwaggerGen();

// Configure DbContext with MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")))
);

// Register Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITodoService, todoService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure development-only middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseCors();
app.UseRouting();

// Custom Middlewares
app.UseMiddleware<AuthMiddleware>();

app.MapGet("/health", () => Results.Json(new { msg = "Hello Get Zell" }));
app.MapControllers();

var port = builder.Configuration.GetValue<int>("Port");
app.Run($"http://0.0.0.0:{port}");