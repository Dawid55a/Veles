using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using VelesAPI.Hubs;
using Veles;
using VelesAPI;
using VelesAPI.DbContext;

namespace VelesAPI
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var app = CreateHostBuilder(args).Build();

            var cs = "Host=localhost;Username=postgres;Password=postgres;Database=postgres";

            using var con = new NpgsqlConnection(cs);
            con.Open();

            var sql = "SELECT version()";

            using var cmd = new NpgsqlCommand(sql, con);

            var version = cmd.ExecuteScalar()!.ToString();
            Debug.WriteLine($"PostgreSQL version: {version}");

            app.Seed(); //runs function Seed() from DataSeeder 

            app.Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}