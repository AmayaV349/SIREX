using SIREX.App.Ui;
using SIREX.Application.Abstractions;
using SIREX.Domain.Entities;

namespace SIREX.App.Forms;

public sealed class CursosForm : Form
{
    private readonly ICursoService _cursoService;
    private readonly IDocenteService _docenteService;
    private readonly DataGridView _grid = new() { Dock = DockStyle.Fill, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, AutoGenerateColumns = true };
    private readonly TextBox _buscar = new() { Width = 220 };

    public CursosForm(ICursoService cursoService, IDocenteService docenteService)
    {
        _cursoService = cursoService;
        _docenteService = docenteService;
        Text = "Cursos";
        Width = 900;
        Height = 480;

        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(8) };
        top.Controls.Add(new Label { Text = "Buscar:", AutoSize = true, Padding = new Padding(0, 8, 0, 0) });
        top.Controls.Add(_buscar);
        var btnBuscar = new Button { Text = "Buscar" };
        btnBuscar.Click += async (_, _) => await CargarAsync();
        top.Controls.Add(btnBuscar);

        var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 48, Padding = new Padding(8) };
        var btnNuevo = new Button { Text = "Nuevo" };
        btnNuevo.Click += async (_, _) => await NuevoAsync();
        var btnEditar = new Button { Text = "Editar" };
        btnEditar.Click += async (_, _) => await EditarAsync();
        var btnEliminar = new Button { Text = "Eliminar" };
        btnEliminar.Click += async (_, _) => await EliminarAsync();
        bottom.Controls.AddRange([btnNuevo, btnEditar, btnEliminar]);

        Controls.Add(_grid);
        Controls.Add(bottom);
        Controls.Add(top);
        Shown += async (_, _) => await CargarAsync();
    }

    private Curso? Seleccionado() => _grid.CurrentRow?.DataBoundItem as Curso;

    private async Task CargarAsync()
    {
        try
        {
            _grid.DataSource = (await _cursoService.ListarAsync(_buscar.Text)).ToList();
        }
        catch (Exception ex)
        {
            UiMessages.ShowError(ex);
        }
    }

    private async Task NuevoAsync()
    {
        var docentes = await _docenteService.ListarAsync();
        using var dlg = new CursoEditDialog(null, docentes);
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;
        try
        {
            await _cursoService.CrearAsync(dlg.Curso);
            await CargarAsync();
        }
        catch (Exception ex)
        {
            UiMessages.ShowError(ex);
        }
    }

    private async Task EditarAsync()
    {
        var sel = Seleccionado();
        if (sel is null)
        {
            MessageBox.Show("Seleccione un curso.", "SIREX");
            return;
        }

        var actual = await _cursoService.ObtenerPorIdAsync(sel.Id);
        if (actual is null)
            return;

        var docentes = await _docenteService.ListarAsync();
        using var dlg = new CursoEditDialog(actual, docentes);
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;
        try
        {
            await _cursoService.ActualizarAsync(dlg.Curso);
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
            MessageBox.Show("Seleccione un curso.", "SIREX");
            return;
        }

        if (MessageBox.Show($"¿Eliminar el curso {sel.Codigo}?", "Confirmar", MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;

        try
        {
            await _cursoService.EliminarAsync(sel.Id);
            await CargarAsync();
        }
        catch (Exception ex)
        {
            UiMessages.ShowError(ex);
        }
    }
}
