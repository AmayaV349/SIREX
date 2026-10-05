using Microsoft.Extensions.DependencyInjection;
using SIREX.Application.Abstractions;

namespace SIREX.App.Forms;

public sealed class MainForm : Form
{
    private readonly IServiceProvider _services;

    public MainForm(IServiceProvider services, string nombreInstitucion, string periodoActivo, string connectionString)
    {
        _services = services;
        Text = $"SIREX — {nombreInstitucion}";
        Width = 960;
        Height = 640;
        IsMdiContainer = true;
        StartPosition = FormStartPosition.CenterScreen;

        var menu = new MenuStrip();
        menu.Items.Add(CrearMenu("Estudiantes", (_, _) => Abrir<EstudiantesForm>()));
        menu.Items.Add(CrearMenu("Docentes", (_, _) => Abrir<DocentesForm>()));
        menu.Items.Add(CrearMenu("Cursos", (_, _) => Abrir<CursosForm>()));
        menu.Items.Add(CrearMenu("Matrículas", (_, _) => Abrir<MatriculasForm>()));
        menu.Items.Add(CrearMenu("Calificaciones", (_, _) => Abrir<CalificacionesForm>()));
        menu.Items.Add(CrearMenu("Reportes", (_, _) => Abrir<ReportesForm>()));
        menu.Items.Add(CrearMenu("Salir", (_, _) => Close()));

        var status = new StatusStrip();
        status.Items.Add(new ToolStripStatusLabel($"BD: {connectionString}"));
        status.Items.Add(new ToolStripStatusLabel($"Periodo activo: {periodoActivo}"));

        MainMenuStrip = menu;
        Controls.Add(menu);
        Controls.Add(status);
    }

    private static ToolStripMenuItem CrearMenu(string texto, EventHandler handler)
    {
        var item = new ToolStripMenuItem(texto);
        item.Click += handler;
        return item;
    }

    private void Abrir<T>() where T : Form
    {
        var form = ActivatorUtilitiesCreateForm<T>();
        form.MdiParent = this;
        form.Show();
    }

    private T ActivatorUtilitiesCreateForm<T>() where T : Form
    {
        if (typeof(T) == typeof(EstudiantesForm))
            return (T)(Form)new EstudiantesForm(_services.GetRequiredService<IEstudianteService>());
        if (typeof(T) == typeof(DocentesForm))
            return (T)(Form)new DocentesForm(_services.GetRequiredService<IDocenteService>());
        if (typeof(T) == typeof(CursosForm))
            return (T)(Form)new CursosForm(_services.GetRequiredService<ICursoService>(), _services.GetRequiredService<IDocenteService>());
        if (typeof(T) == typeof(MatriculasForm))
            return (T)(Form)new MatriculasForm(
                _services.GetRequiredService<IMatriculaService>(),
                _services.GetRequiredService<IEstudianteService>(),
                _services.GetRequiredService<ICursoService>());
        if (typeof(T) == typeof(CalificacionesForm))
            return (T)(Form)new CalificacionesForm(
                _services.GetRequiredService<ICalificacionService>(),
                _services.GetRequiredService<ICursoService>(),
                _services.GetRequiredService<IMatriculaService>());
        if (typeof(T) == typeof(ReportesForm))
            return (T)(Form)new ReportesForm(
                _services.GetRequiredService<IReporteImpresionService>(),
                _services.GetRequiredService<IEstudianteService>(),
                _services.GetRequiredService<ICursoService>());

        throw new InvalidOperationException($"Formulario no registrado: {typeof(T).Name}");
    }
}
