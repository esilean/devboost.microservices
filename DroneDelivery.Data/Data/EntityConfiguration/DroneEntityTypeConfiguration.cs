using DroneDelivery.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DroneDelivery.Data.Data.EntityConfiguration
{
    internal class DroneEntityTypeConfiguration : IEntityTypeConfiguration<Drone>
    {
        public void Configure(EntityTypeBuilder<Drone> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(e => e.Capacidade)
                    .IsRequired();

            builder.Property(e => e.Velocidade)
                    .IsRequired();

            builder.Property(e => e.Autonomia)
                    .IsRequired();

            builder.Property(e => e.Carga)
                    .IsRequired();

            builder.Property(e => e.Status)
                    .IsRequired();

            builder.HasMany(x => x.Pedidos)
                    .WithOne(x => x.Drone)
                    .HasForeignKey(x => x.DroneId);

        }
    }
}
