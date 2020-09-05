using DroneDelivery.Application.Commands.Pedidos;
using DroneDelivery.Application.Configs;
using DroneDelivery.Data.Repositorios.Interfaces;
using DroneDelivery.Domain.Core.Domain;
using DroneDelivery.Domain.Core.Validator;
using DroneDelivery.Domain.Enum;
using DroneDelivery.Domain.Interfaces;
using DroneDelivery.Domain.Models;
using DroneDelivery.Utility.Messages;
using Flunt.Notifications;
using MediatR;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DroneDelivery.Application.CommandHandlers.Pedidos
{
    public class AtualizarPedidoStatusHandler : ValidatorResponse, IRequestHandler<AtualizarPedidoStatusCommand, ResponseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICalcularTempoEntrega _calcularTempoEntrega;
        private readonly IOptions<DronePontoInicialConfig> _dronePontoInicialConfig;

        public AtualizarPedidoStatusHandler(IUnitOfWork unitOfWork, IOptions<DronePontoInicialConfig> dronePontoInicialConfig, ICalcularTempoEntrega calcularTempoEntrega)
        {
            _unitOfWork = unitOfWork;
            _calcularTempoEntrega = calcularTempoEntrega;
            _dronePontoInicialConfig = dronePontoInicialConfig;
        }

        public async Task<ResponseResult> Handle(AtualizarPedidoStatusCommand request, CancellationToken cancellationToken)
        {
            request.Validate();
            if (request.Invalid)
            {
                _response.AddNotifications(request.Notifications);
                return _response;
            }

            var pedido = await _unitOfWork.Pedidos.ObterPorIdAsync(request.Id);
            if (pedido == null)
            {
                _response.AddNotification(new Notification("pedido", "pedido não foi encontrado"));
                return _response;
            }

            if (pedido.Status != PedidoStatus.AguardandoPagamento)
            {
                _response.AddNotification(new Notification("pedido", $"o status do pedido é {pedido.Status}. não pode ser alterado."));
                return _response;
            }


            if (request.Status == PedidoStatus.Pago)
            {
                //temos que olhar TODOS os drones, e nao somente os disponiveis, pq se todos estiverem ocupados... 
                //ainda sim, precisamos validar se temos capacidade de entregar o pedido
                var drones = await _unitOfWork.Drones.ObterTodosAsync();
                if (!drones.Any())
                {
                    _response.AddNotification(new Notification("pedido", PedidoMessage.Erro_DroneNaoCadastrado));
                    return _response;
                }

                var cliente = await _unitOfWork.Usuarios.ObterPorIdAsync(pedido.UsuarioId.GetValueOrDefault());
                if (cliente == null)
                {
                    _response.AddNotification(new Notification("pedido", PedidoMessage.Erro_ClienteNaoEncontrado));
                    return _response;
                }

                // temos que procurar drones disponiveis
                Drone droneDisponivel = null;
                foreach (var drone in drones)
                {

                    //valida se algum drone tem autonomia e aceita capacidade para entregar o pedido
                    var droneTemAutonomia = drone.ValidarAutonomia(_calcularTempoEntrega, _dronePontoInicialConfig.Value.Latitude, _dronePontoInicialConfig.Value.Longitude, cliente.Latitude, cliente.Longitude);
                    var droneAceitaPeso = drone.VerificarDroneAceitaOPesoPedido(pedido.Peso);
                    if (!droneTemAutonomia || !droneAceitaPeso)
                        continue;

                    //verificar se tem algum drone disponivel
                    if (drone.Status != DroneStatus.Livre)
                        continue;

                    //verifica se o drone possui espaço para adicionar mais peso
                    if (!drone.ValidarCapacidadeSobra(pedido.Peso))
                        continue;

                    //verifica se o drone possui autonomia para enttregar o pedido
                    if (!drone.ValidarAutonomiaSobraPorPontoEntrega(_calcularTempoEntrega, _dronePontoInicialConfig.Value.Latitude, _dronePontoInicialConfig.Value.Longitude, cliente.Latitude, cliente.Longitude))
                        continue;

                    droneDisponivel = drone;
                    break;
                }

                if (droneDisponivel == null)
                    pedido.AtualizarStatusPedido(PedidoStatus.AguardandoEntrega);
                else
                {
                    pedido.AtualizarStatusPedido(PedidoStatus.EmEntrega);
                    pedido.AssociarDrone(droneDisponivel);
                }
            }
            else
            {
                pedido.AtualizarStatusPedido(PedidoStatus.Cancelado);
            }

            await _unitOfWork.SaveAsync();

            return _response;
        }
    }
}
