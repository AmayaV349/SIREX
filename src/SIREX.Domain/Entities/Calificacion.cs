namespace SIREX.Domain.Entities;

public class Calificacion
{
    public int Id { get; set; }
    public int MatriculaId { get; set; }
    public Matricula Matricula { get; set; } = null!;
    public decimal NotaFinal { get; set; }
    public string? Observaciones { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
}
