using SIREX.Domain.Entities;

namespace SIREX.Application.Abstractions;

public interface IDocenteService
{
    Task<IReadOnlyList<Docente>> ListarAsync(string? busqueda = null, CancellationToken ct = default);
    Task<Docente?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<Docente> CrearAsync(Docente docente, CancellationToken ct = default);
    Task ActualizarAsync(Docente docente, CancellationToken ct = default);
    Task EliminarAsync(int id, CancellationToken ct = default);
}
