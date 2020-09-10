using DroneDelivery.Shared.Domain.Core.Domain;
using DroneDelivery.Shared.Domain.Core.Enums;
using System;
using System.Collections.Generic;

namespace DroneDelivery.Domain.Models
{
    public class Pedido : Entity, IAggregateRoot
    {

        private readonly List<HistoricoPedido> _historicoPedidos = new List<HistoricoPedido>();

        public Guid UsuarioId { get; private set; }
        public Usuario Usuario { get; private set; }

        public double Peso { get; private set; }
        public DateTime DataPedido { get; private set; }
        public double Valor { get; private set; }
        public PedidoStatus Status { get; private set; }

        public Guid? DroneId { get; private set; }
        public Drone Drone { get; private set; }

        public IReadOnlyCollection<HistoricoPedido> HistoricoPedidos => _historicoPedidos;

        protected Pedido() { }

        private Pedido(double peso, double valor, Usuario usuario)
        {
            Peso = peso;
            Valor = valor;
            Usuario = usuario;
            DataPedido = DateTime.Now;
            Status = PedidoStatus.Pendente;
        }

        public static Pedido Criar(double peso, double valor, Usuario usuario)
        {
            return new Pedido(peso, valor, usuario);
        }

        public void AtualizarStatusPedido(PedidoStatus status)
        {
            Status = status;
        }

        public void AssociarDrone(Drone drone)
        {
            Drone = drone;
        }

        public void CriarHistorico(HistoricoPedido historicoPedido)
        {
            _historicoPedidos.Add(historicoPedido);
        }

    }
}
