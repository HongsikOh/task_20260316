using Employee_Hotline.Application.Interfaces;
using Employee_Hotline.Application.Interfaces.Parser;
using Employee_Hotline.InfraStructure.Parsers;
using Employee_Hotline.InfraStructure.Persistence;
using Employee_Hotline.InfraStructure.Persistence.Repositories;
using Employee_Hotline.InfraStructure.ReadModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Employee_Hotline.InfraStructure.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opts =>
            opts.UseSqlite(configuration.GetConnectionString("SQLite"))
        );

        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<AppDbContext>());

        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IEmployeeReadRepository, EmployeeReadRepository>();

        services.AddScoped<IEmployeeFileParser, CsvEmployeeParser>();
        services.AddScoped<IEmployeeFileParser, JsonEmployeeParser>();

        return services;
    }
}