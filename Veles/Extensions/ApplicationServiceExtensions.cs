using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VelesAPI.DbContext;
using VelesAPI.Interfaces;
using VelesAPI.Services;

namespace VelesAPI.Extensions
{
    public static class ApplicationServiceExtensions

    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            //services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            services.AddDbContext<ChatDataContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("ChatDb"));
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            });

            return services;
        }
        
    }

}
