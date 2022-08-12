namespace VelesAPI;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Build server instance from configuration in startup.cs
        var app = CreateHostBuilder(args).Build();
        // Seeding database
        await app.Seed();
        // Running server
        await app.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
