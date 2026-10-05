# SIREX — Sistema de Información Académica

Aplicación de escritorio **Windows Forms** (.NET 8) para gestión académica básica: estudiantes, docentes, cursos, matrículas, calificaciones e **impresión de reportes** en impresora láser (cola de impresión de Windows).

## Requisitos

- Windows 10/11
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (runtime incluido al publicar self-contained opcional)
- Impresora instalada en Windows (láser USB/red) o **Microsoft Print to PDF** para pruebas

## Estructura

| Proyecto | Descripción |
|----------|-------------|
| `src/SIREX.Domain` | Entidades y enums |
| `src/SIREX.Infrastructure` | EF Core + SQLite |
| `src/SIREX.Application` | Servicios y datos de reportes |
| `src/SIREX.App` | Interfaz WinForms e impresión |

## Compilar y ejecutar

```bash
cd /ruta/a/SIREX
dotnet restore SIREX.sln
dotnet build SIREX.sln
dotnet run --project src/SIREX.App/SIREX.App.csproj
```

La base de datos SQLite `sirex.db` se crea en el directorio de salida del ejecutable (junto a `appsettings.json`) la primera vez que inicia la aplicación.

### Migraciones (opcional)

Si modifica el modelo, en Windows:

```bash
dotnet tool restore
dotnet ef migrations add NombreCambio --project src/SIREX.Infrastructure --startup-project src/SIREX.Infrastructure --output-dir Persistence/Migrations
dotnet ef database update --project src/SIREX.Infrastructure --startup-project src/SIREX.Infrastructure
```

En el arranque normal, la app aplica migraciones con `Database.Migrate()`.

## Configuración

Edite [`src/SIREX.App/appsettings.json`](src/SIREX.App/appsettings.json):

- `Institucion:Nombre` — encabezado de reportes impresos
- `Institucion:PeriodoActivo` — referencia en barra de estado
- `ConnectionStrings:Default` — ruta del archivo SQLite
- `Database:SeedDemoData` — datos demo (docentes y cursos) al crear la BD

## Impresión

Menú **Reportes**:

1. Elija tipo de reporte (listado, constancia, acta).
2. **Vista previa** — revisa el layout.
3. **Imprimir** — en el diálogo de Windows seleccione su impresora láser o “Microsoft Print to PDF”.

La impresión usa `System.Drawing.Printing` (`PrintDocument`) integrada con el spooler de Windows.

## Desarrollo en macOS/Linux

WinForms solo ejecuta en Windows. Puede editar código en cualquier SO; compile y pruebe UI/impresión en una máquina o VM Windows:

```bash
dotnet build SIREX.sln -p:EnableWindowsTargeting=true
```

(Sigue siendo necesario Windows para ejecutar la aplicación.)

## Licencia

Proyecto académico / uso interno — ajuste según su institución.
