using SIREX.Domain.Entities;

namespace SIREX.Application.Abstractions;

public interface ICursoService
{
    Task<IReadOnlyList<Curso>> ListarAsync(string? busqueda = null, CancellationToken ct = default);
    Task<Curso?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<Curso> CrearAsync(Curso curso, CancellationToken ct = default);
    Task ActualizarAsync(Curso curso, CancellationToken ct = default);
    Task EliminarAsync(int id, CancellationToken ct = default);
}
