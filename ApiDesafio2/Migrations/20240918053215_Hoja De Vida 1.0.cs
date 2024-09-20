using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiDesafio2.Migrations
{
    /// <inheritdoc />
    public partial class HojaDeVida10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HojaDeVida",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCompleto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HojaDeVida", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExperienciasProfesionales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Empresa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Cargo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    HojaDeVidaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienciasProfesionales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperienciasProfesionales_HojaDeVida_HojaDeVidaId",
                        column: x => x.HojaDeVidaId,
                        principalTable: "HojaDeVida",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormacionesAcademicas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Institucion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TituloObtenido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HojaDeVidaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormacionesAcademicas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormacionesAcademicas_HojaDeVida_HojaDeVidaId",
                        column: x => x.HojaDeVidaId,
                        principalTable: "HojaDeVida",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Idiomas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreIdioma = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nivel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HojaDeVidaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Idiomas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Idiomas_HojaDeVida_HojaDeVidaId",
                        column: x => x.HojaDeVidaId,
                        principalTable: "HojaDeVida",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReferenciasPersonales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Relacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HojaDeVidaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferenciasPersonales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenciasPersonales_HojaDeVida_HojaDeVidaId",
                        column: x => x.HojaDeVidaId,
                        principalTable: "HojaDeVida",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExperienciasProfesionales_HojaDeVidaId",
                table: "ExperienciasProfesionales",
                column: "HojaDeVidaId");

            migrationBuilder.CreateIndex(
                name: "IX_FormacionesAcademicas_HojaDeVidaId",
                table: "FormacionesAcademicas",
                column: "HojaDeVidaId");

            migrationBuilder.CreateIndex(
                name: "IX_Idiomas_HojaDeVidaId",
                table: "Idiomas",
                column: "HojaDeVidaId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferenciasPersonales_HojaDeVidaId",
                table: "ReferenciasPersonales",
                column: "HojaDeVidaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExperienciasProfesionales");

            migrationBuilder.DropTable(
                name: "FormacionesAcademicas");

            migrationBuilder.DropTable(
                name: "Idiomas");

            migrationBuilder.DropTable(
                name: "ReferenciasPersonales");

            migrationBuilder.DropTable(
                name: "HojaDeVida");
        }
    }
}
