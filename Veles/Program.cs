using System.Diagnostics;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using VelesAPI.Hubs;
using VelesAPI;
using VelesAPI.DbContext;
using ConfigurationManager = Microsoft.Extensions.Configuration.ConfigurationManager;

namespace VelesAPI
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            var app = CreateHostBuilder(args).Build();

            /*var cs = "Host=localhost;Username=postgres;Password=postgres;Database=postgres";

            using var con = new NpgsqlConnection(cs);
            con.Open();

            var sql = "SELECT version()";

            using var cmd = new NpgsqlCommand(sql, con);

            var version = cmd.ExecuteScalar()!.ToString();
            Debug.WriteLine($"PostgreSQL version: {version}");*/

            await app.Seed(); //runs function Seed() from DataSeeder 
                
            await app.RunAsync();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}