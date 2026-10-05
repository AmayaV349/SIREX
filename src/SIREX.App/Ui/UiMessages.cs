using SIREX.Application.Common;

namespace SIREX.App.Ui;

internal static class UiMessages
{
    public static void ShowError(Exception ex)
    {
        var message = ex is BusinessException be ? be.Message : $"Error inesperado: {ex.Message}";
        MessageBox.Show(message, "SIREX", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
