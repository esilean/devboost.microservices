namespace DroneDelivery.Domain.Interfaces
{
    public interface ICalcularTempoEntrega
    {
        double ObterTempoEntregaEmMinutos(double latitudeOrigem, double longitudeOrigem, double latitudeDestino, double longitudeDestino, double velocidade);

        double ObterTempoEntregaEmMinutosIda(double latitudeOrigem, double longitudeOrigem, double latitudeDestino, double longitudeDestino, double velocidade);
    }
}
