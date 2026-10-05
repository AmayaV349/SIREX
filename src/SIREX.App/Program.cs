using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIREX.App.Forms;
using SIREX.Application;
using SIREX.Infrastructure;
using SIREX.Infrastructure.Persistence;

namespace SIREX.App;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        System.Windows.Forms.Application.EnableVisualStyles();
        System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var connectionString = config.GetConnectionString("Default") ?? "Data Source=sirex.db";
        var nombreInstitucion = config["Institucion:Nombre"] ?? "SIREX";
        var seedDemo = config.GetValue("Database:SeedDemoData", true);

        var services = new ServiceCollection();
        services.AddInfrastructure(connectionString);
        services.AddApplication(nombreInstitucion);

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<SirexDbContext>();
        DbInitializer.MigrateAndSeed(db, seedDemo);

        var periodoActivo = config["Institucion:PeriodoActivo"] ?? "";
        System.Windows.Forms.Application.Run(new MainForm(scope.ServiceProvider, nombreInstitucion, periodoActivo, connectionString));
    }
}
