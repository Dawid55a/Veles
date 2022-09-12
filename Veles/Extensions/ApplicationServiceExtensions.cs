using Microsoft.EntityFrameworkCore;
using VelesAPI.DbContext;
using VelesAPI.Helpers;
using VelesAPI.Interfaces;
using VelesAPI.Services;

namespace VelesAPI.Extensions;

public static class ApplicationServiceExtensions

{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

        services.AddDbContext<ChatDataContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("ChatDb"));
        });

        return services;
    }
}
