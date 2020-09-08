using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DroneDelivery.Pagamento.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class AdicionarCamposPagamento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataConfirmacao",
                table: "Pagamentos",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "Pagamentos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataConfirmacao",
                table: "Pagamentos");

            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "Pagamentos");
        }
    }
}
