using Agent_Management_Server.Connect;
using Agent_Management_Server.Interface;
using Agent_Management_Server.models;
using Agent_Management_Server.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<Dbcontext>(options => options.UseSqlServer
    (connectionString));

builder.Services.AddScoped<Iservic<Target>, MyServiceTarget>();
builder.Services.AddScoped<Iservic<Agent>, MyServiceAgent>(); 
builder.Services.AddScoped<Iservic<Mission>, Mission_Menager_service>();
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
