using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SIREX.Infrastructure.Persistence;

namespace SIREX.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<SirexDbContext>(options =>
            options.UseSqlite(connectionString));

        return services;
    }
}
