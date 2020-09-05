namespace DroneDelivery.Utility.Messages
{
    public static class PedidoMessage
    {
        public static readonly string Erro_ClienteNaoEncontrado = "Cliente não foi encontrado";
        public static readonly string Erro_CapacidadeMaximaPedido = $"Capacidade do pedido não pode ser maior que {Utils.CAPACIDADE_MAXIMA_GRAMAS / 1000} KGs";
        public static readonly string Erro_DroneNaoCadastrado = "Não existe drone cadastrado";
    }
}
