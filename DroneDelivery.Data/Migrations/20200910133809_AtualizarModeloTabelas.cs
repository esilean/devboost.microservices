using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace DroneDelivery.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class AtualizarModeloTabelas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "HistoricoPedidos",
                newName: "HistoricoPedidos",
                newSchema: "dbo");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "Usuarios",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Usuarios",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Usuarios",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Usuarios",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoPedidos_DroneId",
                schema: "dbo",
                table: "HistoricoPedidos",
                column: "DroneId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoPedidos_PedidoId",
                schema: "dbo",
                table: "HistoricoPedidos",
                column: "PedidoId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricoPedidos_Drones_DroneId",
                schema: "dbo",
                table: "HistoricoPedidos",
                column: "DroneId",
                principalTable: "Drones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricoPedidos_Pedidos_PedidoId",
                schema: "dbo",
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
                schema: "dbo",
                table: "HistoricoPedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricoPedidos_Pedidos_PedidoId",
                schema: "dbo",
                table: "HistoricoPedidos");

            migrationBuilder.DropIndex(
                name: "IX_HistoricoPedidos_DroneId",
                schema: "dbo",
                table: "HistoricoPedidos");

            migrationBuilder.DropIndex(
                name: "IX_HistoricoPedidos_PedidoId",
                schema: "dbo",
                table: "HistoricoPedidos");

            migrationBuilder.RenameTable(
                name: "HistoricoPedidos",
                schema: "dbo",
                newName: "HistoricoPedidos");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);
        }
    }
}
