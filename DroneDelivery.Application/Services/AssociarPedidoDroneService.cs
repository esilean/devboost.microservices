using DroneDelivery.Application.Configs;
using DroneDelivery.Application.Services.Interfaces;
using DroneDelivery.Data.Repositorios.Interfaces;
using DroneDelivery.Domain.Enum;
using DroneDelivery.Domain.Interfaces;
using DroneDelivery.Domain.Models;
using DroneDelivery.Shared.Domain.Core.Domain;
using DroneDelivery.Shared.Domain.Core.Validator;
using DroneDelivery.Shared.Utility.Messages;
using Flunt.Notifications;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace DroneDelivery.Application.Services
{
    public class AssociarPedidoDroneService : ValidatorResponse, IAssociarPedidoDroneService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICalcularTempoEntrega _calcularTempoEntrega;
        private readonly IOptions<DronePontoInicialConfig> _dronePontoInicialConfig;

        public AssociarPedidoDroneService(IUnitOfWork unitOfWork, IOptions<DronePontoInicialConfig> dronePontoInicialConfig, ICalcularTempoEntrega calcularTempoEntrega)
        {
            _unitOfWork = unitOfWork;
            _calcularTempoEntrega = calcularTempoEntrega;
            _dronePontoInicialConfig = dronePontoInicialConfig;
        }

        public async Task<Drone> AssociarPedidoAoDrone(Pedido pedido)
        {
            //temos que olhar TODOS os drones, e nao somente os disponiveis, pq se todos estiverem ocupados... 
            //ainda sim, precisamos validar se temos capacidade de entregar o pedido
            var drones = await _unitOfWork.Drones.ObterTodosAsync();

            Drone droneDisponivel = null;
            foreach (var drone in drones)
            {

                //valida se algum drone tem autonomia e aceita capacidade para entregar o pedido
                var droneTemAutonomia = drone.ValidarAutonomia(_calcularTempoEntrega, _dronePontoInicialConfig.Value.Latitude, _dronePontoInicialConfig.Value.Longitude, pedido.Usuario.Latitude, pedido.Usuario.Longitude);
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
                if (!drone.ValidarAutonomiaSobraPorPontoEntrega(_calcularTempoEntrega, _dronePontoInicialConfig.Value.Latitude, _dronePontoInicialConfig.Value.Longitude, pedido.Usuario.Latitude, pedido.Usuario.Longitude))
                    continue;

                droneDisponivel = drone;
                break;
            }

            return droneDisponivel;
        }
    }
}
