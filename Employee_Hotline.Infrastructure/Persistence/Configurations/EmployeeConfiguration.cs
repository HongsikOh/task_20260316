using Employee_Hotline.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Employee_Hotline.InfraStructure.Persistence.Configurations;

public sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employee");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasIndex(e => e.Name)
               .IsUnique()
               .HasDatabaseName("IX_Employee_Name");

        builder.Property(e => e.Email)
               .IsRequired()
               .HasMaxLength(200);

        builder.HasIndex(e => e.Email)
               .IsUnique();

        builder.Property(e => e.Tel)
               .IsRequired()
               .HasMaxLength(11);

        builder.HasIndex(e => e.Tel)
               .IsUnique();

        builder.Property(e => e.JoinedAt)
                .IsRequired();
    }
}