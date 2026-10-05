using SIREX.App.Ui;
using SIREX.Application.Abstractions;
using SIREX.Domain.Entities;

namespace SIREX.App.Forms;

public sealed class DocentesForm : Form
{
    private readonly IDocenteService _service;
    private readonly DataGridView _grid = new() { Dock = DockStyle.Fill, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, AutoGenerateColumns = true };
    private readonly TextBox _buscar = new() { Width = 220 };

    public DocentesForm(IDocenteService service)
    {
        _service = service;
        Text = "Docentes";
        Width = 800;
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

    private Docente? Seleccionado() => _grid.CurrentRow?.DataBoundItem as Docente;

    private async Task CargarAsync()
    {
        try
        {
            _grid.DataSource = (await _service.ListarAsync(_buscar.Text)).ToList();
        }
        catch (Exception ex)
        {
            UiMessages.ShowError(ex);
        }
    }

    private async Task NuevoAsync()
    {
        using var dlg = new DocenteEditDialog(null);
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;
        try
        {
            await _service.CrearAsync(dlg.Docente);
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
            MessageBox.Show("Seleccione un docente.", "SIREX");
            return;
        }

        var actual = await _service.ObtenerPorIdAsync(sel.Id);
        if (actual is null)
            return;

        using var dlg = new DocenteEditDialog(actual);
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;
        try
        {
            await _service.ActualizarAsync(dlg.Docente);
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
            MessageBox.Show("Seleccione un docente.", "SIREX");
            return;
        }

        if (MessageBox.Show($"¿Eliminar a {sel.NombreCompleto}?", "Confirmar", MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;

        try
        {
            await _service.EliminarAsync(sel.Id);
            await CargarAsync();
        }
        catch (Exception ex)
        {
            UiMessages.ShowError(ex);
        }
    }
}
