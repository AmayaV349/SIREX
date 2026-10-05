using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SIREX.Infrastructure.Persistence;

#nullable disable

namespace SIREX.Infrastructure.Persistence.Migrations;

[DbContext(typeof(SirexDbContext))]
partial class SirexDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder.HasAnnotation("ProductVersion", "8.0.11");

        modelBuilder.Entity("SIREX.Domain.Entities.Calificacion", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("INTEGER");
            b.Property<int>("MatriculaId").HasColumnType("INTEGER");
            b.Property<decimal>("NotaFinal").HasPrecision(5, 2).HasColumnType("TEXT");
            b.Property<string>("Observaciones").HasMaxLength(500).HasColumnType("TEXT");
            b.Property<DateTime>("FechaRegistro").HasColumnType("TEXT");
            b.HasKey("Id");
            b.HasIndex("MatriculaId").IsUnique();
            b.ToTable("Calificaciones", (string)null);
        });

        modelBuilder.Entity("SIREX.Domain.Entities.Curso", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");
            b.Property<string>("Codigo").IsRequired().HasMaxLength(20).HasColumnType("TEXT");
            b.Property<int>("Creditos").HasColumnType("INTEGER");
            b.Property<int>("Cupo").HasColumnType("INTEGER");
            b.Property<int>("DocenteId").HasColumnType("INTEGER");
            b.Property<string>("Nombre").IsRequired().HasMaxLength(200).HasColumnType("TEXT");
            b.Property<string>("Periodo").IsRequired().HasMaxLength(20).HasColumnType("TEXT");
            b.HasKey("Id");
            b.HasIndex("Codigo").IsUnique();
            b.HasIndex("DocenteId");
            b.ToTable("Cursos", (string)null);
        });

        modelBuilder.Entity("SIREX.Domain.Entities.Docente", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");
            b.Property<bool>("Activo").HasColumnType("INTEGER");
            b.Property<string>("Apellidos").IsRequired().HasMaxLength(100).HasColumnType("TEXT");
            b.Property<string>("Codigo").IsRequired().HasMaxLength(20).HasColumnType("TEXT");
            b.Property<string>("Especialidad").HasMaxLength(150).HasColumnType("TEXT");
            b.Property<string>("Nombres").IsRequired().HasMaxLength(100).HasColumnType("TEXT");
            b.HasKey("Id");
            b.HasIndex("Codigo").IsUnique();
            b.ToTable("Docentes", (string)null);
        });

        modelBuilder.Entity("SIREX.Domain.Entities.Estudiante", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");
            b.Property<bool>("Activo").HasColumnType("INTEGER");
            b.Property<string>("Apellidos").IsRequired().HasMaxLength(100).HasColumnType("TEXT");
            b.Property<string>("Codigo").IsRequired().HasMaxLength(20).HasColumnType("TEXT");
            b.Property<string>("Documento").IsRequired().HasMaxLength(30).HasColumnType("TEXT");
            b.Property<string>("Email").HasMaxLength(150).HasColumnType("TEXT");
            b.Property<string>("Nombres").IsRequired().HasMaxLength(100).HasColumnType("TEXT");
            b.Property<string>("Telefono").HasMaxLength(30).HasColumnType("TEXT");
            b.HasKey("Id");
            b.HasIndex("Codigo").IsUnique();
            b.ToTable("Estudiantes", (string)null);
        });

        modelBuilder.Entity("SIREX.Domain.Entities.Matricula", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");
            b.Property<int>("CursoId").HasColumnType("INTEGER");
            b.Property<int>("Estado").HasColumnType("INTEGER");
            b.Property<int>("EstudianteId").HasColumnType("INTEGER");
            b.Property<DateTime>("Fecha").HasColumnType("TEXT");
            b.HasKey("Id");
            b.HasIndex("CursoId");
            b.HasIndex("EstudianteId", "CursoId").IsUnique();
            b.ToTable("Matriculas", (string)null);
        });

        modelBuilder.Entity("SIREX.Domain.Entities.Calificacion", b =>
        {
            b.HasOne("SIREX.Domain.Entities.Matricula", "Matricula")
                .WithOne("Calificacion")
                .HasForeignKey("SIREX.Domain.Entities.Calificacion", "MatriculaId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            b.Navigation("Matricula");
        });

        modelBuilder.Entity("SIREX.Domain.Entities.Curso", b =>
        {
            b.HasOne("SIREX.Domain.Entities.Docente", "Docente")
                .WithMany("Cursos")
                .HasForeignKey("DocenteId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            b.Navigation("Docente");
        });

        modelBuilder.Entity("SIREX.Domain.Entities.Matricula", b =>
        {
            b.HasOne("SIREX.Domain.Entities.Curso", "Curso")
                .WithMany("Matriculas")
                .HasForeignKey("CursoId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            b.HasOne("SIREX.Domain.Entities.Estudiante", "Estudiante")
                .WithMany("Matriculas")
                .HasForeignKey("EstudianteId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            b.Navigation("Curso");
            b.Navigation("Estudiante");
        });

        modelBuilder.Entity("SIREX.Domain.Entities.Curso", b =>
        {
            b.Navigation("Matriculas");
        });

        modelBuilder.Entity("SIREX.Domain.Entities.Docente", b =>
        {
            b.Navigation("Cursos");
        });

        modelBuilder.Entity("SIREX.Domain.Entities.Estudiante", b =>
        {
            b.Navigation("Matriculas");
        });

        modelBuilder.Entity("SIREX.Domain.Entities.Matricula", b =>
        {
            b.Navigation("Calificacion");
        });
#pragma warning restore 612, 618
    }
}
