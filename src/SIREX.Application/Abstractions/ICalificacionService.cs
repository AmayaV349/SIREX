using SIREX.Domain.Entities;

namespace SIREX.Application.Abstractions;

public interface ICalificacionService
{
    Task<IReadOnlyList<Calificacion>> ListarPorCursoAsync(int cursoId, CancellationToken ct = default);
    Task<Calificacion?> ObtenerPorMatriculaAsync(int matriculaId, CancellationToken ct = default);
    Task GuardarAsync(int matriculaId, decimal notaFinal, string? observaciones, CancellationToken ct = default);
}
