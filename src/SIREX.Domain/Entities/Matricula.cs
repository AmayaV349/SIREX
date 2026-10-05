using SIREX.Domain.Enums;

namespace SIREX.Domain.Entities;

public class Matricula
{
    public int Id { get; set; }
    public int EstudianteId { get; set; }
    public Estudiante Estudiante { get; set; } = null!;
    public int CursoId { get; set; }
    public Curso Curso { get; set; } = null!;
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public EstadoMatricula Estado { get; set; } = EstadoMatricula.Activa;

    public Calificacion? Calificacion { get; set; }
}
