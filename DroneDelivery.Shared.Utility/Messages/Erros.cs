namespace DroneDelivery.Shared.Utility.Messages
{
    public static class Erros
    {
        //Usuario
        public static readonly string ErroCliente_NaoEncontrado = "Cliente não foi encontrado.";
        public static readonly string ErroCliente_EmailJaExiste = "Este email já foi utilizado.";
        public static readonly string ErroCliente_UsuarioOuSenhaInvalido = "Usuário ou senha inválidos.";
        public static readonly string ErroCliente_RefreshTokenNaoAutorizado = "Token não está autorizado para ser atualizado.";

        //Drone
        public static readonly string ErroDrone_NaoEncontrado = "Drone não foi encontrado.";
        public static readonly string ErroDrone_NaoCadastrado = "Não existe drone cadastrado.";
        public static readonly string ErroDrone_CapacidadeMaxima = $"Capacidade do pedido não pode ser maior que {Utils.CAPACIDADE_MAXIMA_GRAMAS / 1000} KGs.";

        //Pedido
        public static readonly string ErroPedido_NaoEncontrado = "Pedido não foi encontrado.";
        public static readonly string ErroPedido_CapacidadeMaxima = $"Capacidade do pedido não pode ser maior que {Utils.CAPACIDADE_MAXIMA_GRAMAS / 1000} KGs.";
        public static readonly string ErroPedido_PedidoJaFoiEntregue = "Este pedido já foi entregue ao Cliente.";
        public static readonly string ErroPedido_PedidoNaoEstaAguardandoPgto = "Este pedido não pode ser pago. Não está com status \"Aguardando Pagamento\"";
        public static readonly string ErroPedido_PedidoNaoEstaProcessandoPgto = "Este pedido não pode ser alterado. Não está com status \"Processando Pagamento\"";
        

        //Pagamentos
        public static readonly string ErroPagamento_CartaoInvalido = "Cartão informado é inválido.";
        public static readonly string ErroPagamento_UltimoRegistroPgto = "Nenhum pagamento em aberto foi encontrado.";

    }
}
