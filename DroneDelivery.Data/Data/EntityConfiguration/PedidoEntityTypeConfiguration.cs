using DroneDelivery.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DroneDelivery.Data.Data.EntityConfiguration
{
    internal class PedidoEntityTypeConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(e => e.Peso)
                    .IsRequired();

            builder.Property(e => e.DataPedido)
                    .IsRequired();

            builder.Property(e => e.Valor)
                    .IsRequired();

            builder.Property(e => e.Status)
                    .IsRequired();

            builder.HasMany(s => s.HistoricoPedidos);

            var navigation = builder.Metadata.FindNavigation(nameof(Pedido.HistoricoPedidos));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

        }
    }
}
