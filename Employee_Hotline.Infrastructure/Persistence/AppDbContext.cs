using Employee_Hotline.Application.Interfaces;
using Employee_Hotline.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Employee_Hotline.InfraStructure.Persistence;

public sealed class AppDbContext : DbContext, IUnitOfWork
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Employee> Employees => Set<Employee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
}