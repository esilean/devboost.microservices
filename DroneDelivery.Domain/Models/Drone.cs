using DroneDelivery.Domain.Enum;
using DroneDelivery.Domain.Interfaces;
using DroneDelivery.Shared.Domain.Core.Domain;
using DroneDelivery.Shared.Domain.Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace DroneDelivery.Domain.Models
{
    public class Drone : Entity, IAggregateRoot
    {

        private readonly List<Pedido> _pedidos = new List<Pedido>();

        public double Capacidade { get; private set; }

        public double Velocidade { get; private set; }

        public double Autonomia { get; private set; }

        public double Carga { get; private set; }

        public IReadOnlyCollection<Pedido> Pedidos => _pedidos;

        public DroneStatus Status { get; private set; }

        protected Drone() { }

        private Drone(double capacidade, double velocidade, double autonomia, double carga, DroneStatus status)
        {
            Capacidade = capacidade;
            Velocidade = velocidade;
            Autonomia = autonomia;
            Carga = carga;
            Status = status;
        }

        public static Drone Criar(double capacidade, double velocidade, double autonomia, double carga, DroneStatus status)
        {
            return new Drone(capacidade, velocidade, autonomia, carga, status);
        }
        public bool AdicionarPedido(Pedido pedido, ICalcularTempoEntrega calcularTempoEntrega, double latitudeOrigem, double longitudeOrigem)
        {

            var temCapacidade = ValidarCapacidadeSobra(pedido.Peso);
            if (!temCapacidade)
                return false;

            var temAutonomia = ValidarAutonomiaSobraPorPontoEntrega(calcularTempoEntrega, latitudeOrigem, longitudeOrigem, pedido.Usuario.Latitude, pedido.Usuario.Longitude);
            if (!temAutonomia)
                return false;


            _pedidos.Add(pedido);
            return true;
        }

        public void RemoverPedido(Pedido pedido)
        {
            _pedidos.Remove(pedido);
        }

        public bool ValidarCapacidadeSobra(double pesoPedido)
        {
            double capacidadeAtual = 0;
            foreach (var pedido in Pedidos.Where(x => x.Status == PedidoStatus.EmEntrega))
                capacidadeAtual += pedido.Peso;


            //adicioar o peso do novo pedido
            capacidadeAtual += pesoPedido;

            return (Capacidade - capacidadeAtual) >= 0;
        }

        public bool ValidarAutonomia(ICalcularTempoEntrega calcularTempoEntrega, double latitudeOrigem, double longitudeOrigem, double latitudeDestino, double longitudeDestino)
        {
            var tempoEmMinutos = calcularTempoEntrega.ObterTempoEntregaEmMinutos(latitudeOrigem, longitudeOrigem, latitudeDestino, longitudeDestino, Velocidade);

            // nao entrega
            if (tempoEmMinutos == 0)
                return false;

            return tempoEmMinutos <= Autonomia;
        }


        public bool ValidarAutonomiaSobraPorPontoEntrega(ICalcularTempoEntrega calcularTempoEntrega, double latitudeOrigem, double longitudeOrigem, double latitudeDestino, double longitudeDestino)
        {
            var ultimaLatitude = latitudeOrigem;
            var ultimaLongitude = longitudeOrigem;

            //obter tempo entrega dos pedidos que ja estao no drone
            double tempoEntregaAtual = 0;
            foreach (var cliente in Pedidos.Where(x => x.Status == PedidoStatus.EmEntrega).Select(x => x.Usuario))
            {
                tempoEntregaAtual += calcularTempoEntrega.ObterTempoEntregaEmMinutosIda(ultimaLatitude, ultimaLongitude, cliente.Latitude, cliente.Longitude, Velocidade);
                ultimaLatitude = cliente.Latitude;
                ultimaLongitude = cliente.Longitude;
            }

            //ida do ultimo pedido até o novo pedido
            var tempoIdaUltimoPedido = calcularTempoEntrega.ObterTempoEntregaEmMinutosIda(ultimaLatitude, ultimaLongitude, latitudeDestino, longitudeDestino, Velocidade);

            //volta pra base
            var tempoVoltaUltimoPedido = calcularTempoEntrega.ObterTempoEntregaEmMinutosIda(latitudeDestino, longitudeDestino, latitudeOrigem, longitudeOrigem, Velocidade);

            // somar com o tempo do novo pedido
            tempoEntregaAtual += tempoIdaUltimoPedido;
            tempoEntregaAtual += tempoVoltaUltimoPedido;

            //obter autonomia atual considerando a bateria do drone
            var autonomialAtual = Autonomia * Carga / 100;

            return (autonomialAtual - tempoEntregaAtual) >= 0;
        }

        public bool VerificarDroneAceitaOPesoPedido(double pesoPedido)
        {
            return Capacidade >= pesoPedido;
        }


        public void AtualizarStatus(DroneStatus status)
        {
            Status = status;
        }

        public void SepararPedidosParaEntrega()
        {
            _pedidos.RemoveAll(x => x.Status != PedidoStatus.EmEntrega);
        }

    }
}
