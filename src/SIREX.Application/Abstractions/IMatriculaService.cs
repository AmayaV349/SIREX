using SIREX.Domain.Entities;

namespace SIREX.Application.Abstractions;

public interface IMatriculaService
{
    Task<IReadOnlyList<Matricula>> ListarAsync(int? cursoId = null, int? estudianteId = null, CancellationToken ct = default);
    Task<Matricula?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<Matricula> CrearAsync(int estudianteId, int cursoId, CancellationToken ct = default);
    Task ActualizarEstadoAsync(int id, Domain.Enums.EstadoMatricula estado, CancellationToken ct = default);
    Task EliminarAsync(int id, CancellationToken ct = default);
}
