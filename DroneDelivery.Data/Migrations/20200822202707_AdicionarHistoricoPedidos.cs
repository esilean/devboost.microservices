using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DroneDelivery.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class AdicionarHistoricoPedidos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HistoricoPedidos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DroneId = table.Column<Guid>(nullable: true),
                    PedidoId = table.Column<Guid>(nullable: true),
                    DataSaida = table.Column<DateTime>(nullable: false),
                    DataEntrega = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoPedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoPedidos_Drones_DroneId",
                        column: x => x.DroneId,
                        principalTable: "Drones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HistoricoPedidos_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoPedidos_DroneId",
                table: "HistoricoPedidos",
                column: "DroneId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoPedidos_PedidoId",
                table: "HistoricoPedidos",
                column: "PedidoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoricoPedidos");
        }
    }
}
