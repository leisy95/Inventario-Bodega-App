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
            migrationBuilder.DropColumn(
                name: "Largo",
                table: "TiposProducto");

            migrationBuilder.DropColumn(
                name: "Material",
                table: "TiposProducto");

            migrationBuilder.DropColumn(
                name: "Medida",
                table: "TiposProducto");

            migrationBuilder.DropColumn(
                name: "Mezcla",
                table: "TiposProducto");

            migrationBuilder.DropColumn(
                name: "Peso",
                table: "TiposProducto");

            migrationBuilder.DropColumn(
                name: "Toquel",
                table: "TiposProducto");

            migrationBuilder.DropColumn(
                name: "UnidadPeso",
                table: "TiposProducto");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "TiposProducto",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Calibre",
                table: "TiposProducto",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Ancho",
                table: "TiposProducto",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Alto",
                table: "TiposProducto",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Densidad",
                table: "TiposProducto",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "TiposProducto",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Impreso/No",
                table: "TiposProducto",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Segundo color",
                table: "TiposProducto",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Tipo Material",
                table: "TiposProducto",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Tipo de Bolsa",
                table: "TiposProducto",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inventario");

            migrationBuilder.DropColumn(
                name: "Alto",
                table: "TiposProducto");

            migrationBuilder.DropColumn(
                name: "Densidad",
                table: "TiposProducto");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "TiposProducto");

            migrationBuilder.DropColumn(
                name: "Impreso/No",
                table: "TiposProducto");

            migrationBuilder.DropColumn(
                name: "Segundo color",
                table: "TiposProducto");

            migrationBuilder.DropColumn(
                name: "Tipo Material",
                table: "TiposProducto");

            migrationBuilder.DropColumn(
                name: "Tipo de Bolsa",
                table: "TiposProducto");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "TiposProducto",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<decimal>(
                name: "Calibre",
                table: "TiposProducto",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Ancho",
                table: "TiposProducto",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "Largo",
                table: "TiposProducto",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Material",
                table: "TiposProducto",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Medida",
                table: "TiposProducto",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mezcla",
                table: "TiposProducto",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Peso",
                table: "TiposProducto",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Toquel",
                table: "TiposProducto",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnidadPeso",
                table: "TiposProducto",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
