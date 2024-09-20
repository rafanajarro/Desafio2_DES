using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiDesafio2.Migrations
{
    /// <inheritdoc />
    public partial class Nuevousuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "usuario",
                table: "HojaDeVida",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "usuario",
                table: "HojaDeVida");
        }
    }
}
