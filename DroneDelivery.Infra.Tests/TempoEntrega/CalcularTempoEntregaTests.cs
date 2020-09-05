using DroneDelivery.Domain.Interfaces;
using DroneDelivery.Infra.TempoEntrega;
using Xunit;

namespace DroneDelivery.Infra.Tests.TempoEntrega
{
    public class CalcularTempoEntregaTests
    {
        private readonly ICalcularTempoEntrega _calcularTempoEntrega;

        public CalcularTempoEntregaTests()
        {
            _calcularTempoEntrega = new CalcularTempoEntrega();
        }


        [Theory(DisplayName = "Calcular Tempo Entrega Somente Ida com Sucesso")]
        [InlineData(-23.5950753, -46.645421, -23.5880684, -46.6564195, 3.33333, 6.8250068250068248)]
        [InlineData(-23.5880684, -46.6564195, -23.5880684, -46.6564195, 3.33333, 0)]
        public void TempoEntrega_CalcularTempoEntregaIda_DeveRetornarTempoEmMinutos(double lat1, double lat2, double long1, double long2, double velocidade, double resultado)
        {
            //Arrange
            //Act
            var tempoMinutos = _calcularTempoEntrega.ObterTempoEntregaEmMinutosIda(lat1, lat2, long1, long2, velocidade);

            Assert.True(tempoMinutos == resultado);
        }

        [Theory(DisplayName = "Calcular Tempo Entrega Ida e Volta com Sucesso")]
        [InlineData(-23.5950753, -46.645421, -23.5880684, -46.6564195, 3.33333, 6.8250068250068248)]
        [InlineData(-23.5880684, -46.6564195, -23.5880684, -46.6564195, 3.33333, 0)]
        public void TempoEntrega_CalcularTempoEntregaIdaEVolta_DeveRetornarTempoEmMinutos(double lat1, double lat2, double long1, double long2, double velocidade, double resultado)
        {
            //Arrange
            //Act
            var tempoMinutos = _calcularTempoEntrega.ObterTempoEntregaEmMinutos(lat1, lat2, long1, long2, velocidade);

            Assert.True(tempoMinutos == resultado * 2);
        }
    }
}
