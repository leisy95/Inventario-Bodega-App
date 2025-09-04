using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventarioBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddInventarioItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventarioItems_TiposProducto_IdTipoProducto",
                table: "InventarioItems");

            migrationBuilder.RenameColumn(
                name: "IdTipoProducto",
                table: "InventarioItems",
                newName: "IdInventario");

            migrationBuilder.RenameIndex(
                name: "IX_InventarioItems_IdTipoProducto",
                table: "InventarioItems",
                newName: "IX_InventarioItems_IdInventario");

            migrationBuilder.AddColumn<int>(
                name: "TipoProductoIdTipo",
                table: "InventarioItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventarioItems_TipoProductoIdTipo",
                table: "InventarioItems",
                column: "TipoProductoIdTipo");

            migrationBuilder.AddForeignKey(
                name: "FK_InventarioItems_Inventario_IdInventario",
                table: "InventarioItems",
                column: "IdInventario",
                principalTable: "Inventario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventarioItems_TiposProducto_TipoProductoIdTipo",
                table: "InventarioItems",
                column: "TipoProductoIdTipo",
                principalTable: "TiposProducto",
                principalColumn: "IdTipo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventarioItems_Inventario_IdInventario",
                table: "InventarioItems");

            migrationBuilder.DropForeignKey(
                name: "FK_InventarioItems_TiposProducto_TipoProductoIdTipo",
                table: "InventarioItems");

            migrationBuilder.DropIndex(
                name: "IX_InventarioItems_TipoProductoIdTipo",
                table: "InventarioItems");

            migrationBuilder.DropColumn(
                name: "TipoProductoIdTipo",
                table: "InventarioItems");

            migrationBuilder.RenameColumn(
                name: "IdInventario",
                table: "InventarioItems",
                newName: "IdTipoProducto");

            migrationBuilder.RenameIndex(
                name: "IX_InventarioItems_IdInventario",
                table: "InventarioItems",
                newName: "IX_InventarioItems_IdTipoProducto");

            migrationBuilder.AddForeignKey(
                name: "FK_InventarioItems_TiposProducto_IdTipoProducto",
                table: "InventarioItems",
                column: "IdTipoProducto",
                principalTable: "TiposProducto",
                principalColumn: "IdTipo",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
