using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaDelivery.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaComandaPedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NomeComanda",
                table: "Pedidos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomeComanda",
                table: "Pedidos");
        }
    }
}
