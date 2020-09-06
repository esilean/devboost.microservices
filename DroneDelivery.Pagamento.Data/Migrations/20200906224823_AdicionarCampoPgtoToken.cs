using Microsoft.EntityFrameworkCore.Migrations;

namespace DroneDelivery.Pagamento.Data.Migrations
{
    public partial class AdicionarCampoPgtoToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Pagamentos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "Pagamentos");
        }
    }
}
