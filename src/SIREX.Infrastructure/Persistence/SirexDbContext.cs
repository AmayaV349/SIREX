using Microsoft.EntityFrameworkCore;
using SIREX.Domain.Entities;

namespace SIREX.Infrastructure.Persistence;

public class SirexDbContext : DbContext
{
    public SirexDbContext(DbContextOptions<SirexDbContext> options)
        : base(options)
    {
    }

    public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
    public DbSet<Docente> Docentes => Set<Docente>();
    public DbSet<Curso> Cursos => Set<Curso>();
    public DbSet<Matricula> Matriculas => Set<Matricula>();
    public DbSet<Calificacion> Calificaciones => Set<Calificacion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SirexDbContext).Assembly);
    }
}
