using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SIREX.Infrastructure.Persistence;

#nullable disable

namespace SIREX.Infrastructure.Persistence.Migrations;

[DbContext(typeof(SirexDbContext))]
[Migration("20260330120000_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Docentes",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Codigo = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                Nombres = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                Apellidos = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                Especialidad = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                Activo = table.Column<bool>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Docentes", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Estudiantes",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Codigo = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                Nombres = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                Apellidos = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                Documento = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                Email = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                Telefono = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                Activo = table.Column<bool>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Estudiantes", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Cursos",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Codigo = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                Nombre = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                Creditos = table.Column<int>(type: "INTEGER", nullable: false),
                DocenteId = table.Column<int>(type: "INTEGER", nullable: false),
                Periodo = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                Cupo = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Cursos", x => x.Id);
                table.ForeignKey(
                    name: "FK_Cursos_Docentes_DocenteId",
                    column: x => x.DocenteId,
                    principalTable: "Docentes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Matriculas",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                EstudianteId = table.Column<int>(type: "INTEGER", nullable: false),
                CursoId = table.Column<int>(type: "INTEGER", nullable: false),
                Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                Estado = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Matriculas", x => x.Id);
                table.ForeignKey(
                    name: "FK_Matriculas_Cursos_CursoId",
                    column: x => x.CursoId,
                    principalTable: "Cursos",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Matriculas_Estudiantes_EstudianteId",
                    column: x => x.EstudianteId,
                    principalTable: "Estudiantes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Calificaciones",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                MatriculaId = table.Column<int>(type: "INTEGER", nullable: false),
                NotaFinal = table.Column<decimal>(type: "TEXT", precision: 5, scale: 2, nullable: false),
                Observaciones = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                FechaRegistro = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Calificaciones", x => x.Id);
                table.ForeignKey(
                    name: "FK_Calificaciones_Matriculas_MatriculaId",
                    column: x => x.MatriculaId,
                    principalTable: "Matriculas",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(name: "IX_Calificaciones_MatriculaId", table: "Calificaciones", column: "MatriculaId", unique: true);
        migrationBuilder.CreateIndex(name: "IX_Cursos_Codigo", table: "Cursos", column: "Codigo", unique: true);
        migrationBuilder.CreateIndex(name: "IX_Cursos_DocenteId", table: "Cursos", column: "DocenteId");
        migrationBuilder.CreateIndex(name: "IX_Docentes_Codigo", table: "Docentes", column: "Codigo", unique: true);
        migrationBuilder.CreateIndex(name: "IX_Estudiantes_Codigo", table: "Estudiantes", column: "Codigo", unique: true);
        migrationBuilder.CreateIndex(name: "IX_Matriculas_CursoId", table: "Matriculas", column: "CursoId");
        migrationBuilder.CreateIndex(name: "IX_Matriculas_EstudianteId_CursoId", table: "Matriculas", columns: new[] { "EstudianteId", "CursoId" }, unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Calificaciones");
        migrationBuilder.DropTable(name: "Matriculas");
        migrationBuilder.DropTable(name: "Cursos");
        migrationBuilder.DropTable(name: "Estudiantes");
        migrationBuilder.DropTable(name: "Docentes");
    }
}
