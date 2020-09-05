using DroneDelivery.Domain.Interfaces;
using Geolocation;

namespace DroneDelivery.Infra.TempoEntrega
{
    public class CalcularTempoEntrega : ICalcularTempoEntrega
    {
        public double ObterTempoEntregaEmMinutos(double latitudeOrigem, double longitudeOrigem, double latitudeDestino, double longitudeDestino, double velocidade)
        {
            double distance = GeoCalculator.GetDistance(latitudeOrigem, longitudeOrigem, latitudeDestino, longitudeDestino, 1, DistanceUnit.Meters);
            if (distance <= 0)
                return 0;

            //velocidade em m/s
            //T = d / v
            var tempoEmMinutos = ((distance * 2) / velocidade) / 60;

            return tempoEmMinutos;
        }

        public double ObterTempoEntregaEmMinutosIda(double latitudeOrigem, double longitudeOrigem, double latitudeDestino, double longitudeDestino, double velocidade)
        {
            double distance = GeoCalculator.GetDistance(latitudeOrigem, longitudeOrigem, latitudeDestino, longitudeDestino, 1, DistanceUnit.Meters);
            if (distance <= 0)
                return 0;

            //velocidade em m/s
            //T = d / v
            var tempoEmMinutos = ((distance) / velocidade) / 60;

            return tempoEmMinutos;
        }
    }
}
