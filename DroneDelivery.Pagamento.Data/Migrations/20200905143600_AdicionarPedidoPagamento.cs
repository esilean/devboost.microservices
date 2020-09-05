using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DroneDelivery.Pagamento.Data.Migrations
{
    public partial class AdicionarPedidoPagamento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pagamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PedidoId = table.Column<Guid>(nullable: false),
                    Valor = table.Column<double>(nullable: false),
                    NumeroCartao = table.Column<string>(nullable: true),
                    VencimentoCartao = table.Column<DateTime>(nullable: false),
                    CodigoSeguranca = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamentos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pagamentos");
        }
    }
}
