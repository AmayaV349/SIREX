using Microsoft.EntityFrameworkCore;
using SIREX.Domain.Entities;

namespace SIREX.Infrastructure.Persistence;

public static class DbInitializer
{
    public static void MigrateAndSeed(SirexDbContext db, bool seedDemoData = true)
    {
        db.Database.Migrate();

        if (!seedDemoData || db.Docentes.Any())
            return;

        var docente1 = new Docente
        {
            Codigo = "D001",
            Nombres = "María",
            Apellidos = "García",
            Especialidad = "Matemáticas"
        };
        var docente2 = new Docente
        {
            Codigo = "D002",
            Nombres = "Carlos",
            Apellidos = "López",
            Especialidad = "Informática"
        };
        db.Docentes.AddRange(docente1, docente2);
        db.SaveChanges();

        db.Cursos.AddRange(
            new Curso
            {
                Codigo = "MAT101",
                Nombre = "Álgebra I",
                Creditos = 4,
                DocenteId = docente1.Id,
                Periodo = "2026-1",
                Cupo = 30
            },
            new Curso
            {
                Codigo = "INF201",
                Nombre = "Programación I",
                Creditos = 5,
                DocenteId = docente2.Id,
                Periodo = "2026-1",
                Cupo = 25
            },
            new Curso
            {
                Codigo = "MAT102",
                Nombre = "Cálculo I",
                Creditos = 4,
                DocenteId = docente1.Id,
                Periodo = "2026-1",
                Cupo = 30
            });

        db.SaveChanges();
    }
}
