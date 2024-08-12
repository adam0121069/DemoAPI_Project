using CharlieApi.Models.EFModels;
using CharlieApi.EFService;
using Microsoft.EntityFrameworkCore;
using CharlieApi.DapperService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

// 注册 DbContext
builder.Services.AddDbContext<Mssql2022dbContext>(options =>
    options.UseSqlServer(connectionString));

// 注册 UserRepository
builder.Services.AddScoped<DapperUser>(provider =>
{
    return new DapperUser(connectionString);
});

// Add UserService to the container
builder.Services.AddScoped<EFUserService>();

// Add services to the container
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Redirect root URL to Swagger UI
    app.Use(async (context, next) =>
    {
        if (context.Request.Path == "/")
        {
            context.Response.Redirect("/swagger/index.html", true);
        }
        else
        {
            await next();
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
