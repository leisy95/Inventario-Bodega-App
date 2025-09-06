using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventarioBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddReferenciaPesoToInventarioItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReferenciaPeso",
                table: "InventarioItems",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferenciaPeso",
                table: "InventarioItems");
        }
    }
}
