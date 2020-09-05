using DroneDelivery.Domain.Core.Domain;
using DroneDelivery.Domain.Enum;
using System;

namespace DroneDelivery.Domain.Models
{
    public class Pedido : Entity, IAggregateRoot
    {

        public Guid? UsuarioId { get; private set; }
        public Usuario Usuario { get; private set; }

        public double Peso { get; private set; }

        public DateTime DataPedido { get; private set; }

        public PedidoStatus Status { get; private set; }

        public Guid? DroneId { get; private set; }

        public Drone Drone { get; private set; }

        protected Pedido() { }

        private Pedido(double peso, Usuario usuario)
        {
            Peso = peso;
            Usuario = usuario;
            DataPedido = DateTime.Now;
        }

        public static Pedido Criar(double peso, Usuario usuario)
        {
            return new Pedido(peso, usuario);
        }

        public void AtualizarStatusPedido(PedidoStatus status)
        {
            Status = status;
        }

        public void AssociarDrone(Drone drone)
        {
            Drone = drone;
        }

    }
}
