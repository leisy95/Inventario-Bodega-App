using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventarioBackend.Migrations
{
    /// <inheritdoc />
    public partial class RenameFechaConfirmacionToFechaSalida : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FechaConfirmacion",
                table: "Salidas",
                newName: "FechaSalida");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FechaSalida",
                table: "Salidas",
                newName: "FechaConfirmacion");
        }
    }
}
