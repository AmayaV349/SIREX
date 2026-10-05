using System.Drawing.Printing;
using SIREX.Application.Printing;

namespace SIREX.App.Printing;

internal static class ImpresionUiHelper
{
    public static void VistaPrevia(IWin32Window owner, InformeDocumento documento)
    {
        using var printDoc = new InformePrintDocument(documento);
        using var preview = new PrintPreviewDialog
        {
            Document = printDoc,
            Width = 900,
            Height = 700
        };
        preview.ShowDialog(owner);
    }

    public static void Imprimir(IWin32Window owner, InformeDocumento documento)
    {
        using var printDoc = new InformePrintDocument(documento);
        using var dialog = new PrintDialog { Document = printDoc, UseEXDialog = true };
        if (dialog.ShowDialog(owner) != DialogResult.OK)
            return;

        printDoc.Print();
    }
}
