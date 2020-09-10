using DroneDelivery.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DroneDelivery.Data.Data.EntityConfiguration
{
    internal class HistoricoPedidosEntityTypeConfiguration : IEntityTypeConfiguration<HistoricoPedido>
    {
        public void Configure(EntityTypeBuilder<HistoricoPedido> builder)
        {
            builder.ToTable("HistoricoPedidos", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(e => e.DataSaida).IsRequired();
            builder.Property(e => e.DataEntrega);

            builder.HasOne(e => e.Drone)
                .WithMany()
                .HasForeignKey(x => x.DroneId);

        }
    }
}
