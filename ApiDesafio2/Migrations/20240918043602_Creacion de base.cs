using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiDesafio2.Migrations
{
    /// <inheritdoc />
    public partial class Creaciondebase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OfertasEmpleo",
                columns: table => new
                {
                    OfertaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreOferta = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescripcionOferta = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Requisitos = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Salario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaPublicacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Contacto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfertasEmpleo", x => x.OfertaId);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorreoElectronico = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RolUsuario = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.UsuarioId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfertasEmpleo");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
