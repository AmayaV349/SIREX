using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIREX.Domain.Entities;

namespace SIREX.Infrastructure.Persistence.Configurations;

public class DocenteConfiguration : IEntityTypeConfiguration<Docente>
{
    public void Configure(EntityTypeBuilder<Docente> builder)
    {
        builder.ToTable("Docentes");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Codigo).HasMaxLength(20).IsRequired();
        builder.HasIndex(d => d.Codigo).IsUnique();
        builder.Property(d => d.Nombres).HasMaxLength(100).IsRequired();
        builder.Property(d => d.Apellidos).HasMaxLength(100).IsRequired();
        builder.Property(d => d.Especialidad).HasMaxLength(150);
    }
}
