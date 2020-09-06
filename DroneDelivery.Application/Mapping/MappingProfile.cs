using AutoMapper;
using DroneDelivery.Application.Commands.Drones;
using DroneDelivery.Application.Dtos.Drone;
using DroneDelivery.Application.Dtos.Pedido;
using DroneDelivery.Application.Dtos.Usuario;
using DroneDelivery.Domain.Models;
using DroneDelivery.Domain.Enum;

namespace DroneDelivery.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Commands 
            CreateMap<CriarDroneCommand, Drone>()
                .ConstructUsing(c => Drone.Criar(c.Capacidade, c.Velocidade, c.Autonomia, c.Carga, DroneStatus.Livre));

            //Domain to Model
            CreateMap<Drone, DroneDto>()
                .ForMember(d => d.Status, opts => opts.MapFrom(x => x.Status.ToString()));
            CreateMap<Pedido, PedidoDto>();

            CreateMap<Pedido, DronePedidoDto>();
            CreateMap<Usuario, UsuarioDto>();

            CreateMap<Drone, DroneSituacaoDto>()
                .ForMember(d => d.Situacao, opts => opts.MapFrom(x => x.Status.ToString()))
                .ForMember(d => d.Pedidos, opts => opts.MapFrom(x => x.Pedidos));
        }

    }
}
