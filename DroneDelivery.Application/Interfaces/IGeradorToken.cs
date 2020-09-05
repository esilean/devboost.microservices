using DroneDelivery.Application.Dtos.Token;
using DroneDelivery.Domain.Models;

namespace DroneDelivery.Application.Interfaces
{
    public interface IGeradorToken
    {
        JsonWebTokenDto GerarToken(Usuario usuario);

        RefreshTokenDto GerarRefreshToken(Usuario usuario);
    }
}
