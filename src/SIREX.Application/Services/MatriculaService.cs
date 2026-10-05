using Microsoft.EntityFrameworkCore;
using SIREX.Application.Abstractions;
using SIREX.Application.Common;
using SIREX.Domain.Entities;
using SIREX.Domain.Enums;
using SIREX.Infrastructure.Persistence;

namespace SIREX.Application.Services;

public class MatriculaService(SirexDbContext db) : IMatriculaService
{
    public async Task<IReadOnlyList<Matricula>> ListarAsync(
        int? cursoId = null,
        int? estudianteId = null,
        CancellationToken ct = default)
    {
        var query = db.Matriculas.AsNoTracking()
            .Include(m => m.Estudiante)
            .Include(m => m.Curso)
            .AsQueryable();

        if (cursoId.HasValue)
            query = query.Where(m => m.CursoId == cursoId.Value);
        if (estudianteId.HasValue)
            query = query.Where(m => m.EstudianteId == estudianteId.Value);

        return await query.OrderByDescending(m => m.Fecha).ToListAsync(ct);
    }

    public Task<Matricula?> ObtenerPorIdAsync(int id, CancellationToken ct = default) =>
        db.Matriculas.AsNoTracking()
            .Include(m => m.Estudiante)
            .Include(m => m.Curso)
            .FirstOrDefaultAsync(m => m.Id == id, ct);

    public async Task<Matricula> CrearAsync(int estudianteId, int cursoId, CancellationToken ct = default)
    {
        if (!await db.Estudiantes.AnyAsync(e => e.Id == estudianteId && e.Activo, ct))
            throw new BusinessException("El estudiante no existe o está inactivo.");

        var curso = await db.Cursos.FindAsync([cursoId], ct)
            ?? throw new BusinessException("El curso no existe.");

        if (await db.Matriculas.AnyAsync(m => m.EstudianteId == estudianteId && m.CursoId == cursoId, ct))
            throw new BusinessException("El estudiante ya está matriculado en este curso.");

        var activas = await db.Matriculas.CountAsync(
            m => m.CursoId == cursoId && m.Estado == EstadoMatricula.Activa, ct);
        if (activas >= curso.Cupo)
            throw new BusinessException("El curso ha alcanzado el cupo máximo.");

        var matricula = new Matricula
        {
            EstudianteId = estudianteId,
            CursoId = cursoId,
            Fecha = DateTime.UtcNow,
            Estado = EstadoMatricula.Activa
        };
        db.Matriculas.Add(matricula);
        await db.SaveChangesAsync(ct);

        await db.Entry(matricula).Reference(m => m.Estudiante).LoadAsync(ct);
        await db.Entry(matricula).Reference(m => m.Curso).LoadAsync(ct);
        return matricula;
    }

    public async Task ActualizarEstadoAsync(int id, EstadoMatricula estado, CancellationToken ct = default)
    {
        var matricula = await db.Matriculas.FindAsync([id], ct)
            ?? throw new BusinessException("Matrícula no encontrada.");
        matricula.Estado = estado;
        await db.SaveChangesAsync(ct);
    }

    public async Task EliminarAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.Matriculas.FindAsync([id], ct)
            ?? throw new BusinessException("Matrícula no encontrada.");
        db.Matriculas.Remove(entity);
        await db.SaveChangesAsync(ct);
    }
}
