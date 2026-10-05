using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIREX.Domain.Entities;

namespace SIREX.Infrastructure.Persistence.Configurations;

public class CalificacionConfiguration : IEntityTypeConfiguration<Calificacion>
{
    public void Configure(EntityTypeBuilder<Calificacion> builder)
    {
        builder.ToTable("Calificaciones");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.NotaFinal).HasPrecision(5, 2);
        builder.Property(c => c.Observaciones).HasMaxLength(500);
        builder.HasIndex(c => c.MatriculaId).IsUnique();
        builder.HasOne(c => c.Matricula)
            .WithOne(m => m.Calificacion)
            .HasForeignKey<Calificacion>(c => c.MatriculaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
