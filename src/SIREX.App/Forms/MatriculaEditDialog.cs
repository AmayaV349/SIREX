using SIREX.Domain.Entities;

namespace SIREX.App.Forms;

public sealed class MatriculaEditDialog : Form
{
    private readonly ComboBox _estudiante = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox _curso = new() { DropDownStyle = ComboBoxStyle.DropDownList };

    public int EstudianteId => _estudiante.SelectedValue is int id ? id : 0;
    public int CursoId => _curso.SelectedValue is int id ? id : 0;

    public MatriculaEditDialog(IReadOnlyList<Estudiante> estudiantes, IReadOnlyList<Curso> cursos)
    {
        Text = "Nueva matrícula";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(420, 180);

        _estudiante.DisplayMember = nameof(Estudiante.NombreCompleto);
        _estudiante.ValueMember = nameof(Estudiante.Id);
        _estudiante.DataSource = estudiantes.Where(e => e.Activo).ToList();

        _curso.DisplayMember = nameof(Curso.Nombre);
        _curso.ValueMember = nameof(Curso.Id);
        _curso.DataSource = cursos.ToList();

        var table = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2, Padding = new Padding(12) };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        table.Controls.Add(new Label { Text = "Estudiante:", AutoSize = true }, 0, 0);
        table.Controls.Add(_estudiante, 1, 0);
        table.Controls.Add(new Label { Text = "Curso:", AutoSize = true }, 0, 1);
        table.Controls.Add(_curso, 1, 1);

        var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, Height = 44, Padding = new Padding(8) };
        var ok = new Button { Text = "Guardar", DialogResult = DialogResult.OK };
        var cancel = new Button { Text = "Cancelar", DialogResult = DialogResult.Cancel };
        buttons.Controls.Add(ok);
        buttons.Controls.Add(cancel);
        AcceptButton = ok;
        CancelButton = cancel;

        Controls.Add(table);
        Controls.Add(buttons);
    }
}
