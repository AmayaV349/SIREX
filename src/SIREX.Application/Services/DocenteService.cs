using Microsoft.EntityFrameworkCore;
using SIREX.Application.Abstractions;
using SIREX.Application.Common;
using SIREX.Domain.Entities;
using SIREX.Infrastructure.Persistence;

namespace SIREX.Application.Services;

public class DocenteService(SirexDbContext db) : IDocenteService
{
    public async Task<IReadOnlyList<Docente>> ListarAsync(string? busqueda = null, CancellationToken ct = default)
    {
        var query = db.Docentes.AsNoTracking().OrderBy(d => d.Apellidos).ThenBy(d => d.Nombres);
        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var term = busqueda.Trim();
            query = query.Where(d =>
                    d.Codigo.Contains(term) ||
                    d.Nombres.Contains(term) ||
                    d.Apellidos.Contains(term))
                .OrderBy(d => d.Apellidos);
        }

        return await query.ToListAsync(ct);
    }

    public Task<Docente?> ObtenerPorIdAsync(int id, CancellationToken ct = default) =>
        db.Docentes.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id, ct);

    public async Task<Docente> CrearAsync(Docente docente, CancellationToken ct = default)
    {
        Validar(docente);
        if (await db.Docentes.AnyAsync(d => d.Codigo == docente.Codigo, ct))
            throw new BusinessException($"Ya existe un docente con código {docente.Codigo}.");

        db.Docentes.Add(docente);
        await db.SaveChangesAsync(ct);
        return docente;
    }

    public async Task ActualizarAsync(Docente docente, CancellationToken ct = default)
    {
        Validar(docente);
        var existing = await db.Docentes.FindAsync([docente.Id], ct)
            ?? throw new BusinessException("Docente no encontrado.");

        if (await db.Docentes.AnyAsync(d => d.Codigo == docente.Codigo && d.Id != docente.Id, ct))
            throw new BusinessException($"Ya existe otro docente con código {docente.Codigo}.");

        existing.Codigo = docente.Codigo;
        existing.Nombres = docente.Nombres;
        existing.Apellidos = docente.Apellidos;
        existing.Especialidad = docente.Especialidad;
        existing.Activo = docente.Activo;
        await db.SaveChangesAsync(ct);
    }

    public async Task EliminarAsync(int id, CancellationToken ct = default)
    {
        if (await db.Cursos.AnyAsync(c => c.DocenteId == id, ct))
            throw new BusinessException("No se puede eliminar el docente porque tiene cursos asignados.");

        var entity = await db.Docentes.FindAsync([id], ct)
            ?? throw new BusinessException("Docente no encontrado.");
        db.Docentes.Remove(entity);
        await db.SaveChangesAsync(ct);
    }

    private static void Validar(Docente d)
    {
        if (string.IsNullOrWhiteSpace(d.Codigo))
            throw new BusinessException("El código del docente es obligatorio.");
        if (string.IsNullOrWhiteSpace(d.Nombres))
            throw new BusinessException("Los nombres son obligatorios.");
        if (string.IsNullOrWhiteSpace(d.Apellidos))
            throw new BusinessException("Los apellidos son obligatorios.");
    }
}
