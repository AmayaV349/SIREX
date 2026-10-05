using Microsoft.EntityFrameworkCore;
using SIREX.Application.Abstractions;
using SIREX.Application.Common;
using SIREX.Domain.Entities;
using SIREX.Infrastructure.Persistence;

namespace SIREX.Application.Services;

public class CalificacionService(SirexDbContext db) : ICalificacionService
{
    public async Task<IReadOnlyList<Calificacion>> ListarPorCursoAsync(int cursoId, CancellationToken ct = default)
    {
        return await db.Calificaciones.AsNoTracking()
            .Include(c => c.Matricula)
            .ThenInclude(m => m.Estudiante)
            .Where(c => c.Matricula.CursoId == cursoId)
            .OrderBy(c => c.Matricula.Estudiante.Apellidos)
            .ToListAsync(ct);
    }

    public Task<Calificacion?> ObtenerPorMatriculaAsync(int matriculaId, CancellationToken ct = default) =>
        db.Calificaciones.AsNoTracking().FirstOrDefaultAsync(c => c.MatriculaId == matriculaId, ct);

    public async Task GuardarAsync(int matriculaId, decimal notaFinal, string? observaciones, CancellationToken ct = default)
    {
        if (notaFinal < 0 || notaFinal > 100)
            throw new BusinessException("La nota debe estar entre 0 y 100.");

        var matricula = await db.Matriculas.FindAsync([matriculaId], ct)
            ?? throw new BusinessException("Matrícula no encontrada.");

        var calificacion = await db.Calificaciones.FirstOrDefaultAsync(c => c.MatriculaId == matriculaId, ct);
        if (calificacion is null)
        {
            calificacion = new Calificacion { MatriculaId = matricula.Id };
            db.Calificaciones.Add(calificacion);
        }

        calificacion.NotaFinal = notaFinal;
        calificacion.Observaciones = observaciones;
        calificacion.FechaRegistro = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
    }
}
