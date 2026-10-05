namespace SIREX.Domain.Entities;

public class Docente
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string? Especialidad { get; set; }
    public bool Activo { get; set; } = true;

    public ICollection<Curso> Cursos { get; set; } = new List<Curso>();

    public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();
}
