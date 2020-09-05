﻿using DroneDelivery.Application.Commands.Pedidos;
using DroneDelivery.Application.Configs;
using DroneDelivery.Application.Interfaces;
using DroneDelivery.Data.Repositorios.Interfaces;
using DroneDelivery.Domain.Core.Domain;
using DroneDelivery.Domain.Core.Validator;
using DroneDelivery.Domain.Models;
using DroneDelivery.Domain.Enum;
using DroneDelivery.Domain.Interfaces;
using Flunt.Notifications;
using MediatR;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DroneDelivery.Utility.Messages;

namespace DroneDelivery.Application.CommandHandlers.Pedidos
{
    public class CriarPedidoHandler : ValidatorResponse, IRequestHandler<CriarPedidoCommand, ResponseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsuarioAutenticado _usuarioAutenticado;

        public CriarPedidoHandler(IUnitOfWork unitOfWork, IUsuarioAutenticado usuarioAutenticado)
        {
            _unitOfWork = unitOfWork;
            _usuarioAutenticado = usuarioAutenticado;
        }


        public async Task<ResponseResult> Handle(CriarPedidoCommand request, CancellationToken cancellationToken)
        {
            request.Validate();
            if (request.Notifications.Any())
            {
                _response.AddNotifications(request.Notifications);
                return _response;
            }

            var clienteId = _usuarioAutenticado.GetCurrentId();
            var cliente = await _unitOfWork.Usuarios.ObterPorIdAsync(clienteId);
            if (cliente == null)
            {
                _response.AddNotification(new Notification("pedido", PedidoMessage.Erro_ClienteNaoEncontrado));
                return _response;
            }

            var pedido = Pedido.Criar(request.Peso, cliente);
            pedido.AtualizarStatusPedido(PedidoStatus.AguardandoPagamento);
            

            await _unitOfWork.Pedidos.AdicionarAsync(pedido);
            await _unitOfWork.SaveAsync();

            return _response;
        }
    }
}
