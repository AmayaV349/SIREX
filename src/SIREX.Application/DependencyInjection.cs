using Microsoft.Extensions.DependencyInjection;
using SIREX.Application.Abstractions;
using SIREX.Application.Services;

namespace SIREX.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, string nombreInstitucion)
    {
        services.AddScoped<IEstudianteService, EstudianteService>();
        services.AddScoped<IDocenteService, DocenteService>();
        services.AddScoped<ICursoService, CursoService>();
        services.AddScoped<IMatriculaService, MatriculaService>();
        services.AddScoped<ICalificacionService, CalificacionService>();
        services.AddScoped<IReporteImpresionService>(sp =>
            new ReporteImpresionService(
                sp.GetRequiredService<Infrastructure.Persistence.SirexDbContext>(),
                nombreInstitucion));

        return services;
    }
}
