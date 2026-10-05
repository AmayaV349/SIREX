using Microsoft.EntityFrameworkCore;
using SIREX.Application.Abstractions;
using SIREX.Application.Common;
using SIREX.Domain.Entities;
using SIREX.Infrastructure.Persistence;

namespace SIREX.Application.Services;

public class CursoService(SirexDbContext db) : ICursoService
{
    public async Task<IReadOnlyList<Curso>> ListarAsync(string? busqueda = null, CancellationToken ct = default)
    {
        var query = db.Cursos.AsNoTracking()
            .Include(c => c.Docente)
            .OrderBy(c => c.Periodo).ThenBy(c => c.Codigo);

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var term = busqueda.Trim();
            query = query.Where(c =>
                    c.Codigo.Contains(term) ||
                    c.Nombre.Contains(term) ||
                    c.Periodo.Contains(term))
                .OrderBy(c => c.Codigo);
        }

        return await query.ToListAsync(ct);
    }

    public Task<Curso?> ObtenerPorIdAsync(int id, CancellationToken ct = default) =>
        db.Cursos.AsNoTracking()
            .Include(c => c.Docente)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<Curso> CrearAsync(Curso curso, CancellationToken ct = default)
    {
        await ValidarAsync(curso, ct);
        if (await db.Cursos.AnyAsync(c => c.Codigo == curso.Codigo, ct))
            throw new BusinessException($"Ya existe un curso con código {curso.Codigo}.");

        db.Cursos.Add(curso);
        await db.SaveChangesAsync(ct);
        return curso;
    }

    public async Task ActualizarAsync(Curso curso, CancellationToken ct = default)
    {
        await ValidarAsync(curso, ct);
        var existing = await db.Cursos.FindAsync([curso.Id], ct)
            ?? throw new BusinessException("Curso no encontrado.");

        if (await db.Cursos.AnyAsync(c => c.Codigo == curso.Codigo && c.Id != curso.Id, ct))
            throw new BusinessException($"Ya existe otro curso con código {curso.Codigo}.");

        var matriculasActivas = await db.Matriculas.CountAsync(
            m => m.CursoId == curso.Id && m.Estado == Domain.Enums.EstadoMatricula.Activa, ct);
        if (curso.Cupo < matriculasActivas)
            throw new BusinessException($"El cupo no puede ser menor que las matrículas activas ({matriculasActivas}).");

        existing.Codigo = curso.Codigo;
        existing.Nombre = curso.Nombre;
        existing.Creditos = curso.Creditos;
        existing.DocenteId = curso.DocenteId;
        existing.Periodo = curso.Periodo;
        existing.Cupo = curso.Cupo;
        await db.SaveChangesAsync(ct);
    }

    public async Task EliminarAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.Cursos.FindAsync([id], ct)
            ?? throw new BusinessException("Curso no encontrado.");
        db.Cursos.Remove(entity);
        await db.SaveChangesAsync(ct);
    }

    private async Task ValidarAsync(Curso c, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(c.Codigo))
            throw new BusinessException("El código del curso es obligatorio.");
        if (string.IsNullOrWhiteSpace(c.Nombre))
            throw new BusinessException("El nombre del curso es obligatorio.");
        if (string.IsNullOrWhiteSpace(c.Periodo))
            throw new BusinessException("El periodo es obligatorio.");
        if (c.Creditos <= 0)
            throw new BusinessException("Los créditos deben ser mayores que cero.");
        if (c.Cupo <= 0)
            throw new BusinessException("El cupo debe ser mayor que cero.");
        if (!await db.Docentes.AnyAsync(d => d.Id == c.DocenteId, ct))
            throw new BusinessException("El docente seleccionado no existe.");
    }
}
