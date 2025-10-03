using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventarioBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddClienteToMovimientoInventario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClienteId",
                table: "Salidas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClienteId",
                table: "MovimientosInventario",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Salidas_ClienteId",
                table: "Salidas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosInventario_ClienteId",
                table: "MovimientosInventario",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientosInventario_Clientes_ClienteId",
                table: "MovimientosInventario",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Salidas_Clientes_ClienteId",
                table: "Salidas",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovimientosInventario_Clientes_ClienteId",
                table: "MovimientosInventario");

            migrationBuilder.DropForeignKey(
                name: "FK_Salidas_Clientes_ClienteId",
                table: "Salidas");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropIndex(
                name: "IX_Salidas_ClienteId",
                table: "Salidas");

            migrationBuilder.DropIndex(
                name: "IX_MovimientosInventario_ClienteId",
                table: "MovimientosInventario");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "Salidas");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "MovimientosInventario");
        }
    }
}
