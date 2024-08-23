using CharlieApi.Models.EFModels;
using CharlieApi.EFService;
using Microsoft.EntityFrameworkCore;
using CharlieApi.DapperService;
using CharlieApi.Service;

const string Connstring = "DefaultConnection";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? connectionString = builder.Configuration.GetConnectionString(Connstring);

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

// Mssql2022dbContext
builder.Services.AddDbContext<Mssql2022dbContext>(options =>
    options.UseSqlServer(connectionString));

//DapperUserService
builder.Services.AddScoped<DapperUserService>(provider =>
{
    return new DapperUserService(connectionString);
});

// 添加 HttpClient 和 QuestionAnswerService
builder.Services.AddHttpClient<QuestionAnswerService>();

// Add UserService to the container
builder.Services.AddScoped<EFUserService>();

// Add services to the container
builder.Services.AddControllers();

// cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
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

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
