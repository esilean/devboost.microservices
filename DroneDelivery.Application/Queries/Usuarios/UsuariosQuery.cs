using DroneDelivery.Domain.Core.Domain;
using DroneDelivery.Domain.Core.Queries;
using MediatR;

namespace DroneDelivery.Application.Queries.Usuarios
{
    public class UsuariosQuery : QueryFilter, IRequest<ResponseResult> { }
}
