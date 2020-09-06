using DroneDelivery.Domain.Enum;
using DroneDelivery.Domain.Interfaces;
using DroneDelivery.Domain.Models;
using DroneDelivery.Shared.Domain.Core.Enums;
using Moq;
using System;
using Xunit;

namespace DroneDelivery.Domain.Tests.Models
{
    public class DroneTests
    {
        [Fact(DisplayName = "Criar um novo drone")]
        public void Drone_CriarDrone_DeveCriarUmDrone()
        {
            // Arrange
            // Act
            var drone = Drone.Criar(12000, 3, 35, 100, DroneStatus.Livre);

            // Assert
            Assert.Equal(12000, drone.Capacidade);
            Assert.Equal(3, drone.Velocidade);
            Assert.Equal(35, drone.Autonomia);
            Assert.Equal(100, drone.Carga);
            Assert.Equal(DroneStatus.Livre, drone.Status);
        }

        [Fact(DisplayName = "Deve retornar true se peso do pedido estiver na capacidade de sobra do drone (Quantas gramas ainda está disponível)")]
        public void Drone_ValidarCapacidaDeSobra_RetornarTrueSeDroneAceitaOPesoDoNovoPedido()
        {
            // Arrange
            var drone = Drone.Criar(12000, 3, 35, 100, DroneStatus.Livre);

            // Act
            var resultado = drone.ValidarCapacidadeSobra(5000);


            // Assert
            Assert.True(resultado);
        }

        [Theory(DisplayName = "Deve retornar resultado esperado validando a capacidade de sobra do drone (Quantas gramas ainda está disponível)")]
        [InlineData(8000, 5000, 4000, false)]
        [InlineData(8000, 5000, 3001, false)]
        [InlineData(8000, 5000, 3000, true)]
        [InlineData(8000, 1000, 3000, true)]
        public void Drone_ValidarCapacidaDeSobra_DeveRetornarResultadoEsperado(double capacidadeDrone, double capacidadeOcupadaDrone, double pesoNovoPedido, bool resultadoEsperado)
        {
            // Arrange

            var latitudeOrigem = -23.5880684;
            var longitudeOrigem = -46.6564195;
            var drone = Drone.Criar(capacidadeDrone, 3, 35, 100, DroneStatus.Livre);

            var usuario = Usuario.Criar("test", "test@test.com", -23.5950753, -46.645421, UsuarioRole.Cliente);

            //Calcular tempo entrega
            var calcularTempoEntrega = new Mock<ICalcularTempoEntrega>();
            calcularTempoEntrega.Setup(p => p.ObterTempoEntregaEmMinutosIda(latitudeOrigem, longitudeOrigem, usuario.Latitude, usuario.Longitude, drone.Velocidade));

            var pedido = Pedido.Criar(capacidadeOcupadaDrone, 1000, usuario);
            pedido.AtualizarStatusPedido(PedidoStatus.EmEntrega);
            drone.AdicionarPedido(pedido, calcularTempoEntrega.Object, latitudeOrigem, longitudeOrigem);

            // Act
            var resultado = drone.ValidarCapacidadeSobra(pesoNovoPedido);

            // Assert
            Assert.Equal(resultadoEsperado, resultado);
        }

        [Theory(DisplayName = "Deve retornar resultado esperado validando a autonomia de sobra do drone (É possível levar o novo pedido?)")]
        [InlineData(35, 100, 5, 10, 15, true)]
        [InlineData(29, 100, 5, 10, 15, false)]
        [InlineData(35, 100, 10, 15, 25, false)]
        public void Drone_ValidarAutonomiaDeSobra_DeveRetornarResultadoEsperado(double autonomia, double carga, double autonomiaUtilizada, double tempoDeIda, double tempoDeVolta, bool resultadoEsperado)
        {
            // Arrange
            var drone = Drone.Criar(12000, 3, autonomia, carga, DroneStatus.Livre);

            var usuario = Usuario.Criar("test", "test@test.com", It.IsAny<double>(), It.IsAny<double>(), UsuarioRole.Cliente);

            var calcularTempoEntrega = new Mock<ICalcularTempoEntrega>();
            calcularTempoEntrega.Setup(p => p.ObterTempoEntregaEmMinutosIda(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()));

            var pedido = Pedido.Criar(1000, 1000, usuario);
            pedido.AtualizarStatusPedido(PedidoStatus.EmEntrega);
            drone.AdicionarPedido(pedido, calcularTempoEntrega.Object, It.IsAny<double>(), It.IsAny<double>());

            calcularTempoEntrega.SetupSequence(p => p.ObterTempoEntregaEmMinutosIda(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()))
                .Returns(autonomiaUtilizada)
                .Returns(tempoDeIda)
                .Returns(tempoDeVolta);

            // Act
            var resultado = drone.ValidarAutonomiaSobraPorPontoEntrega(calcularTempoEntrega.Object, It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>());

            // Assert
            Assert.Equal(resultadoEsperado, resultado);
        }


    }
}
