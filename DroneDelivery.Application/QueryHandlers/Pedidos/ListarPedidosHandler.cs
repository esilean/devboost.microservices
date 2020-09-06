using AutoMapper;
using DroneDelivery.Application.Dtos.Pedido;
using DroneDelivery.Application.Interfaces;
using DroneDelivery.Application.Queries.Pedidos;
using DroneDelivery.Data.Repositorios.Interfaces;
using DroneDelivery.Domain.Models;
using DroneDelivery.Shared.Domain.Core.Domain;
using DroneDelivery.Shared.Domain.Core.Validator;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DroneDelivery.Application.QueryHandlers.Pedidos
{
    public class ListarPedidosHandler : ValidatorResponse, IRequestHandler<PedidosQuery, ResponseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUsuarioAutenticado _usuarioAutenticado;

        public ListarPedidosHandler(IUnitOfWork unitOfWork, IMapper mapper, IUsuarioAutenticado usuarioAutenticado)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _usuarioAutenticado = usuarioAutenticado;
        }


        public async Task<ResponseResult> Handle(PedidosQuery request, CancellationToken cancellationToken)
        {

            var pedidos = await _unitOfWork.Pedidos.ObterDoClienteAsync(_usuarioAutenticado.GetCurrentId());

            _response.AddValue(new
            {
                pedidos = _mapper.Map<IEnumerable<Pedido>, IEnumerable<PedidoDto>>(pedidos)
            });

            return _response;
        }
    }
}
