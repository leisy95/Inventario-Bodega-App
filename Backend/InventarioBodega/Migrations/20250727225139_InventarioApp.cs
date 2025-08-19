using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventarioBackend.Migrations
{
    /// <inheritdoc />
    public partial class InventarioApp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TiposProducto",
                columns: table => new
                {
                    IdTipo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Referencia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Medida = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UnidadPeso = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Ancho = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Largo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Calibre = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Material = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Mezcla = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Toquel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposProducto", x => x.IdTipo);
                });

            migrationBuilder.CreateTable(
                name: "InventarioItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTipoProducto = table.Column<int>(type: "int", nullable: false),
                    PesoActual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaRegistroItem = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventarioItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventarioItems_TiposProducto_IdTipoProducto",
                        column: x => x.IdTipoProducto,
                        principalTable: "TiposProducto",
                        principalColumn: "IdTipo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovimientosInventario",
                columns: table => new
                {
                    IdMovimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdItem = table.Column<int>(type: "int", nullable: false),
                    FechaMovimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoMovimiento = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CantidadPeso = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientosInventario", x => x.IdMovimiento);
                    table.ForeignKey(
                        name: "FK_MovimientosInventario_InventarioItems_IdItem",
                        column: x => x.IdItem,
                        principalTable: "InventarioItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventarioItems_IdTipoProducto",
                table: "InventarioItems",
                column: "IdTipoProducto");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosInventario_IdItem",
                table: "MovimientosInventario",
                column: "IdItem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovimientosInventario");

            migrationBuilder.DropTable(
                name: "InventarioItems");

            migrationBuilder.DropTable(
                name: "TiposProducto");
        }
    }
}
