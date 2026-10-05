using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIREX.Domain.Entities;

namespace SIREX.Infrastructure.Persistence.Configurations;

public class EstudianteConfiguration : IEntityTypeConfiguration<Estudiante>
{
    public void Configure(EntityTypeBuilder<Estudiante> builder)
    {
        builder.ToTable("Estudiantes");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Codigo).HasMaxLength(20).IsRequired();
        builder.HasIndex(e => e.Codigo).IsUnique();
        builder.Property(e => e.Nombres).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Apellidos).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Documento).HasMaxLength(30).IsRequired();
        builder.Property(e => e.Email).HasMaxLength(150);
        builder.Property(e => e.Telefono).HasMaxLength(30);
    }
}
