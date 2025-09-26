using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventarioBackend.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFechaSalidaColumnFromInventarioItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaSalida",
                table: "InventarioItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaSalida",
                table: "InventarioItems",
                type: "datetime2",
                nullable: true);
        }
    }
}
