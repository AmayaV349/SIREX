using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIREX.Domain.Entities;

namespace SIREX.Infrastructure.Persistence.Configurations;

public class MatriculaConfiguration : IEntityTypeConfiguration<Matricula>
{
    public void Configure(EntityTypeBuilder<Matricula> builder)
    {
        builder.ToTable("Matriculas");
        builder.HasKey(m => m.Id);
        builder.HasIndex(m => new { m.EstudianteId, m.CursoId }).IsUnique();
        builder.HasOne(m => m.Estudiante)
            .WithMany(e => e.Matriculas)
            .HasForeignKey(m => m.EstudianteId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Curso)
            .WithMany(c => c.Matriculas)
            .HasForeignKey(m => m.CursoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
