using SIREX.App.Ui;
using SIREX.Application.Abstractions;
using SIREX.Domain.Entities;
using SIREX.Domain.Enums;

namespace SIREX.App.Forms;

public sealed class CalificacionesForm : Form
{
    private readonly ICalificacionService _calificacionService;
    private readonly ICursoService _cursoService;
    private readonly IMatriculaService _matriculaService;
    private readonly ComboBox _curso = new() { Width = 280, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly DataGridView _grid = new()
    {
        Dock = DockStyle.Fill,
        AutoGenerateColumns = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        MultiSelect = false
    };

    private List<Matricula> _matriculas = [];

    public CalificacionesForm(
        ICalificacionService calificacionService,
        ICursoService cursoService,
        IMatriculaService matriculaService)
    {
        _calificacionService = calificacionService;
        _cursoService = cursoService;
        _matriculaService = matriculaService;
        Text = "Calificaciones";
        Width = 900;
        Height = 480;

        _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Estudiante", HeaderText = "Estudiante", ReadOnly = true, Width = 220 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Codigo", HeaderText = "Código", ReadOnly = true, Width = 90 });
        var notaCol = new DataGridViewTextBoxColumn { Name = "Nota", HeaderText = "Nota final", Width = 90 };
        _grid.Columns.Add(notaCol);
        _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Observaciones", HeaderText = "Observaciones", Width = 240 });

        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(8) };
        top.Controls.Add(new Label { Text = "Curso:", AutoSize = true, Padding = new Padding(0, 8, 0, 0) });
        top.Controls.Add(_curso);
        var btnCargar = new Button { Text = "Cargar matrículas" };
        btnCargar.Click += async (_, _) => await CargarMatriculasAsync();
        top.Controls.Add(btnCargar);

        var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 48, Padding = new Padding(8) };
        var btnGuardar = new Button { Text = "Guardar calificaciones" };
        btnGuardar.Click += async (_, _) => await GuardarAsync();
        bottom.Controls.Add(btnGuardar);

        Controls.Add(_grid);
        Controls.Add(bottom);
        Controls.Add(top);
        Shown += async (_, _) => await CargarCursosAsync();
    }

    private async Task CargarCursosAsync()
    {
        var cursos = await _cursoService.ListarAsync();
        _curso.DisplayMember = nameof(Curso.Nombre);
        _curso.ValueMember = nameof(Curso.Id);
        _curso.DataSource = cursos.ToList();
    }

    private async Task CargarMatriculasAsync()
    {
        if (_curso.SelectedValue is not int cursoId)
            return;

        try
        {
            _matriculas = (await _matriculaService.ListarAsync(cursoId))
                .Where(m => m.Estado == EstadoMatricula.Activa)
                .ToList();

            var calificaciones = await _calificacionService.ListarPorCursoAsync(cursoId);
            var notas = calificaciones.ToDictionary(c => c.MatriculaId);

            _grid.Rows.Clear();
            foreach (var m in _matriculas)
            {
                notas.TryGetValue(m.Id, out var cal);
                var idx = _grid.Rows.Add(m.Estudiante.NombreCompleto, m.Estudiante.Codigo,
                    cal?.NotaFinal.ToString("0.##") ?? "",
                    cal?.Observaciones ?? "");
                _grid.Rows[idx].Tag = m.Id;
            }
        }
        catch (Exception ex)
        {
            UiMessages.ShowError(ex);
        }
    }

    private async Task GuardarAsync()
    {
        try
        {
            foreach (DataGridViewRow row in _grid.Rows)
            {
                if (row.Tag is not int matriculaId)
                    continue;

                var notaText = row.Cells["Nota"].Value?.ToString()?.Trim();
                if (string.IsNullOrEmpty(notaText))
                    continue;

                if (!decimal.TryParse(notaText, out var nota))
                    throw new InvalidOperationException($"Nota inválida para fila {row.Index + 1}.");

                var obs = row.Cells["Observaciones"].Value?.ToString();
                await _calificacionService.GuardarAsync(matriculaId, nota, obs);
            }

            MessageBox.Show("Calificaciones guardadas.", "SIREX");
            await CargarMatriculasAsync();
        }
        catch (Exception ex)
        {
            UiMessages.ShowError(ex);
        }
    }
}
