using SIREX.Application.Printing;

namespace SIREX.Application.Abstractions;

public interface IReporteImpresionService
{
    string NombreInstitucion { get; }
    Task<InformeDocumento> CrearListadoEstudiantesAsync(CancellationToken ct = default);
    Task<InformeDocumento> CrearConstanciaMatriculaAsync(int estudianteId, CancellationToken ct = default);
    Task<InformeDocumento> CrearActaCalificacionesAsync(int cursoId, CancellationToken ct = default);
}
