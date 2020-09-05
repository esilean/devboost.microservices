using DroneDelivery.Pagamento.Application.Dtos;
using DroneDelivery.Pagamento.Application.Interfaces;
using DroneDelivery.Pagamento.Data.Repositorios.Interfaces;
using DroneDelivery.Pagamento.Domain.Enums;
using DroneDelivery.Pagamento.Domain.Models;
using System;
using System.Threading.Tasks;

namespace DroneDelivery.Pagamento.Application.Services
{
    public class PedidoPagamentoService : IPedidoPagamentoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEnviarRespostaPagamento _enviarRespostaPagamento;

        public PedidoPagamentoService(IUnitOfWork unitOfWork, IEnviarRespostaPagamento enviarRespostaPagamento)
        {
            _unitOfWork = unitOfWork;
            _enviarRespostaPagamento = enviarRespostaPagamento;
        }

        public async Task RealizarPagamento(Guid pedidoId, CriarPedidoPagamentoDto criarPedidoPagamentoDto)
        {
            //validacoes do DTO


            var pedido = await _unitOfWork.Pedidos.ObterPorIdAsync(pedidoId);
            if (pedido == null)
                return;

            if (pedido.Status != PedidoStatus.AguardandoPagamento)
                return;


            var pagamento = new PedidoPagamento(
                pedido,
                criarPedidoPagamentoDto.NumeroCartao,
                criarPedidoPagamentoDto.VencimentoCartao,
                criarPedidoPagamentoDto.CodigoSeguranca);

            var status = pagamento.ValidarCartao() ? PagamentoStatus.Aprovado : PagamentoStatus.Reprovado;
            pagamento.AtualizarStatus(status);

            pedido.AdicionarPagamento(pagamento);
            pedido.AtualizarStatus(PedidoStatus.Pago);


            //// publicar uma resposta
            await _enviarRespostaPagamento.EnviarConfirmacaoPagamento(new CriarRepostaPagamentoDto
            {
                PedidoId = pedido.PedidoId,
                Status = pedido.Status
            });


            await _unitOfWork.Pedidos.AdicionarPagamentoAsync(pagamento);
            await _unitOfWork.SaveAsync();
        }
    }
}
