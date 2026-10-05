using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIREX.Domain.Entities;

namespace SIREX.Infrastructure.Persistence.Configurations;

public class CursoConfiguration : IEntityTypeConfiguration<Curso>
{
    public void Configure(EntityTypeBuilder<Curso> builder)
    {
        builder.ToTable("Cursos");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Codigo).HasMaxLength(20).IsRequired();
        builder.HasIndex(c => c.Codigo).IsUnique();
        builder.Property(c => c.Nombre).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Periodo).HasMaxLength(20).IsRequired();
        builder.HasOne(c => c.Docente)
            .WithMany(d => d.Cursos)
            .HasForeignKey(c => c.DocenteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
