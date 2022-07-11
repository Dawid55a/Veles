using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Veles;

var cs = "Host=localhost;Username=postgres;Password=postgres;Database=postgres";

using var con = new NpgsqlConnection(cs);
con.Open();

var sql = "SELECT version()";

using var cmd = new NpgsqlCommand(sql, con);

var version = cmd.ExecuteScalar().ToString();
Debug.WriteLine($"PostgreSQL version: {version}");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add database services to the container
builder.Services.AddDbContext<ChatDataContext>(
    o => o.UseNpgsql(builder.Configuration.GetConnectionString("ChatDb")));//takes connection string from Veles/appsettings.json

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Seed();//runs function Seed() from DataSeeder 

app.Run();
