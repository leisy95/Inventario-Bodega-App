using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventarioBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inventario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Referencia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TipodeBolsa = table.Column<string>(name: "Tipo de Bolsa", type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TipoMaterial = table.Column<string>(name: "Tipo Material", type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Densidad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Segundocolor = table.Column<string>(name: "Segundo color", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImpresoNo = table.Column<string>(name: "Impreso/No", type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Ancho = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Alto = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Calibre = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Peso = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    ReferenciaNormalizada = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventarioItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdInventario = table.Column<int>(type: "int", nullable: false),
                    ReferenciaPeso = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PesoActual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaRegistroItem = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventarioItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventarioItems_Inventario_IdInventario",
                        column: x => x.IdInventario,
                        principalTable: "Inventario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovimientosInventario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Referencia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReferenciaPeso = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Peso = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Usuario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdInventario = table.Column<int>(type: "int", nullable: false),
                    IdInventarioItem = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientosInventario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimientosInventario_InventarioItems_IdInventarioItem",
                        column: x => x.IdInventarioItem,
                        principalTable: "InventarioItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimientosInventario_Inventario_IdInventario",
                        column: x => x.IdInventario,
                        principalTable: "Inventario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventarioItems_IdInventario",
                table: "InventarioItems",
                column: "IdInventario");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosInventario_IdInventario",
                table: "MovimientosInventario",
                column: "IdInventario");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosInventario_IdInventarioItem",
                table: "MovimientosInventario",
                column: "IdInventarioItem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovimientosInventario");

            migrationBuilder.DropTable(
                name: "InventarioItems");

            migrationBuilder.DropTable(
                name: "Inventario");
        }
    }
}
