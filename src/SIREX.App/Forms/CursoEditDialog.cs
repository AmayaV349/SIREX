using SIREX.Domain.Entities;

namespace SIREX.App.Forms;

public sealed class CursoEditDialog : Form
{
    private readonly TextBox _codigo = new();
    private readonly TextBox _nombre = new();
    private readonly NumericUpDown _creditos = new() { Minimum = 1, Maximum = 20, Value = 3 };
    private readonly NumericUpDown _cupo = new() { Minimum = 1, Maximum = 500, Value = 30 };
    private readonly TextBox _periodo = new();
    private readonly ComboBox _docente = new() { DropDownStyle = ComboBoxStyle.DropDownList };

    public Curso Curso { get; }

    public CursoEditDialog(Curso? existente, IReadOnlyList<Docente> docentes)
    {
        Curso = existente is null
            ? new Curso { Periodo = "2026-1" }
            : new Curso
            {
                Id = existente.Id,
                Codigo = existente.Codigo,
                Nombre = existente.Nombre,
                Creditos = existente.Creditos,
                DocenteId = existente.DocenteId,
                Periodo = existente.Periodo,
                Cupo = existente.Cupo
            };

        Text = existente is null ? "Nuevo curso" : "Editar curso";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(440, 300);

        _docente.DisplayMember = nameof(Docente.NombreCompleto);
        _docente.ValueMember = nameof(Docente.Id);
        _docente.DataSource = docentes.ToList();

        var table = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 7, Padding = new Padding(12) };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        AgregarFila(table, "Código:", _codigo, 0);
        AgregarFila(table, "Nombre:", _nombre, 1);
        AgregarFila(table, "Créditos:", _creditos, 2);
        AgregarFila(table, "Cupo:", _cupo, 3);
        AgregarFila(table, "Periodo:", _periodo, 4);
        AgregarFila(table, "Docente:", _docente, 5);

        var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, Height = 44, Padding = new Padding(8) };
        var ok = new Button { Text = "Guardar", DialogResult = DialogResult.OK };
        var cancel = new Button { Text = "Cancelar", DialogResult = DialogResult.Cancel };
        ok.Click += (_, _) => GuardarEnModelo();
        buttons.Controls.Add(ok);
        buttons.Controls.Add(cancel);
        AcceptButton = ok;
        CancelButton = cancel;

        _codigo.Text = Curso.Codigo;
        _nombre.Text = Curso.Nombre;
        _creditos.Value = Curso.Creditos;
        _cupo.Value = Curso.Cupo;
        _periodo.Text = Curso.Periodo;
        if (docentes.Any())
            _docente.SelectedValue = Curso.DocenteId == 0 ? docentes[0].Id : Curso.DocenteId;

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
        Curso.Codigo = _codigo.Text.Trim();
        Curso.Nombre = _nombre.Text.Trim();
        Curso.Creditos = (int)_creditos.Value;
        Curso.Cupo = (int)_cupo.Value;
        Curso.Periodo = _periodo.Text.Trim();
        Curso.DocenteId = _docente.SelectedValue is int id ? id : 0;
    }
}
