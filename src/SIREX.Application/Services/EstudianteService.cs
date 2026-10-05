using Microsoft.EntityFrameworkCore;
using SIREX.Application.Abstractions;
using SIREX.Application.Common;
using SIREX.Domain.Entities;
using SIREX.Infrastructure.Persistence;

namespace SIREX.Application.Services;

public class EstudianteService(SirexDbContext db) : IEstudianteService
{
    public async Task<IReadOnlyList<Estudiante>> ListarAsync(string? busqueda = null, CancellationToken ct = default)
    {
        var query = db.Estudiantes.AsNoTracking().OrderBy(e => e.Apellidos).ThenBy(e => e.Nombres);
        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var term = busqueda.Trim();
            query = query.Where(e =>
                    e.Codigo.Contains(term) ||
                    e.Nombres.Contains(term) ||
                    e.Apellidos.Contains(term) ||
                    e.Documento.Contains(term))
                .OrderBy(e => e.Apellidos);
        }

        return await query.ToListAsync(ct);
    }

    public Task<Estudiante?> ObtenerPorIdAsync(int id, CancellationToken ct = default) =>
        db.Estudiantes.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, ct);

    public async Task<Estudiante> CrearAsync(Estudiante estudiante, CancellationToken ct = default)
    {
        Validar(estudiante);
        if (await db.Estudiantes.AnyAsync(e => e.Codigo == estudiante.Codigo, ct))
            throw new BusinessException($"Ya existe un estudiante con código {estudiante.Codigo}.");

        db.Estudiantes.Add(estudiante);
        await db.SaveChangesAsync(ct);
        return estudiante;
    }

    public async Task ActualizarAsync(Estudiante estudiante, CancellationToken ct = default)
    {
        Validar(estudiante);
        var existing = await db.Estudiantes.FindAsync([estudiante.Id], ct)
            ?? throw new BusinessException("Estudiante no encontrado.");

        if (await db.Estudiantes.AnyAsync(e => e.Codigo == estudiante.Codigo && e.Id != estudiante.Id, ct))
            throw new BusinessException($"Ya existe otro estudiante con código {estudiante.Codigo}.");

        existing.Codigo = estudiante.Codigo;
        existing.Nombres = estudiante.Nombres;
        existing.Apellidos = estudiante.Apellidos;
        existing.Documento = estudiante.Documento;
        existing.Email = estudiante.Email;
        existing.Telefono = estudiante.Telefono;
        existing.Activo = estudiante.Activo;
        await db.SaveChangesAsync(ct);
    }

    public async Task EliminarAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.Estudiantes.FindAsync([id], ct)
            ?? throw new BusinessException("Estudiante no encontrado.");
        db.Estudiantes.Remove(entity);
        await db.SaveChangesAsync(ct);
    }

    private static void Validar(Estudiante e)
    {
        if (string.IsNullOrWhiteSpace(e.Codigo))
            throw new BusinessException("El código del estudiante es obligatorio.");
        if (string.IsNullOrWhiteSpace(e.Nombres))
            throw new BusinessException("Los nombres son obligatorios.");
        if (string.IsNullOrWhiteSpace(e.Apellidos))
            throw new BusinessException("Los apellidos son obligatorios.");
        if (string.IsNullOrWhiteSpace(e.Documento))
            throw new BusinessException("El documento es obligatorio.");
    }
}
