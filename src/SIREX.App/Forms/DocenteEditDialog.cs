using SIREX.Domain.Entities;

namespace SIREX.App.Forms;

public sealed class DocenteEditDialog : Form
{
    private readonly TextBox _codigo = new();
    private readonly TextBox _nombres = new();
    private readonly TextBox _apellidos = new();
    private readonly TextBox _especialidad = new();
    private readonly CheckBox _activo = new() { Text = "Activo", Checked = true };

    public Docente Docente { get; }

    public DocenteEditDialog(Docente? existente)
    {
        Docente = existente is null
            ? new Docente()
            : new Docente
            {
                Id = existente.Id,
                Codigo = existente.Codigo,
                Nombres = existente.Nombres,
                Apellidos = existente.Apellidos,
                Especialidad = existente.Especialidad,
                Activo = existente.Activo
            };

        Text = existente is null ? "Nuevo docente" : "Editar docente";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(420, 260);

        var table = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 6, Padding = new Padding(12) };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        AgregarFila(table, "Código:", _codigo, 0);
        AgregarFila(table, "Nombres:", _nombres, 1);
        AgregarFila(table, "Apellidos:", _apellidos, 2);
        AgregarFila(table, "Especialidad:", _especialidad, 3);
        table.Controls.Add(_activo, 1, 4);

        var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, Height = 44, Padding = new Padding(8) };
        var ok = new Button { Text = "Guardar", DialogResult = DialogResult.OK };
        var cancel = new Button { Text = "Cancelar", DialogResult = DialogResult.Cancel };
        ok.Click += (_, _) => GuardarEnModelo();
        buttons.Controls.Add(ok);
        buttons.Controls.Add(cancel);
        AcceptButton = ok;
        CancelButton = cancel;

        _codigo.Text = Docente.Codigo;
        _nombres.Text = Docente.Nombres;
        _apellidos.Text = Docente.Apellidos;
        _especialidad.Text = Docente.Especialidad ?? "";
        _activo.Checked = Docente.Activo;

        Controls.Add(table);
        Controls.Add(buttons);
    }

    private static void AgregarFila(TableLayoutPanel table, string label, Control control, int row)
    {
        table.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left }, 0, row);
        control.Dock = DockStyle.Fill;
        table.Controls.Add(control, 1, row);
    }

    private void GuardarEnModelo()
    {
        Docente.Codigo = _codigo.Text.Trim();
        Docente.Nombres = _nombres.Text.Trim();
        Docente.Apellidos = _apellidos.Text.Trim();
        Docente.Especialidad = string.IsNullOrWhiteSpace(_especialidad.Text) ? null : _especialidad.Text.Trim();
        Docente.Activo = _activo.Checked;
    }
}
