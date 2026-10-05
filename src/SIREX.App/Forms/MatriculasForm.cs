using SIREX.App.Ui;
using SIREX.Application.Abstractions;
using SIREX.Domain.Entities;
using SIREX.Domain.Enums;

namespace SIREX.App.Forms;

public sealed class MatriculasForm : Form
{
    private readonly IMatriculaService _matriculaService;
    private readonly IEstudianteService _estudianteService;
    private readonly ICursoService _cursoService;
    private readonly DataGridView _grid = new() { Dock = DockStyle.Fill, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, AutoGenerateColumns = true };
    private readonly ComboBox _filtroCurso = new() { Width = 260, DropDownStyle = ComboBoxStyle.DropDownList };

    public MatriculasForm(
        IMatriculaService matriculaService,
        IEstudianteService estudianteService,
        ICursoService cursoService)
    {
        _matriculaService = matriculaService;
        _estudianteService = estudianteService;
        _cursoService = cursoService;
        Text = "Matrículas";
        Width = 900;
        Height = 480;

        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(8) };
        top.Controls.Add(new Label { Text = "Curso:", AutoSize = true, Padding = new Padding(0, 8, 0, 0) });
        top.Controls.Add(_filtroCurso);
        var btnFiltrar = new Button { Text = "Filtrar" };
        btnFiltrar.Click += async (_, _) => await CargarAsync();
        top.Controls.Add(btnFiltrar);

        var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 48, Padding = new Padding(8) };
        var btnNuevo = new Button { Text = "Nueva matrícula" };
        btnNuevo.Click += async (_, _) => await NuevaAsync();
        var btnRetirar = new Button { Text = "Marcar retirada" };
        btnRetirar.Click += async (_, _) => await CambiarEstadoAsync(EstadoMatricula.Retirada);
        var btnActivar = new Button { Text = "Reactivar" };
        btnActivar.Click += async (_, _) => await CambiarEstadoAsync(EstadoMatricula.Activa);
        var btnEliminar = new Button { Text = "Eliminar" };
        btnEliminar.Click += async (_, _) => await EliminarAsync();
        bottom.Controls.AddRange([btnNuevo, btnRetirar, btnActivar, btnEliminar]);

        Controls.Add(_grid);
        Controls.Add(bottom);
        Controls.Add(top);
        Shown += async (_, _) =>
        {
            await CargarCombosAsync();
            await CargarAsync();
        };
    }

    private Matricula? Seleccionado() => _grid.CurrentRow?.DataBoundItem as Matricula;

    private async Task CargarCombosAsync()
    {
        var cursos = await _cursoService.ListarAsync();
        var items = new List<Curso> { new() { Id = 0, Codigo = "", Nombre = "(Todos)", DocenteId = 0, Periodo = "" } };
        items.AddRange(cursos);
        _filtroCurso.DisplayMember = nameof(Curso.Nombre);
        _filtroCurso.ValueMember = nameof(Curso.Id);
        _filtroCurso.DataSource = items;
        _filtroCurso.SelectedIndex = 0;
    }

    private async Task CargarAsync()
    {
        try
        {
            int? cursoId = _filtroCurso.SelectedValue is int id && id > 0 ? id : null;
            _grid.DataSource = (await _matriculaService.ListarAsync(cursoId)).ToList();
        }
        catch (Exception ex)
        {
            UiMessages.ShowError(ex);
        }
    }

    private async Task NuevaAsync()
    {
        var estudiantes = await _estudianteService.ListarAsync();
        var cursos = await _cursoService.ListarAsync();
        using var dlg = new MatriculaEditDialog(estudiantes, cursos);
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;
        try
        {
            await _matriculaService.CrearAsync(dlg.EstudianteId, dlg.CursoId);
            await CargarAsync();
        }
        catch (Exception ex)
        {
            UiMessages.ShowError(ex);
        }
    }

    private async Task CambiarEstadoAsync(EstadoMatricula estado)
    {
        var sel = Seleccionado();
        if (sel is null)
        {
            MessageBox.Show("Seleccione una matrícula.", "SIREX");
            return;
        }

        try
        {
            await _matriculaService.ActualizarEstadoAsync(sel.Id, estado);
            await CargarAsync();
        }
        catch (Exception ex)
        {
            UiMessages.ShowError(ex);
        }
    }

    private async Task EliminarAsync()
    {
        var sel = Seleccionado();
        if (sel is null)
        {
            MessageBox.Show("Seleccione una matrícula.", "SIREX");
            return;
        }

        if (MessageBox.Show("¿Eliminar la matrícula seleccionada?", "Confirmar", MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;

        try
        {
            await _matriculaService.EliminarAsync(sel.Id);
            await CargarAsync();
        }
        catch (Exception ex)
        {
            UiMessages.ShowError(ex);
        }
    }
}
