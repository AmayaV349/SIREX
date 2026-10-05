using SIREX.Domain.Entities;

namespace SIREX.App.Forms;

public sealed class EstudianteEditDialog : Form
{
    private readonly TextBox _codigo = new();
    private readonly TextBox _nombres = new();
    private readonly TextBox _apellidos = new();
    private readonly TextBox _documento = new();
    private readonly TextBox _email = new();
    private readonly TextBox _telefono = new();
    private readonly CheckBox _activo = new() { Text = "Activo", Checked = true };

    public Estudiante Estudiante { get; }

    public EstudianteEditDialog(Estudiante? existente)
    {
        Estudiante = existente is null
            ? new Estudiante()
            : new Estudiante
            {
                Id = existente.Id,
                Codigo = existente.Codigo,
                Nombres = existente.Nombres,
                Apellidos = existente.Apellidos,
                Documento = existente.Documento,
                Email = existente.Email,
                Telefono = existente.Telefono,
                Activo = existente.Activo
            };

        Text = existente is null ? "Nuevo estudiante" : "Editar estudiante";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(420, 320);

        var table = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 8, Padding = new Padding(12) };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        AgregarFila(table, "Código:", _codigo, 0);
        AgregarFila(table, "Nombres:", _nombres, 1);
        AgregarFila(table, "Apellidos:", _apellidos, 2);
        AgregarFila(table, "Documento:", _documento, 3);
        AgregarFila(table, "Email:", _email, 4);
        AgregarFila(table, "Teléfono:", _telefono, 5);
        table.Controls.Add(_activo, 1, 6);

        var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, Height = 44, Padding = new Padding(8) };
        var ok = new Button { Text = "Guardar", DialogResult = DialogResult.OK };
        var cancel = new Button { Text = "Cancelar", DialogResult = DialogResult.Cancel };
        ok.Click += (_, _) => GuardarEnModelo();
        buttons.Controls.Add(ok);
        buttons.Controls.Add(cancel);
        AcceptButton = ok;
        CancelButton = cancel;

        _codigo.Text = Estudiante.Codigo;
        _nombres.Text = Estudiante.Nombres;
        _apellidos.Text = Estudiante.Apellidos;
        _documento.Text = Estudiante.Documento;
        _email.Text = Estudiante.Email ?? "";
        _telefono.Text = Estudiante.Telefono ?? "";
        _activo.Checked = Estudiante.Activo;

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
        Estudiante.Codigo = _codigo.Text.Trim();
        Estudiante.Nombres = _nombres.Text.Trim();
        Estudiante.Apellidos = _apellidos.Text.Trim();
        Estudiante.Documento = _documento.Text.Trim();
        Estudiante.Email = string.IsNullOrWhiteSpace(_email.Text) ? null : _email.Text.Trim();
        Estudiante.Telefono = string.IsNullOrWhiteSpace(_telefono.Text) ? null : _telefono.Text.Trim();
        Estudiante.Activo = _activo.Checked;
    }
}
