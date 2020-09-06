using DroneDelivery.Shared.Domain.Core.Domain;
using DroneDelivery.Shared.Domain.Core.Queries;
using MediatR;

namespace DroneDelivery.Application.Queries.Usuarios
{
    public class UsuariosQuery : Query, IRequest<ResponseResult> { }
}
