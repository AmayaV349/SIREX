using SIREX.Domain.Entities;

namespace SIREX.Application.Abstractions;

public interface IEstudianteService
{
    Task<IReadOnlyList<Estudiante>> ListarAsync(string? busqueda = null, CancellationToken ct = default);
    Task<Estudiante?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<Estudiante> CrearAsync(Estudiante estudiante, CancellationToken ct = default);
    Task ActualizarAsync(Estudiante estudiante, CancellationToken ct = default);
    Task EliminarAsync(int id, CancellationToken ct = default);
}
