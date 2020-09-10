using DroneDelivery.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DroneDelivery.Data.Data.EntityConfiguration
{
    internal class UsuarioEntityTypeConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(100);
            builder.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.Property(e => e.RefreshToken)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.Property(e => e.RefreshTokenExpiracao);

            builder.Property(e => e.Latitude)
                    .IsRequired();
            builder.Property(e => e.Longitude)
                    .IsRequired();

            builder.Property(e => e.Role)
                    .IsRequired();
        }
    }
}
