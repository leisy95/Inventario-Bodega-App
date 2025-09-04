using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventarioBackend.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIdTipoProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventarioItems_TiposProducto_TipoProductoIdTipo",
                table: "InventarioItems");

            migrationBuilder.DropTable(
                name: "TiposProducto");

            migrationBuilder.DropIndex(
                name: "IX_InventarioItems_TipoProductoIdTipo",
                table: "InventarioItems");

            migrationBuilder.DropColumn(
                name: "TipoProductoIdTipo",
                table: "InventarioItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoProductoIdTipo",
                table: "InventarioItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TiposProducto",
                columns: table => new
                {
                    IdTipo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Alto = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Ancho = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Calibre = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Densidad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ImpresoNo = table.Column<string>(name: "Impreso/No", type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Referencia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Segundocolor = table.Column<string>(name: "Segundo color", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TipodeBolsa = table.Column<string>(name: "Tipo de Bolsa", type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TipoMaterial = table.Column<string>(name: "Tipo Material", type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposProducto", x => x.IdTipo);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventarioItems_TipoProductoIdTipo",
                table: "InventarioItems",
                column: "TipoProductoIdTipo");

            migrationBuilder.AddForeignKey(
                name: "FK_InventarioItems_TiposProducto_TipoProductoIdTipo",
                table: "InventarioItems",
                column: "TipoProductoIdTipo",
                principalTable: "TiposProducto",
                principalColumn: "IdTipo");
        }
    }
}
