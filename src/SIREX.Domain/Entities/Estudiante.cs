namespace SIREX.Domain.Entities;

public class Estudiante
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Documento { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public bool Activo { get; set; } = true;

    public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();

    public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();
}
