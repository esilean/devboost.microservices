using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace DroneDelivery.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class AtualizarTabelasPedido : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoricoPedidos_Drones_DroneId",
                table: "HistoricoPedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricoPedidos_Pedidos_PedidoId",
                table: "HistoricoPedidos");

            migrationBuilder.DropIndex(
                name: "IX_HistoricoPedidos_DroneId",
                table: "HistoricoPedidos");

            migrationBuilder.DropIndex(
                name: "IX_HistoricoPedidos_PedidoId",
                table: "HistoricoPedidos");

            migrationBuilder.AddColumn<double>(
                name: "Valor",
                table: "Pedidos",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Valor",
                table: "Pedidos");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoPedidos_DroneId",
                table: "HistoricoPedidos",
                column: "DroneId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoPedidos_PedidoId",
                table: "HistoricoPedidos",
                column: "PedidoId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricoPedidos_Drones_DroneId",
                table: "HistoricoPedidos",
                column: "DroneId",
                principalTable: "Drones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricoPedidos_Pedidos_PedidoId",
                table: "HistoricoPedidos",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
