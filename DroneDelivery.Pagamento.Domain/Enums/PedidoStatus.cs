namespace DroneDelivery.Pagamento.Domain.Enums
{
    public enum PedidoStatus
    {
        AguardandoEntrega = 1,
        EmEntrega = 2,
        Entregue = 3,
        Cancelado = 4,
        AguardandoPagamento = 5,
        Pago = 6
    }
}
