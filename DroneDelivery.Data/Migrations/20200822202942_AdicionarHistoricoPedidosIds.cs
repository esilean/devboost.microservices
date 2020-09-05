using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DroneDelivery.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class AdicionarHistoricoPedidosIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoricoPedidos_Drones_DroneId",
                table: "HistoricoPedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricoPedidos_Pedidos_PedidoId",
                table: "HistoricoPedidos");

            migrationBuilder.AlterColumn<Guid>(
                name: "PedidoId",
                table: "HistoricoPedidos",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DroneId",
                table: "HistoricoPedidos",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoricoPedidos_Drones_DroneId",
                table: "HistoricoPedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricoPedidos_Pedidos_PedidoId",
                table: "HistoricoPedidos");

            migrationBuilder.AlterColumn<Guid>(
                name: "PedidoId",
                table: "HistoricoPedidos",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "DroneId",
                table: "HistoricoPedidos",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricoPedidos_Drones_DroneId",
                table: "HistoricoPedidos",
                column: "DroneId",
                principalTable: "Drones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricoPedidos_Pedidos_PedidoId",
                table: "HistoricoPedidos",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
