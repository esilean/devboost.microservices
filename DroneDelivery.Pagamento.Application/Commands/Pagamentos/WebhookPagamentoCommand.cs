using DroneDelivery.Pagamento.Domain.Enums;
using DroneDelivery.Shared.Domain.Core.Commands;
using Flunt.Validations;
using Newtonsoft.Json;
using System;

namespace DroneDelivery.Pagamento.Application.Commands.Pagamentos
{
    public class WebhookPagamentoCommand : Command
    {
        public Guid PedidoId { get; }

        public PagamentoStatus Status { get; }

        [JsonConstructor]
        public WebhookPagamentoCommand(Guid pedidoId, PagamentoStatus status)
        {
            PedidoId = pedidoId;
            Status = status;
        }

        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotEmpty(PedidoId, nameof(PedidoId), "O Id do Pedido não pode ser vazio"));
        }
    }

}
