namespace DroneDelivery.Shared.Domain.Core.Enums
{
    public enum PedidoStatus
    {
        Pendente = 0,
        AguardandoPagamento = 1,
        ProcessandoPagamento = 2,
        Pago = 3,
        AguardandoDrone = 4,
        EmEntrega = 5,
        Entregue = 6,
        Cancelado = 7
    }
}
