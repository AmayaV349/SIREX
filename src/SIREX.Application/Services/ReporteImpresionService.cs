using Microsoft.EntityFrameworkCore;
using SIREX.Application.Abstractions;
using SIREX.Application.Common;
using SIREX.Application.Printing;
using SIREX.Domain.Enums;
using SIREX.Infrastructure.Persistence;

namespace SIREX.Application.Services;

public class ReporteImpresionService(SirexDbContext db, string nombreInstitucion) : IReporteImpresionService
{
    public string NombreInstitucion { get; } = nombreInstitucion;

    public async Task<InformeDocumento> CrearListadoEstudiantesAsync(CancellationToken ct = default)
    {
        var estudiantes = await db.Estudiantes.AsNoTracking()
            .OrderBy(e => e.Apellidos).ThenBy(e => e.Nombres)
            .ToListAsync(ct);

        var filas = estudiantes.Select(e => (IReadOnlyList<string>)new[]
        {
            e.Codigo,
            e.NombreCompleto,
            e.Documento,
            e.Activo ? "Activo" : "Inactivo"
        }).ToList();

        return new InformeDocumento
        {
            TituloInstitucion = NombreInstitucion,
            TituloReporte = "Listado de estudiantes",
            Subtitulo = $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}",
            EncabezadosColumnas = new[] { "Código", "Nombre", "Documento", "Estado" },
            Filas = filas
        };
    }

    public async Task<InformeDocumento> CrearConstanciaMatriculaAsync(int estudianteId, CancellationToken ct = default)
    {
        var estudiante = await db.Estudiantes.AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == estudianteId, ct)
            ?? throw new BusinessException("Estudiante no encontrado.");

        var matriculas = await db.Matriculas.AsNoTracking()
            .Include(m => m.Curso)
            .Where(m => m.EstudianteId == estudianteId && m.Estado == EstadoMatricula.Activa)
            .OrderBy(m => m.Curso.Periodo)
            .ThenBy(m => m.Curso.Codigo)
            .ToListAsync(ct);

        var filas = matriculas.Select(m => (IReadOnlyList<string>)new[]
        {
            m.Curso.Codigo,
            m.Curso.Nombre,
            m.Curso.Periodo,
            m.Fecha.ToLocalTime().ToString("dd/MM/yyyy")
        }).ToList();

        var parrafos = new List<string>
        {
            "CONSTANCIA DE MATRÍCULA",
            "",
            $"Se certifica que el(la) estudiante {estudiante.NombreCompleto}, código {estudiante.Codigo}, " +
            $"documento {estudiante.Documento}, se encuentra matriculado(a) en los siguientes cursos:"
        };

        return new InformeDocumento
        {
            TituloInstitucion = NombreInstitucion,
            TituloReporte = "Constancia de matrícula",
            Subtitulo = estudiante.NombreCompleto,
            Parrafos = parrafos,
            EncabezadosColumnas = new[] { "Código", "Curso", "Periodo", "Fecha matrícula" },
            Filas = filas
        };
    }

    public async Task<InformeDocumento> CrearActaCalificacionesAsync(int cursoId, CancellationToken ct = default)
    {
        var curso = await db.Cursos.AsNoTracking()
            .Include(c => c.Docente)
            .FirstOrDefaultAsync(c => c.Id == cursoId, ct)
            ?? throw new BusinessException("Curso no encontrado.");

        var matriculas = await db.Matriculas.AsNoTracking()
            .Include(m => m.Estudiante)
            .Include(m => m.Calificacion)
            .Where(m => m.CursoId == cursoId && m.Estado == EstadoMatricula.Activa)
            .OrderBy(m => m.Estudiante.Apellidos)
            .ToListAsync(ct);

        var filas = matriculas.Select(m => (IReadOnlyList<string>)new[]
        {
            m.Estudiante.Codigo,
            m.Estudiante.NombreCompleto,
            m.Calificacion?.NotaFinal.ToString("0.00") ?? "—",
            m.Calificacion?.Observaciones ?? ""
        }).ToList();

        return new InformeDocumento
        {
            TituloInstitucion = NombreInstitucion,
            TituloReporte = "Acta de calificaciones",
            Subtitulo = $"{curso.Codigo} — {curso.Nombre} | Periodo {curso.Periodo} | Docente: {curso.Docente.NombreCompleto}",
            EncabezadosColumnas = new[] { "Código", "Estudiante", "Nota", "Observaciones" },
            Filas = filas
        };
    }
}
