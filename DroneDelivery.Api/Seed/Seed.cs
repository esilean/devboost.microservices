using DroneDelivery.Data.Data;
using DroneDelivery.Domain.Models;
using DroneDelivery.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using DroneDelivery.Application.Interfaces;

namespace DroneDelivery.Api.Seed
{
    public class Seed
    {
        public static void SeedData(DroneDbContext context, IPasswordHasher<Usuario> passwordHasher, IGeradorToken geradorToken)
        {
            var admin = context.Usuarios.FirstOrDefault(x => (int)x.Role == (int)UsuarioRole.Admin);

            if (admin == null)
            {
                admin = Usuario.Criar("Admin", "admin@test.com", -23.5753639, -46.645421, UsuarioRole.Admin);
                var passwordHash = passwordHasher.HashPassword(admin, "123");
                admin.AdicionarPassword(passwordHash);

                var token = geradorToken.GerarRefreshToken(admin);
                admin.AdicionarRefreshToken(token.Token, token.DataExpiracao);

                context.Usuarios.Add(admin);
                context.SaveChanges();
            }
        }
    }
}
