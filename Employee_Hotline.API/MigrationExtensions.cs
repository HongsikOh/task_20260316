using Employee_Hotline.InfraStructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Employee_Hotline.API;

public static class MigrationExtensions
{
    public static async Task MigrateDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await ctx.Database.MigrateAsync();
    }
}