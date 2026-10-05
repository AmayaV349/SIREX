using System.Drawing;
using System.Drawing.Printing;
using SIREX.Application.Printing;

namespace SIREX.App.Printing;

public sealed class InformePrintDocument : PrintDocument
{
    private readonly InformeDocumento _documento;
    private int _filaIndex;
    private int _pagina;

    public InformePrintDocument(InformeDocumento documento)
    {
        _documento = documento;
        DocumentName = documento.TituloReporte;
        DefaultPageSettings.Margins = new Margins(60, 60, 60, 60);
    }

    protected override void OnBeginPrint(PrintEventArgs e)
    {
        _filaIndex = 0;
        _pagina = 1;
        base.OnBeginPrint(e);
    }

    protected override void OnPrintPage(PrintPageEventArgs e)
    {
        var g = e.Graphics!;
        var margin = e.MarginBounds;
        float y = margin.Top;
        var titleFont = new Font("Segoe UI", 14, FontStyle.Bold);
        var headerFont = new Font("Segoe UI", 11, FontStyle.Bold);
        var bodyFont = new Font("Segoe UI", 10);
        var smallFont = new Font("Segoe UI", 9);

        g.DrawString(_documento.TituloInstitucion, titleFont, Brushes.Black, margin.Left, y);
        y += titleFont.GetHeight(g) + 4;
        g.DrawString(_documento.TituloReporte, headerFont, Brushes.Black, margin.Left, y);
        y += headerFont.GetHeight(g) + 4;

        if (!string.IsNullOrWhiteSpace(_documento.Subtitulo))
        {
            g.DrawString(_documento.Subtitulo, bodyFont, Brushes.Black, margin.Left, y);
            y += bodyFont.GetHeight(g) + 8;
        }

        foreach (var parrafo in _documento.Parrafos)
        {
            if (y > margin.Bottom - 40)
            {
                FinalizarPagina(e, smallFont, margin, ref y);
                return;
            }

            g.DrawString(parrafo, bodyFont, Brushes.Black, margin.Left, y);
            y += bodyFont.GetHeight(g) + 2;
        }

        if (_documento.EncabezadosColumnas.Count > 0)
        {
            y += 6;
            var columnas = _documento.EncabezadosColumnas.Count;
            var colWidth = margin.Width / columnas;

            for (var c = 0; c < columnas; c++)
            {
                var x = margin.Left + c * colWidth;
                g.DrawString(_documento.EncabezadosColumnas[c], headerFont, Brushes.Black, x, y);
            }

            y += headerFont.GetHeight(g) + 4;
            g.DrawLine(Pens.Black, margin.Left, y, margin.Right, y);
            y += 6;

            while (_filaIndex < _documento.Filas.Count)
            {
                if (y + bodyFont.GetHeight(g) > margin.Bottom - 30)
                {
                    FinalizarPagina(e, smallFont, margin, ref y);
                    return;
                }

                var fila = _documento.Filas[_filaIndex];
                for (var c = 0; c < columnas; c++)
                {
                    var texto = c < fila.Count ? fila[c] : "";
                    var x = margin.Left + c * colWidth;
                    g.DrawString(texto, bodyFont, Brushes.Black, new RectangleF(x, y, colWidth - 4, bodyFont.GetHeight(g) * 2));
                }

                y += bodyFont.GetHeight(g) + 4;
                _filaIndex++;
            }
        }

        g.DrawString($"Página {_pagina}", smallFont, Brushes.Gray, margin.Right - 80, margin.Bottom + 10);
        e.HasMorePages = false;
    }

    private void FinalizarPagina(PrintPageEventArgs e, Font smallFont, Rectangle margin, ref float y)
    {
        e.Graphics!.DrawString($"Página {_pagina}", smallFont, Brushes.Gray, margin.Right - 80, margin.Bottom + 10);
        _pagina++;
        e.HasMorePages = true;
    }
}
