namespace SIREX.Application.Printing;

public class InformeDocumento
{
    public string TituloInstitucion { get; set; } = "SIREX - Sistema de Información Académica";
    public string TituloReporte { get; set; } = string.Empty;
    public string? Subtitulo { get; set; }
    public IReadOnlyList<string> EncabezadosColumnas { get; set; } = Array.Empty<string>();
    public IReadOnlyList<IReadOnlyList<string>> Filas { get; set; } = Array.Empty<IReadOnlyList<string>>();
    public IReadOnlyList<string> Parrafos { get; set; } = Array.Empty<string>();
}
