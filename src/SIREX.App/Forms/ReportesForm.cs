using SIREX.App.Printing;
using SIREX.App.Ui;
using SIREX.Application.Abstractions;
using SIREX.Application.Printing;
using SIREX.Domain.Entities;

namespace SIREX.App.Forms;

public sealed class ReportesForm : Form
{
    private enum TipoReporte
    {
        ListadoEstudiantes,
        ConstanciaMatricula,
        ActaCalificaciones
    }

    private readonly IReporteImpresionService _reporteService;
    private readonly IEstudianteService _estudianteService;
    private readonly ICursoService _cursoService;

    private readonly ComboBox _tipo = new() { Width = 280, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox _estudiante = new() { Width = 280, DropDownStyle = ComboBoxStyle.DropDownList, Visible = false };
    private readonly ComboBox _curso = new() { Width = 280, DropDownStyle = ComboBoxStyle.DropDownList, Visible = false };
    private readonly Label _lblEstudiante = new() { Text = "Estudiante:", AutoSize = true, Visible = false };
    private readonly Label _lblCurso = new() { Text = "Curso:", AutoSize = true, Visible = false };

    public ReportesForm(
        IReporteImpresionService reporteService,
        IEstudianteService estudianteService,
        ICursoService cursoService)
    {
        _reporteService = reporteService;
        _estudianteService = estudianteService;
        _cursoService = cursoService;

        Text = "Reportes e impresión";
        Width = 520;
        Height = 320;

        _tipo.Items.AddRange(
        [
            "Listado de estudiantes",
            "Constancia de matrícula",
            "Acta de calificaciones por curso"
        ]);
        _tipo.SelectedIndex = 0;
        _tipo.SelectedIndexChanged += (_, _) => ActualizarParametros();

        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 5,
            Padding = new Padding(16)
        };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        panel.Controls.Add(new Label { Text = "Reporte:", AutoSize = true }, 0, 0);
        panel.Controls.Add(_tipo, 1, 0);
        panel.Controls.Add(_lblEstudiante, 0, 1);
        panel.Controls.Add(_estudiante, 1, 1);
        panel.Controls.Add(_lblCurso, 0, 2);
        panel.Controls.Add(_curso, 1, 2);

        var info = new Label
        {
            Text = "Use Vista previa para revisar el documento. Al imprimir, seleccione su impresora láser en el diálogo de Windows.",
            AutoSize = false,
            Dock = DockStyle.Fill,
            MaximumSize = new Size(420, 80)
        };
        panel.Controls.Add(info, 0, 3);
        panel.SetColumnSpan(info, 2);

        var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 48, Padding = new Padding(8), FlowDirection = FlowDirection.RightToLeft };
        var btnImprimir = new Button { Text = "Imprimir" };
        btnImprimir.Click += async (_, _) => await ImprimirAsync();
        var btnPreview = new Button { Text = "Vista previa" };
        btnPreview.Click += async (_, _) => await VistaPreviaAsync();
        bottom.Controls.Add(btnImprimir);
        bottom.Controls.Add(btnPreview);

        Controls.Add(panel);
        Controls.Add(bottom);

        Shown += async (_, _) =>
        {
            await CargarCombosAsync();
            ActualizarParametros();
        };
    }

    private async Task CargarCombosAsync()
    {
        var estudiantes = await _estudianteService.ListarAsync();
        _estudiante.DisplayMember = nameof(Estudiante.NombreCompleto);
        _estudiante.ValueMember = nameof(Estudiante.Id);
        _estudiante.DataSource = estudiantes.ToList();

        var cursos = await _cursoService.ListarAsync();
        _curso.DisplayMember = nameof(Curso.Nombre);
        _curso.ValueMember = nameof(Curso.Id);
        _curso.DataSource = cursos.ToList();
    }

    private void ActualizarParametros()
    {
        var tipo = (TipoReporte)_tipo.SelectedIndex;
        var needEstudiante = tipo == TipoReporte.ConstanciaMatricula;
        var needCurso = tipo == TipoReporte.ActaCalificaciones;

        _lblEstudiante.Visible = _estudiante.Visible = needEstudiante;
        _lblCurso.Visible = _curso.Visible = needCurso;
    }

    private async Task<InformeDocumento?> CrearDocumentoAsync()
    {
        try
        {
            return (TipoReporte)_tipo.SelectedIndex switch
            {
                TipoReporte.ListadoEstudiantes => await _reporteService.CrearListadoEstudiantesAsync(),
                TipoReporte.ConstanciaMatricula when _estudiante.SelectedValue is int estId =>
                    await _reporteService.CrearConstanciaMatriculaAsync(estId),
                TipoReporte.ActaCalificaciones when _curso.SelectedValue is int cursoId =>
                    await _reporteService.CrearActaCalificacionesAsync(cursoId),
                _ => throw new InvalidOperationException("Seleccione los parámetros del reporte.")
            };
        }
        catch (Exception ex)
        {
            UiMessages.ShowError(ex);
            return null;
        }
    }

    private async Task VistaPreviaAsync()
    {
        var doc = await CrearDocumentoAsync();
        if (doc is null)
            return;
        ImpresionUiHelper.VistaPrevia(this, doc);
    }

    private async Task ImprimirAsync()
    {
        var doc = await CrearDocumentoAsync();
        if (doc is null)
            return;
        ImpresionUiHelper.Imprimir(this, doc);
    }
}
