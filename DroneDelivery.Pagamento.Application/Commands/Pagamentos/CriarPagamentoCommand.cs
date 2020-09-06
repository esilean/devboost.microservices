using DroneDelivery.Shared.Domain.Core.Commands;
using Flunt.Validations;
using Newtonsoft.Json;
using System;

namespace DroneDelivery.Pagamento.Application.Commands.Pagamentos
{
    public class CriarPagamentoCommand : Command
    {
        public Guid PedidoId { get; private set; }

        public string NumeroCartao { get; }

        public DateTime VencimentoCartao { get; }

        public int CodigoSeguranca { get; }

        [JsonConstructor]
        public CriarPagamentoCommand(string numeroCartao, DateTime vencimentoCartao, int codigoSeguranca)
        {
            NumeroCartao = numeroCartao;
            VencimentoCartao = vencimentoCartao;
            CodigoSeguranca = codigoSeguranca;
        }

        public void MarcarPedidoId(Guid id)
        {
            PedidoId = id;
        }

        public void Validate()
        {

            AddNotifications(new Contract()
                .Requires()
                .IsNotEmpty(PedidoId, nameof(PedidoId), "O Id do Pedido não pode ser vazio"));

            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrWhiteSpace(NumeroCartao, nameof(NumeroCartao), "O Número do Cartão não pode ser vazio"));

            AddNotifications(new Contract()
                .Requires()
                .IsNotNull(VencimentoCartao, nameof(VencimentoCartao), "O Vencimento do Cartão não pode ser vazio"));

            AddNotifications(new Contract()
                .Requires()
                .IsGreaterThan(CodigoSeguranca, 0, nameof(CodigoSeguranca), "O Código de Segurança tem que ser maior que zero"));
        }
    }
}
