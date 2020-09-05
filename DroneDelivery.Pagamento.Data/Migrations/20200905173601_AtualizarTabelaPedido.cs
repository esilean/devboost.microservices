using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace DroneDelivery.Pagamento.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class AtualizarTabelaPedido : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Valor",
                table: "Pagamentos");

            migrationBuilder.AddColumn<double>(
                name: "Valor",
                table: "Pedidos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ValorPago",
                table: "Pagamentos",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Valor",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "ValorPago",
                table: "Pagamentos");

            migrationBuilder.AddColumn<double>(
                name: "Valor",
                table: "Pagamentos",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
