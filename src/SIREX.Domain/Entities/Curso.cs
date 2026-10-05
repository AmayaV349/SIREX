namespace SIREX.Domain.Entities;

public class Curso
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public int Creditos { get; set; }
    public int DocenteId { get; set; }
    public Docente Docente { get; set; } = null!;
    public string Periodo { get; set; } = string.Empty;
    public int Cupo { get; set; } = 30;

    public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
}
